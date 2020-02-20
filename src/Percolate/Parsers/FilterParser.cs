using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Models.Filtering;
using System.Collections.Generic;
using System.Linq;

namespace Percolate.Parsers
{
    static class FilterParser
    {
        private static readonly Dictionary<FilterOperator, string> OperatorDictionary = new Dictionary<FilterOperator, string>()
        {
            { FilterOperator.Equals, "==" },
            { FilterOperator.DoesNotEqual, "!=" },
            { FilterOperator.GreaterThan, ">" },
            { FilterOperator.LessThan, "<" },
            { FilterOperator.GreaterThanOrEqual, ">=" },
            { FilterOperator.LessThanOrEqual, "<=" },
        };

        static FilterModel ParseFilterParameter(ActionExecutedContext actionExecutedContext)
        {
            var sortModel = new FilterModel();

            if (actionExecutedContext.HttpContext.Request.Query.ContainsKey("filter"))
            {
                sortModel.Nodes = ParseFilterParameterNodes(actionExecutedContext.HttpContext.Request.Query["filter"]);
            }

            return sortModel;
        }

        private static IEnumerable<FilterNode> ParseFilterParameterNodes(StringValues rawValues)
        {
            return rawValues.Select(value => ParseFilterParameterNode(value));
        }

        private static FilterNode ParseFilterParameterNode(string rawValue)
        {
            bool operatorSearchFailed = true;
            FilterOperator? filterOperator = null;
            string[] values = new string[3];

            foreach (var op in OperatorDictionary)
            {
                if (rawValue.Split(op.Value).Length == 3)
                {
                    values = rawValue.Split(op.Value);
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
                FilterValue = values[2]
            };
        }
    }
}
