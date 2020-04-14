using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Sorting;
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
            var queryCollection = new Dictionary<string, StringValues>();

            var result = SortParser.ParseSortQuery(queryCollection);

            Assert.Empty(result.Nodes);
        }

        [Fact]
        public void ParseSortParameters_WhenCalledWithValidQueryParameter_ReturnsParsedValues()
        {
            var sortString = "foo,-bar,spam,-eggs";

            var sortList = sortString.Split(',').ToList();

            var queryCollection = new Dictionary<string, StringValues>()
            {
                { "sort", sortString }
            };

            var result = SortParser.ParseSortQuery(queryCollection);

            var resultNodes = result.Nodes.ToList();

            Assert.All(resultNodes, (node) =>
            {
                var itemToCompareAgainst = sortList.ElementAt(resultNodes.IndexOf(node));
                var directionToCompare = itemToCompareAgainst.StartsWith('-') ? SortQueryDirection.Descending : SortQueryDirection.Ascending;

                if (directionToCompare == SortQueryDirection.Ascending)
                {
                    Assert.Equal(itemToCompareAgainst, node.Name);
                }
                else
                {
                    Assert.Equal(itemToCompareAgainst.Remove(0, 1), node.Name);
                }

                Assert.Equal(directionToCompare, node.Direction);
            });
        }

        [Fact]
        public void ParseSortParameters_WhenCalledWithInvalidQueryParameter_ThrowsException()
        {
            var queryCollection = new Dictionary<string, StringValues>()
            {
                { "sort", "foo,-bar,>=spam" }
            };

            //have to enumerate the result to actually throw the exception
            Assert.Throws<ParameterParsingException>(() => SortParser.ParseSortQuery(queryCollection).Nodes.ToList());
        }
    }
}
