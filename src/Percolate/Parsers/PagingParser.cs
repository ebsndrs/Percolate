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
                pagingModel.Page = ParseIntParameter(queryStrings);
            }

            if (queryCollection.ContainsKey("pageSize"))
            {
                var queryStrings = queryCollection["pageSize"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                pagingModel.PageSize = ParseIntParameter(queryStrings);
            }

            return pagingModel;
        }

        private static int ParseIntParameter(string[] queryStrings)
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
