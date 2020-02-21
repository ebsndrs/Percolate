using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Models.Filtering;
using System.Collections.Generic;
using System.Linq;

namespace Percolate.Parsers
{
    internal static class FilterParser
    {
        private static readonly Dictionary<FilterOperator, string> operatorDictionary = new Dictionary<FilterOperator, string>()
        {
            { FilterOperator.Equals, "==" },
            { FilterOperator.DoesNotEqual, "!=" },
            { FilterOperator.GreaterThanOrEqual, ">=" },
            { FilterOperator.LessThanOrEqual, "<=" },
            { FilterOperator.GreaterThan, ">" },
            { FilterOperator.LessThan, "<" },
        };

        internal static FilterModel ParseFilterParameter(ActionExecutedContext actionExecutedContext)
        {
            var sortModel = new FilterModel();

            if (actionExecutedContext.HttpContext.Request.Query.ContainsKey("filter"))
            {
                var queryStrings = actionExecutedContext.HttpContext.Request.Query["filter"].ToString().Split(',');
                sortModel.Nodes = ParseFilterParameterNodes(queryStrings);
            }

            return sortModel;
        }

        private static IEnumerable<FilterNode> ParseFilterParameterNodes(string[] queryStrings)
        {
            return queryStrings.Select(queryString => ParseFilterParameterNode(queryString));
        }

        private static FilterNode ParseFilterParameterNode(string queryString)
        {
            bool operatorSearchFailed = true;
            FilterOperator? filterOperator = null;
            string[] values = new string[3];

            foreach (var op in operatorDictionary)
            {
                if (queryString.Contains(op.Value))
                {
                    values = queryString.Split(op.Value);
                    filterOperator = op.Key;
                    operatorSearchFailed = false;
                    break;
                }
            }

            if (operatorSearchFailed)
            {
                throw new ParameterParsingException();
            }

            return new FilterNode
            {
                PropertyName = values[0],
                Operator = filterOperator.Value,
                FilterValue = values[1]
            };
        }
    }
}
