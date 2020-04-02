using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Percolate.Filtering
{
    public static class FilterParser
    {
        //we use these regexes to split the query by non-escaped commas and pipes
        //this enables filters with commas and pipes in them while still allowing
        //multiple filters and logical OR operations
        private const string escapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";
        private const string escapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";
        private static readonly Dictionary<string, FilterQueryOperator> operators = new Dictionary<string, FilterQueryOperator>()
        {
            { "=", FilterQueryOperator.Equals },
            { "!=", FilterQueryOperator.DoesNotEqual },
            { ">=", FilterQueryOperator.GreaterThanOrEqual },
            { "<=", FilterQueryOperator.LessThanOrEqual },
            { ">", FilterQueryOperator.GreaterThan },
            { "<", FilterQueryOperator.LessThan }
        };

        public static FilterQuery ParseFilterQuery(IQueryCollection queryCollection)
        {
            var query = new FilterQuery();

            if (queryCollection.ContainsKey("filter"))
            {
                query.Nodes = Regex
                    .Split(queryCollection["filter"], escapedCommaPattern)
                    .Select(queryString => ParseFilterQueryNode(queryString));
            }

            return query;
        }

        private static FilterQueryNode ParseFilterQueryNode(string value)
        {
            //find the operator; it'll be the first operator that appears in the string
            string @operator = IdentifyOperator(value);

            FilterQueryOperator parsedOperator = operators[@operator];

            //split the value by the identified operator
            var splitFilterQuery = value
                .Split(@operator, 2)
                .Select(item => item.Trim())
                .ToArray();

            if (string.IsNullOrWhiteSpace(splitFilterQuery[0]))
            {
                throw new ParameterParsingException($"The filter query parameter \"{value}\" has an empty properties set.");
            }

            if (string.IsNullOrWhiteSpace(splitFilterQuery[1]))
            {
                throw new ParameterParsingException($"The filter query parameter \"{value}\" has an empty values set.");
            }

            //split any pipe delimited strings clean up the escape characters, and cull any duplicates
            //we considered throwing an exception in the case of duplicate properties or values, but it's easy
            //to account for and doesn't change the meaning of the filter query string
            var filterProperties = Regex
                .Split(splitFilterQuery[0], escapedPipePattern)
                .Select(property => property.Replace("\\", ""))
                .Distinct();

            var filterValues = Regex
                .Split(splitFilterQuery[1], escapedPipePattern)
                .Select(value => value.Replace("\\", ""))
                .Distinct();

            return new FilterQueryNode
            {
                Properties = filterProperties,
                Values = filterValues,
                Operator = parsedOperator,
                IsOperatorNegated = IsFilterOperatorNegated(parsedOperator)
            };
        }

        private static bool IsFilterOperatorNegated(FilterQueryOperator @operator)
        {
            return @operator == FilterQueryOperator.DoesNotEqual;
        }

        private static string IdentifyOperator(string node)
        {
            var nodeAsArray = node.ToCharArray();

            for (int i = 0; i < nodeAsArray.Length; i++)
            {
                var current = nodeAsArray[i];

                if (current == '=')
                {
                    return $"{current}";
                }

                if (i != nodeAsArray.Length - 1)
                {
                    if (current == '!' || current == '>' || current == '<')
                    {
                        var next = nodeAsArray[i + 1];
                        if (next == '=')
                        {
                            return $"{current}{next}";
                        }
                        else if (current != '!')
                        {
                            return $"{current}";
                        }
                    }
                }
            }

            throw new ParameterParsingException($"No valid operator was found in the filter query parameter \"{node}\".");
        }
    }
}
