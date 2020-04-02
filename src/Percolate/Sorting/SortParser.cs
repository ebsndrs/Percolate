using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Percolate.Sorting
{
    public static class SortParser
    {
        public static SortQuery ParseSortQuery(IQueryCollection queryCollection)
        {
            var query = new SortQuery();

            if (queryCollection.ContainsKey("sort"))
            {
                query.Nodes = queryCollection["sort"]
                    .ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(queryString => ParseSortQueryNode(queryString));
            }

            return query;
        }

        private static SortQueryNode ParseSortQueryNode(string query)
        {
            var pattern = @"^[-]?\w+$";

            if (Regex.IsMatch(query, pattern))
            {
                if (query.StartsWith('-'))
                {
                    return new SortQueryNode
                    {
                        Name = query.Remove(0, 1),
                        Direction = SortQueryDirection.Descending
                    };
                }
                else
                {
                    return new SortQueryNode
                    {
                        Name = query,
                        Direction = SortQueryDirection.Ascending
                    };
                }
            }
            else
            {
                throw new ParameterParsingException($"The sort query parameter \"{query}\" is not in a valid format.");
            }
        }
    }
}
