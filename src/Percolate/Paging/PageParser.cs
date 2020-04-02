using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using System;

namespace Percolate.Paging
{
    public static class PageParser
    {
        public static PageQuery ParsePageQuery(IQueryCollection queryCollection, int defaultPage, int defaultPageSize)
        {
            var query = new PageQuery();

            if (queryCollection.ContainsKey("page"))
            {
                query.Page = ParseSingleInt(queryCollection["page"].ToString(), "page");
            }
            else
            {
                query.Page = defaultPage;
            }

            if (queryCollection.ContainsKey("pageSize"))
            {
                query.PageSize = ParseSingleInt(queryCollection["pageSize"].ToString(), "pageSize");
            }
            else
            {
                query.PageSize = defaultPageSize;
            }

            return query;
        }

        private static int ParseSingleInt(string query, string name)
        {
            try
            {
                return int.Parse(query);
            }
            catch (FormatException)
            {
                throw new ParameterParsingException($"The {name} query parameter \"{query}\" could not be parsed to an integer.");
            }
            catch (OverflowException)
            {
                throw new ParameterParsingException($"The {name} query parameter \"{query}\" is to large and could not be parsed");
            }
        }
    }
}
