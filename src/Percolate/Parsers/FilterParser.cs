using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using Percolate.Models.Filtering;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Percolate.Parsers
{
    internal static class FilterParser
    {
        private static readonly Dictionary<FilterOperator, string> filterOperators = new Dictionary<FilterOperator, string>()
        {
            { FilterOperator.Equals, "==" },
            { FilterOperator.DoesNotEqual, "!=" },
            { FilterOperator.GreaterThan, ">" },
            { FilterOperator.LessThan, "<" },
            { FilterOperator.GreaterThanOrEqual, ">=" },
            { FilterOperator.LessThanOrEqual, "<=" }
        };

        internal static FilterModel ParseFilterParameter(IQueryCollection queryCollection)
        {
            var filterModel = new FilterModel();

            if (queryCollection.ContainsKey("filter"))
            {
                var queryStrings = queryCollection["filter"].ToString().Split(',');
                filterModel.Nodes = ParseFilterParameterNodes(queryStrings);
            }

            return filterModel;
        }

        private static IEnumerable<FilterNode> ParseFilterParameterNodes(string[] queryStrings)
        {
            return queryStrings.Select(queryString => ParseFilterParameterNode(queryString));
        }

        private static FilterNode ParseFilterParameterNode(string queryString)
        {
            //loop through the possible operators to try and find a match
            foreach (var op in filterOperators)
            {
                //insert the operator string into our regex pattern to check if it matches
                //the regex only matches if exactly one of the filter operators is present in the string
                //this is more certain than string.Contains() because it ensures an exact character match
                if (Regex.IsMatch(queryString, $@"^\w+({op.Value}){{1}}[^=!><][\s\S]+$"))
                {
                    var values = queryString.Split(op.Value);
                    return new FilterNode
                    {
                        PropertyName = values[0],
                        Operator = op.Key,
                        FilterValue = values[1]
                    };
                }
            }

            //if we reached the end of the loop, no match was found
            throw new ParameterParsingException();
        }
    }
}
