using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Percolate.Sorting
{
    public static class SortParser
    {
        public static SortQuery ParseSortQuery(Dictionary<string, StringValues> queryCollection)
        {
            if (queryCollection.ContainsKey("sort"))
            {
                return new SortQuery
                {
                    Nodes = queryCollection["sort"]
                    .ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(queryString => ParseSortQueryNode(queryString))
                };
            }
            else
            {
                return new SortQuery();
            }
        }

        private static SortQueryNode ParseSortQueryNode(string query)
        {
            var splitQuery = query
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(segment => segment.Trim().ToLower());

            if (splitQuery.Count() != 2)
            {
                throw new ParameterParsingException();
            }

            return new SortQueryNode
            {
                Name = splitQuery.ElementAt(0),
                Direction = splitQuery.ElementAt(1) switch
                {
                    "asc" => SortQueryDirection.Ascending,
                    "desc" => SortQueryDirection.Descending,
                    _ => throw new ParameterParsingException()
                }
            };
        }
    }
}
