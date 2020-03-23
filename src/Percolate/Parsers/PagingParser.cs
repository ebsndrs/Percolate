using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using Percolate.Models.Paging;
using System;

namespace Percolate.Parsers
{
    public static class PagingParser
    {
        public static PageModel ParsePagingParameters(IQueryCollection queryCollection)
        {
            var pagingModel = new PageModel();

            if (queryCollection.ContainsKey("page"))
            {
                var queryStrings = queryCollection["page"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                pagingModel.Page = ParsePageParameter(queryStrings);
            }

            if (queryCollection.ContainsKey("pageSize"))
            {
                var queryStrings = queryCollection["pageSize"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                pagingModel.PageSize = ParsePageSizeParameter(queryStrings);
            }

            return pagingModel;
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
