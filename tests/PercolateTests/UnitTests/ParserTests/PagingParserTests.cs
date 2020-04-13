using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Paging;
using System.Collections.Generic;
using Xunit;

namespace PercolateTests.UnitTests.ParserTests
{
    public class PagingParserTests
    {
        [Fact]
        public void ParsePagingParameters_WhenCalledWithNoQueryParameters_ReturnsDefaultValues()
        {
            var queryCollection = new Dictionary<string, StringValues>();

            var result = PageParser.ParsePageQuery(queryCollection);

            Assert.Null(result.Page);
            Assert.Null(result.PageSize);
        }

        [Fact]
        public void ParsePagingParameters_WhenCalledWithNonIntParameters_ThrowsException()
        {
            //non-int page
            var queryCollection1 = new Dictionary<string, StringValues>()
            {
                { "page", "foo" },
                { "pageSize", "100" }
            };

            //non-int pageSize
            var queryCollection2 = new Dictionary<string, StringValues>()
            {
                { "page", "1" },
                { "pageSize", "bar" }
            };

            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection1));
            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection2));
        }

        [Fact]
        public void ParsePagingParameters_WhenCalledWithValidParameters_ReturnsParsedValues()
        {
            int page = int.MaxValue;
            int pageSize = int.MaxValue;

            var queryCollection = new Dictionary<string, StringValues>()
            {
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            var result = PageParser.ParsePageQuery(queryCollection);

            Assert.Equal(page, result.Page);
            Assert.Equal(pageSize, result.PageSize);
        }

        [Fact]
        public void ParsePageParameters_WhenCalledWithOverflowIntParameters_ThrowsException()
        {
            //overflowing page
            var queryCollection1 = new Dictionary<string, StringValues>()
            {
                { "page", int.MaxValue.ToString() + "1" },
                { "pageSize", "100" }
            };

            //overflowing pageSize
            var queryCollection2 = new Dictionary<string, StringValues>()
            {
                { "page", "1" },
                { "pageSize", int.MaxValue.ToString() + "1" }
            };

            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection1));
            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection2));
        }
    }
}
