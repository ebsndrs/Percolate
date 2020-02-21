using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Models.Paging;
using System;

namespace Percolate.Parsers
{
    internal static class PagingParser
    {
        internal static PagingModel ParsePagingParameters(ActionExecutedContext actionExecutedContext)
        {
            var PagingModel = new PagingModel();

            if (actionExecutedContext.HttpContext.Request.Query.ContainsKey("page"))
            {
                var queryStrings = actionExecutedContext.HttpContext.Request.Query["page"].ToString().Split(',');
                PagingModel.Page = ParsePageParameter(queryStrings);
            }

            if (actionExecutedContext.HttpContext.Request.Query.ContainsKey("pageSize"))
            {
                var queryStrings = actionExecutedContext.HttpContext.Request.Query["pageSize"].ToString().Split(',');
                PagingModel.PageSize = ParsePageSizeParameter(queryStrings);
            }

            return PagingModel;
        }

        private static int ParsePageParameter(string[] queryStrings)
        {
            if (queryStrings.Length != 1)
            {
                throw new ParameterParsingException();
            }

            try
            {
                return int.Parse(queryStrings[0]);
            }
            catch (FormatException)
            {
                throw new ParameterParsingException();
            }
        }

        private static int ParsePageSizeParameter(string[] queryStrings)
        {
            if (queryStrings.Length != 1)
            {
                throw new ParameterParsingException();
            }

            try
            {
                return int.Parse(queryStrings[0]);
            }
            catch (FormatException)
            {
                throw new ParameterParsingException();
            }
        }
    }
}
