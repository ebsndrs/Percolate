using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Models.Sorting;
using Percolate.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PercolateTests.UnitTests.ParserTests
{
    public class SortParserTests
    {
        [Fact]
        public void ParseSortParameters_WhenCalledWithNoQueryParameter_ReturnsDefaultValues()
        {
            var queryCollection = new QueryCollection();

            var result = SortParser.ParseSortParameter(queryCollection);

            Assert.Empty(result.Nodes);
        }

        [Fact]
        public void ParseSortParameters_WhenCalledWithValidQueryParameter_ReturnsParsedValues()
        {
            var sortString = "foo,-bar,spam,-eggs";

            var sortArray = sortString.Split(',');

            var store = new Dictionary<string, StringValues>()
            {
                { "sort", sortString }
            };

            var queryCollection = new QueryCollection(store);

            var result = SortParser.ParseSortParameter(queryCollection);

            for (int i = 0; i < sortArray.Length; i++)
            {
                var item = sortArray[i];
                var resultToCompare = result.Nodes.ElementAt(i);
                var directionToCompare = item.StartsWith('-') ? SortDirection.Descending : SortDirection.Ascending;

                if (directionToCompare == SortDirection.Ascending)
                {
                    Assert.Equal(item, resultToCompare.PropertyName);
                }
                else
                {
                    Assert.Equal(item.Replace("-", string.Empty), resultToCompare.PropertyName);
                }

                Assert.Equal(directionToCompare, resultToCompare.Direction);
            }
        }

        [Fact]
        public void ParseSortParameters_WhenCalledWithInvalidQueryParameter_ThrowsException()
        {
            var store = new Dictionary<string, StringValues>()
            {
                { "sort", "foo,-bar,>=spam" }
            };

            var queryCollection = new QueryCollection(store);

            try
            {
                var result = SortParser.ParseSortParameter(queryCollection);
            }
            catch (Exception e)
            {
                Assert.True(e is ParameterParsingException);
            }
        }
    }
}
