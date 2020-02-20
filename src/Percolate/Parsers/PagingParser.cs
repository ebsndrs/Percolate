using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Models.Paging;
using System;

namespace Percolate.Parsers
{
    static class PagingParser
    {
        static PagingModel ParsePagingParameters(ActionExecutedContext actionExecutedContext)
        {
            var PagingModel = new PagingModel();

            if (actionExecutedContext.HttpContext.Request.Query.ContainsKey("page"))
            {
                PagingModel.Page = ParsePageParameter(actionExecutedContext.HttpContext.Request.Query["page"]);
            }

            if (actionExecutedContext.HttpContext.Request.Query.ContainsKey("pageSize"))
            {
                PagingModel.PageSize = ParsePageSizeParameter(actionExecutedContext.HttpContext.Request.Query["pageSize"]);
            }

            return PagingModel;
        }

        private static int ParsePageParameter(StringValues rawValues)
        {
            if (rawValues.Count != 1)
            {
                throw new ParameterParsingException();
            }

            try
            {
                return int.Parse(rawValues[0]);
            }
            catch (FormatException)
            {
                throw new ParameterParsingException();
            }
        }

        private static int ParsePageSizeParameter(StringValues rawValues)
        {
            if (rawValues.Count != 1)
            {
                throw new ParameterParsingException();
            }

            try
            {
                return int.Parse(rawValues[0]);
            }
            catch (FormatException)
            {
                throw new ParameterParsingException();
            }
        }
    }
}
