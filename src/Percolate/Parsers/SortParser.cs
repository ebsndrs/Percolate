using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Percolate.Models.Sorting;
using System.Collections.Generic;

namespace Percolate.Parsers
{
    static class SortParser
    {
        static SortModel ParseSortParameter(ActionExecutedContext actionExecutedContext)
        {
            var sortModel = new SortModel();

            if (actionExecutedContext.HttpContext.Request.Query.ContainsKey("sort"))
            {
                sortModel.Nodes = ParseSortParameterNodes(actionExecutedContext.HttpContext.Request.Query["page"]);
            }

            return sortModel;
        }

        private static IEnumerable<SortNode> ParseSortParameterNodes(StringValues rawValues)
        {
            var sortNodes = new List<SortNode>();

            foreach (var node in rawValues)
            {
                sortNodes.Add(ParseSortParameterNode(node));
            }

            return sortNodes;
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
