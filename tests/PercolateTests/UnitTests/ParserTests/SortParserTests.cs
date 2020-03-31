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

            var sortList = sortString.Split(',').ToList();

            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "sort", sortString }
            });

            var result = SortParser.ParseSortParameter(queryCollection);

            var resultNodes = result.Nodes.ToList();

            Assert.All(resultNodes, (node) =>
            {
                var itemToCompareAgainst = sortList.ElementAt(resultNodes.IndexOf(node));
                var directionToCompare = itemToCompareAgainst.StartsWith('-') ? SortQueryDirection.Descending : SortQueryDirection.Ascending;

                if (directionToCompare == SortQueryDirection.Ascending)
                    Assert.Equal(itemToCompareAgainst, node.PropertyName);
                else
                    Assert.Equal(itemToCompareAgainst.Remove(0, 1), node.PropertyName);

                Assert.Equal(directionToCompare, node.Direction);
            });
        }

        [Fact]
        public void ParseSortParameters_WhenCalledWithInvalidQueryParameter_ThrowsException()
        {
            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "sort", "foo,-bar,>=spam" }
            });

            Assert.Throws<ParameterParsingException>(() => SortParser.ParseSortParameter(queryCollection).Nodes.ToList());
        }
    }
}
