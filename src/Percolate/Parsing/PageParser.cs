using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using Percolate.Models.Paging;
using System;

namespace Percolate.Parsers
{
    public static class PageParser
    {
        public static PageQueryModel ParsePagingParameters(IQueryCollection queryCollection)
        {
            var pagingModel = new PageQueryModel();

            if (queryCollection.ContainsKey("page"))
            {
                var queryString = queryCollection["page"].ToString();
                pagingModel.Page = ParseSingleIntParameter(queryString, "page");
            }

            if (queryCollection.ContainsKey("pageSize"))
            {
                var queryString = queryCollection["pageSize"].ToString();
                pagingModel.PageSize = ParseSingleIntParameter(queryString, "pageSize");
            }

            return pagingModel;
        }

        private static int ParseSingleIntParameter(string queryString, string queryKey)
        {
            try
            {
                return int.Parse(queryString);
            }
            catch (FormatException)
            {
                throw new ParameterParsingException($"The {queryKey} query parameter \"{queryString}\" could not be parsed to an integer.");
            }
            catch (OverflowException)
            {
                throw new ParameterParsingException($"The {queryKey} query parameter \"{queryString}\" is to large and could not be parsed");
            }
        }
    }
}
