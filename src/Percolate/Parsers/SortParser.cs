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
        public static SortModel ParseSortParameter(IQueryCollection queryCollection)
        {
            var sortModel = new SortModel();

            if (queryCollection.ContainsKey("sort"))
            {
                var queryStrings = queryCollection["sort"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                sortModel.Nodes = queryStrings.Select(queryString => ParseSortParameterNode(queryString));
            }

            return sortModel;
        }

        private static SortNode ParseSortParameterNode(string value)
        {
            var pattern = @"^[-]?\w+$";

            if (Regex.IsMatch(value, pattern))
            {
                if (value.StartsWith('-'))
                {
                    return new SortNode
                    {
                        PropertyName = value.Replace("-", string.Empty),
                        Direction = SortDirection.Descending
                    };
                }
                else
                {
                    return new SortNode
                    {
                        PropertyName = value,
                        Direction = SortDirection.Ascending
                    };
                }
            }
            else
            {
                throw new ParameterParsingException();
            }
        }
    }
}
