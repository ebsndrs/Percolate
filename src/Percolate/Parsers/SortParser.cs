using Microsoft.AspNetCore.Http;
using Percolate.Models.Sorting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Percolate.Parsers
{
    internal static class SortParser
    {
        internal static SortModel ParseSortParameter(IQueryCollection queryCollection)
        {
            var sortModel = new SortModel();

            if (queryCollection.ContainsKey("sort"))
            {
                var queryStrings = queryCollection["sort"].ToString().Split(',');
                sortModel.Nodes = ParseSortParameterNodes(queryStrings);
            }

            return sortModel;
        }

        private static IEnumerable<SortNode> ParseSortParameterNodes(string[] queryStrings)
        {
            return queryStrings.Select(queryString => ParseSortParameterNode(queryString));
        }

        private static SortNode ParseSortParameterNode(string rawValue)
        {
            var pattern = @"^[-]{1}\w+";

            if (Regex.IsMatch(rawValue, pattern))
            {
                return new SortNode
                {
                    PropertyName = rawValue.Replace("-", string.Empty),
                    Direction = SortDirection.Descending
                };
            }
            else
            {
                return new SortNode
                {
                    PropertyName = rawValue,
                    Direction = SortDirection.Ascending
                };
            }
        }
    }
}
