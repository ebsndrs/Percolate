using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using System;
using System.Collections.Generic;

namespace Percolate.Paging
{
    public static class PageParser
    {
        public static PageQuery ParsePageQuery(Dictionary<string, StringValues> queryCollection)
        {
            var query = new PageQuery();

            if (queryCollection.ContainsKey("page"))
            {
                query.Page = ParseSingleInt(queryCollection["page"].ToString(), "page");
            }

            if (queryCollection.ContainsKey("pageSize"))
            {
                query.PageSize = ParseSingleInt(queryCollection["pageSize"].ToString(), "pageSize");
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
