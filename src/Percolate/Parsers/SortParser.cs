using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Percolate.Models.Sorting;
using System.Collections.Generic;
using System.Linq;

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
            var sortNode = new SortNode
            {
                PropertyName = rawValue,
                Direction = SortDirection.Ascending
            };

            if (rawValue.StartsWith("-"))
            {
                sortNode.PropertyName = rawValue.Remove(0, 1);
                sortNode.Direction = SortDirection.Descending;
            }

            return sortNode;
        }
    }
}
