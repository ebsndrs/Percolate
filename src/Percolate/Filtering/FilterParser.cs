using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Extensions;
using System;
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
        private static readonly Dictionary<string, FilterQueryNodeOperator> nodeOperators = new Dictionary<string, FilterQueryNodeOperator>()
        {
            //commented out operators are not supported yet

            { " eq ", FilterQueryNodeOperator.IsEqual },
            { " ieq ", FilterQueryNodeOperator.CaseInsensitiveIsEqual },
            { " ne ", FilterQueryNodeOperator.IsNotEqual },
            { " ine ", FilterQueryNodeOperator.CaseInsensitiveIsNotEqual },
            { " gt ", FilterQueryNodeOperator.IsGreaterThan },
            { " ge ", FilterQueryNodeOperator.IsGreaterThanOrEqual },
            { " lt ", FilterQueryNodeOperator.IsLessThan },
            { " le ", FilterQueryNodeOperator.IsLessThanOrEqual },
            { " c ", FilterQueryNodeOperator.DoesContain },
            { " ic ", FilterQueryNodeOperator.CaseInsensitiveDoesContain },
            { " nc ", FilterQueryNodeOperator.DoesNotContain },
            { " inc ", FilterQueryNodeOperator.CaseInsensitiveDoesNotContain },
            { " sw ", FilterQueryNodeOperator.DoesStartWith },
            { " isw ", FilterQueryNodeOperator.CaseInsensitiveDoesStartWith },
            { " nsw ", FilterQueryNodeOperator.DoesNotStartWith },
            { " insw ", FilterQueryNodeOperator.CaseInsensitiveDoesNotStartWith },
            { " ew ", FilterQueryNodeOperator.DoesEndWith },
            { " iew ", FilterQueryNodeOperator.CaseInsensitiveDoesEndWith },
            { " new ", FilterQueryNodeOperator.DoesNotEndWith },
            { " inew ", FilterQueryNodeOperator.CaseInsensitiveDoesNotEndWith }
        };

        private static readonly Dictionary<string, FilterQueryClauseOperator> clauseOperators = new Dictionary<string, FilterQueryClauseOperator>()
        {
            { "and", FilterQueryClauseOperator.And },
            { "or" , FilterQueryClauseOperator.Or},
        };

        public static FilterQuery ParseFilterQuery(Dictionary<string, StringValues> queryCollection)
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
            //find the operator; it'll be the first operator that appears in the string with at least 1 space on each side
            string @operator = IdentifyNodeOperator(value);

            FilterQueryNodeOperator parsedOperator = nodeOperators[@operator];

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

            return new FilterQueryNode
            {
                Properties = ParseProperties(splitFilterQuery[0]),
                Values = ParseValues(splitFilterQuery[1]),
                Operator = parsedOperator,
                IsOperatorNegated = IsFilterOperatorNegated(parsedOperator)
            };
        }

        private static bool IsFilterOperatorNegated(FilterQueryNodeOperator @operator)
        {
            return @operator == FilterQueryNodeOperator.IsNotEqual;
        }

        private static string IdentifyNodeOperator(string node)
        {
            var foundOperators = nodeOperators
                .Where(@operator => node.Contains(@operator.Key, StringComparison.InvariantCultureIgnoreCase));

            if (foundOperators.Count() != 1)
            {
                var exceptionMessage = foundOperators.Any() ?
                    $"More than one valid operator was found in the filter query parameter \"{node}\"." :
                    $"No valid operator was found in the filter query parameter \"{node}\".";

                throw new ParameterParsingException(exceptionMessage);
            }

            return foundOperators.Single().Key;

            #region old code
            //keeping this here in case we decide to also support symbol operators
            //var nodeAsArray = node.ToCharArray();

            //for (int i = 0; i < nodeAsArray.Length; i++)
            //{
            //    var current = nodeAsArray[i];

            //    if (current == '=')
            //    {
            //        return $"{current}";
            //    }

            //    if (i != nodeAsArray.Length - 1)
            //    {
            //        if (current == '!' || current == '>' || current == '<')
            //        {
            //            var next = nodeAsArray[i + 1];
            //            if (next == '=')
            //            {
            //                return $"{current}{next}";
            //            }
            //            else if (current != '!')
            //            {
            //                return $"{current}";
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        private static IEnumerable<FilterQueryNodeProperty> ParseProperties(string propertiesClause)
        {
            //a clause should follow the format <propertyName> <clause operator> <propertyName2>...etc.
            //thus, even (including 0) indexes are properties, and odd indexes are operators
            //because of this, properties cannot be named "And" or "Or". This is enforced elsewhere but we still check for it here

            var splitPropertySegments = propertiesClause
                .Split(' ')
                .Select(segment => segment.Trim())
                .ToArray();

            var properties = new List<FilterQueryNodeProperty>();

            foreach (var (segment, index) in splitPropertySegments.WithIndex())
            {
                //If the index is odd, the segment must be an operator. Otherwise, it must not be an operator.
                if (index % 2 != 0)
                {
                    if (!clauseOperators.Keys.Contains(segment, StringComparer.InvariantCultureIgnoreCase))
                    {
                        throw new ParameterParsingException();
                    }
                }
                else
                {
                    if (clauseOperators.Keys.Contains(segment, StringComparer.InvariantCultureIgnoreCase))
                    {
                        throw new ParameterParsingException();
                    }

                    var property = new FilterQueryNodeProperty
                    {
                        Name = segment,
                        PreviousOperator = index != 0 ? clauseOperators[splitPropertySegments[index - 1]] : FilterQueryClauseOperator.None
                    };

                    properties.Add(property);
                }
            }

            return properties;
        }

        private static IEnumerable<string> ParseValues(string valuesClause)
        {
            //splits the values clause on whitespace that is not enclosed in single spaces
            //https://stackoverflow.com/questions/28789842/split-string-while-ignoring-escaped-character
            var pattern = @"('[^']*?(?:\\'[^']*?)*'|[^\s]+)";

            var segments = Regex
                .Split(valuesClause, pattern)
                .Where(segment => !string.IsNullOrWhiteSpace(segment))
                .Select(segment =>
                {
                    var formattedSegment = segment
                        .Trim()
                        .Replace("\\", "");

                    if (formattedSegment.StartsWith('\''))
                    {
                        formattedSegment = formattedSegment.Remove(0, 1);
                    }
                    if (formattedSegment.EndsWith('\''))
                    {
                        formattedSegment = formattedSegment.Remove(formattedSegment.Length - 1, 1);
                    }

                    return formattedSegment;
                })
                .ToArray();

            var values = new List<string>();

            foreach (var (segment, index) in segments.WithIndex())
            {
                //If the index is odd, the segment must be 'or'. Otherwise, it can be any value.
                if (index % 2 != 0)
                {
                    if (!string.Equals(segment, "or", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new ParameterParsingException();
                    }
                    else if (segments.Length > 1 && index == segments.Length - 1)
                    {
                        //the final segment cannot be "or" if there is more than 1
                        //for example, "name eq or" would work but "name eq or or" would not because we expect another value after the last "or"
                        throw new ParameterParsingException();
                    }
                }
                else
                {
                    values.Add(segment);
                }
            }

            return values;
        }

        private static IEnumerable<string> IdentifyClauseOperators(string clause)
        {
            return clauseOperators
                .Where(@operator => clause.Contains(@operator.Key, StringComparison.InvariantCultureIgnoreCase))
                .Select(pair => pair.Key);
        }
    }
}
