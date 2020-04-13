using Microsoft.Extensions.Primitives;
using Percolate.Attributes;
using Percolate.Extensions;
using Percolate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Percolate.Filtering
{
    public static class FilterHelper
    {
        public static bool IsFilteringEnabled(EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            if (attribute != default && attribute.FilterSetting != PercolateAttributeSetting.Unset)
            {
                return attribute.FilterSetting == PercolateAttributeSetting.Enabled;
            }
            else if (type.IsFilteringEnabled.HasValue)
            {
                return type.IsFilteringEnabled.Value;
            }
            else
            {
                return options.IsFilteringEnabled;
            }
        }

        public static FilterQuery ParseFilterQuery(Dictionary<string, StringValues> queryCollection)
        {
            return FilterParser.ParseFilterQuery(queryCollection);
        }

        public static void ValidateFilterQuery(FilterQuery query, IPercolateType type)
        {
            FilterValidator.ValidateFilterQuery(query, type, FilterValidator.GetFilterQueryValidationRules(type));
        }

        public static IQueryable<T> ApplyFilterQuery<T>(IQueryable<T> queryable, FilterQuery query)
        {
            var expressions = new Dictionary<FilterQueryNodeOperator, Func<Expression, Expression, Expression>>()
            {
                { FilterQueryNodeOperator.IsEqual, (member, constant) => Expression.Equal(member, constant) },
                { FilterQueryNodeOperator.CaseInsensitiveIsEqual, (member, constant) => Expression.Call(StringEquals(), member, constant, CaseInsensitiveExpression()) },
                { FilterQueryNodeOperator.IsNotEqual, (member, constant) => Expression.NotEqual(member, constant) },
                { FilterQueryNodeOperator.CaseInsensitiveIsNotEqual, (member, constant) => Expression.Not(Expression.Call(StringEquals(), member, constant, CaseInsensitiveExpression())) },
                { FilterQueryNodeOperator.IsGreaterThanOrEqual, (member, constant) => Expression.GreaterThanOrEqual(member, constant) },
                { FilterQueryNodeOperator.IsLessThanOrEqual, (member, constant) => Expression.LessThanOrEqual(member, constant) },
                { FilterQueryNodeOperator.IsGreaterThan, (member, constant) => Expression.GreaterThan(member, constant) },
                { FilterQueryNodeOperator.IsLessThan, (member, constant) => Expression.LessThan(member, constant) },
                { FilterQueryNodeOperator.DoesContain, (member, constant) => Expression.Call(member, StringContains(), constant, CaseSensitiveExpression()) },
                { FilterQueryNodeOperator.CaseInsensitiveDoesContain, (member, constant) => Expression.Call(member, StringContains(), constant, CaseInsensitiveExpression()) },
                { FilterQueryNodeOperator.DoesNotContain, (member, constant) => Expression.Not(Expression.Call(member, StringContains(), constant, CaseSensitiveExpression())) },
                { FilterQueryNodeOperator.CaseInsensitiveDoesNotContain, (member, constant) => Expression.Not(Expression.Call(member, StringContains(), constant, CaseInsensitiveExpression())) },
                { FilterQueryNodeOperator.DoesStartWith, (member, constant) => Expression.Call(member, StringStartsWith(), constant, CaseSensitiveExpression()) },
                { FilterQueryNodeOperator.CaseInsensitiveDoesStartWith, (member, constant) => Expression.Call(member, StringStartsWith(), constant, CaseInsensitiveExpression()) },
                { FilterQueryNodeOperator.DoesNotStartWith, (member, constant) => Expression.Not(Expression.Call(member, StringStartsWith(), constant, CaseSensitiveExpression())) },
                { FilterQueryNodeOperator.CaseInsensitiveDoesNotStartWith, (member, constant) => Expression.Not(Expression.Call(member, StringStartsWith(), constant, CaseInsensitiveExpression())) },
                { FilterQueryNodeOperator.DoesEndWith, (member, constant) => Expression.Call(member, StringEndsWith(), constant, CaseSensitiveExpression()) },
                { FilterQueryNodeOperator.CaseInsensitiveDoesEndWith, (member, constant) => Expression.Call(member, StringEndsWith(), constant, CaseInsensitiveExpression()) },
                { FilterQueryNodeOperator.DoesNotEndWith, (member, constant) => Expression.Not(Expression.Call(member, StringEndsWith(), constant, CaseSensitiveExpression())) },
                { FilterQueryNodeOperator.CaseInsensitiveDoesNotEndWith, (member, constant) => Expression.Not(Expression.Call(member, StringEndsWith(), constant, CaseInsensitiveExpression())) }

            };

            //short circuit the return if there are no nodes
            if (query == null || !query.Nodes.Any())
            {
                return queryable;
            }

            //define the parameter the lambda will use
            var parameterExpression = Expression.Parameter(typeof(T), "x");

            //define the empty final expression
            Expression finalExpression = Expression.Empty();

            foreach (var (node, nodeIndex) in query.Nodes.WithIndex())
            {
                //define the empty expression for the node
                Expression nodeExpression = Expression.Empty();

                foreach (var (property, propertyIndex) in node.Properties.WithIndex())
                {
                    //define the empty expression for the property
                    Expression propertyExpression = Expression.Empty();

                    //define the member expression here so that we can reuse it in the loop below
                    Expression memberExpression = parameterExpression;

                    //support for nested properties
                    foreach (var member in property.Name.Split('.'))
                    {
                        memberExpression = Expression.Property(memberExpression, member);
                    }

                    foreach (var (value, valueIndex) in node.Values.WithIndex())
                    {
                        //convert the value and create a constant expression from it
                        var convertedValue = ConvertValue(memberExpression.Type, value);
                        Expression constantExpression = Expression.Constant(convertedValue);

                        //invoke the node's associated expression method from the dictionary, passing in the converted constant
                        Expression valueExpression = expressions[node.Operator].Invoke(memberExpression, constantExpression);

                        if (valueIndex == 0)
                        {
                            //if it's the first value, override the empty propertyExpression
                            propertyExpression = valueExpression;
                        }
                        else
                        {
                            //if it isn't the first value, chain onto the previous value expressions with "OR"
                            propertyExpression = Expression.Or(propertyExpression, valueExpression);
                        }
                    }

                    if (propertyIndex == 0)
                    {
                        //if it's the first property, override the empty nodeExpression
                        nodeExpression = propertyExpression;
                    }
                    else
                    {
                        //otherwise, determine which logical chaining operator to use and chain it to the existing nodeExpression.
                        //if there is no operator then we've reached the end of the list so there is nothing to be chained
                        nodeExpression = property.PreviousOperator switch
                        {
                            FilterQueryClauseOperator.And => Expression.And(nodeExpression, propertyExpression),
                            FilterQueryClauseOperator.Or => Expression.Or(nodeExpression, propertyExpression),
                            _ => nodeExpression
                        };
                    }
                }

                if (nodeIndex == 0)
                {
                    //if it's the first node, override the empty finalExpression
                    finalExpression = nodeExpression;
                }
                else
                {
                    //otherwise, chain onto the finalExpression with "AND"
                    finalExpression = Expression.And(finalExpression, nodeExpression);
                }
            }

            var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameterExpression);

            return queryable.Where(lambda);
        }

        private static object ConvertValue(Type type, string text)
        {
            var typeConverter = TypeDescriptor.GetConverter(type);

            return typeConverter.ConvertFromString(text);
        }

        private static Expression CaseSensitiveExpression() => Expression.Constant(StringComparison.InvariantCulture);

        private static Expression CaseInsensitiveExpression() => Expression.Constant(StringComparison.InvariantCultureIgnoreCase);

        private static MethodInfo StringEquals() => typeof(string)
            .GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(string), typeof(StringComparison) });

        private static MethodInfo StringContains() => typeof(string)
            .GetMethod(nameof(string.Contains), new[] { typeof(string), typeof(StringComparison) });

        private static MethodInfo StringStartsWith() => typeof(string)
            .GetMethod(nameof(string.StartsWith), new[] { typeof(string), typeof(StringComparison) });

        private static MethodInfo StringEndsWith() => typeof(string)
            .GetMethod(nameof(string.EndsWith), new[] { typeof(string), typeof(StringComparison) });
    }
}
