using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using Percolate.Models.Sorting;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Percolate.Parsers
{
    public static class SortParser
    {
        public static SortQueryModel ParseSortParameter(IQueryCollection queryCollection)
        {
            var sortModel = new SortQueryModel();

            if (queryCollection.ContainsKey("sort"))
            {
                var queryStrings = queryCollection["sort"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                sortModel.Nodes = queryStrings.Select(queryString => ParseSortParameterNode(queryString));
            }

            return sortModel;
        }

        private static SortQueryNode ParseSortParameterNode(string value)
        {
            var pattern = @"^[-]?\w+$";

            if (Regex.IsMatch(value, pattern))
            {
                if (value.StartsWith('-'))
                {
                    return new SortQueryNode
                    {
                        PropertyName = value.Remove(0, 1),
                        Direction = SortQueryDirection.Descending
                    };
                }
                else
                {
                    return new SortQueryNode
                    {
                        PropertyName = value,
                        Direction = SortQueryDirection.Ascending
                    };
                }
            }
            else
                throw new ParameterParsingException($"The sort query parameter \"{value}\" is not in a valid format.");
        }
    }
}
