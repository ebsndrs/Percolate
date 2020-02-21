using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using Percolate.Models.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Percolate.Parsers
{
    public static class FilterParser
    {
        public static FilterModel ParseFilterParameter(IQueryCollection queryCollection)
        {
            var filterModel = new FilterModel();

            if (queryCollection.ContainsKey("filter"))
            {
                var queryStrings = queryCollection["filter"].ToString().Split(',');
                filterModel.Nodes = ParseFilterParameterNodes(queryStrings);
            }

            return filterModel;
        }

        private static Dictionary<FilterOperator, string> filterOperators => new Dictionary<FilterOperator, string>()
        {
            { FilterOperator.Equals, "==" },
            { FilterOperator.DoesNotEqual, "!=" },
            { FilterOperator.GreaterThan, ">" },
            { FilterOperator.LessThan, "<" },
            { FilterOperator.GreaterThanOrEqual, ">=" },
            { FilterOperator.LessThanOrEqual, "<=" }
        };

        private static IEnumerable<FilterNode> ParseFilterParameterNodes(string[] queryStrings)
        {
            return queryStrings.Select(queryString => ParseFilterParameterNode(queryString));
        }

        private static FilterNode ParseFilterParameterNode(string queryString)
        {
            FilterOperator? filterOperator = null;
            Range operatorRange = new Range(0, 0);

            for (int i = 0; i < queryString.Length; i++)
            {
                //only evaluate chars that aren't the first or last chars of the string
                if (i > 0 && i < queryString.Length - 1)
                {
                    //grab the current, next, and previous chars
                    var current = queryString[i];
                    var previous = queryString[i - 1];
                    var next = queryString[i + 1];

                    /* now, evaluate what the current char is. If it's one of our potential filter operators,
                     * now, evaluate what the current char is. If it's one of our potential filter operators, 
                     * break the loop because we've identified the operator that query string is using.
                     * This allows for a queryString like abc==<=!=xyz to be valid: it will evaluate to abc (==) <=!=xyz
                     * We did it this way because the filter value might contain characters that are possible operators.
                     * For example, one might want to filter on a string that contains a ! character. This is the most generous
                     * parsing possible. Further checks on the value can be performed in the validation step.
                     * 
                     * Once a match is found, we use a Range to determine where in the string the operator is found.
                     * This Range is used to "split" the string later on.
                     * The range begins at i and ends at i + n where n is the length of the operator.
                     */
                    if (current == '>')
                    {
                        if (next != '=')
                        {
                            filterOperator = FilterOperator.GreaterThan;
                            operatorRange = new Range(i, i + 1);
                            break;
                        }
                        else if (next == '=')
                        {
                            filterOperator = FilterOperator.GreaterThanOrEqual;
                            operatorRange = new Range(i, i + 2);
                            break;
                        }
                    }
                    else if (current == '<')
                    {
                        if (next != '=')
                        {
                            filterOperator = FilterOperator.LessThan;
                            operatorRange = new Range(i, i + 1);
                            break;
                        }
                        else if (next == '=')
                        {
                            filterOperator = FilterOperator.LessThanOrEqual;
                            operatorRange = new Range(i, i + 2);
                            break;
                        }
                    }
                    else if (current == '!')
                    {
                        if (next == '=')
                        {
                            filterOperator = FilterOperator.DoesNotEqual;
                            operatorRange = new Range(i, i + 2);
                            break;
                        }
                    }
                    else if (current == '=')
                    {
                        if (next == '=')
                        {
                            filterOperator = FilterOperator.Equals;
                            operatorRange = new Range(i, i + 2);
                            break;
                        }
                    }
                }
            }

            //If the above loop finished without assigning the filterOperator, we can assume it's a malformed parameter
            if (filterOperator == null)
            {
                throw new ParameterParsingException();
            }

            return new FilterNode
            {
                PropertyName = queryString[..operatorRange.Start],
                Operator = filterOperator.Value,
                FilterValue = queryString[operatorRange.End..]
            };
        }
    }
}
