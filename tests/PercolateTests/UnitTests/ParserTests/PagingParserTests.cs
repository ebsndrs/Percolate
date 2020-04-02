using Microsoft.AspNetCore.Http.Internal;
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
            var queryCollection = new QueryCollection();

            var result = PageParser.ParsePageQuery(queryCollection, 1, 100);

            Assert.Equal(1, result.Page);
            Assert.Equal(100, result.PageSize);
        }

        [Fact]
        public void ParsePagingParameters_WhenCalledWithNonIntParameters_ThrowsException()
        {
            //non-int page
            var queryCollection1 = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "page", "foo" },
                { "pageSize", "100" }
            });

            //non-int pageSize
            var queryCollection2 = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "page", "1" },
                { "pageSize", "bar" }
            });

            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection1, 1, 100));
            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection2, 1, 100));
        }

        [Fact]
        public void ParsePagingParameters_WhenCalledWithValidParameters_ReturnsParsedValues()
        {
            int page = int.MaxValue;
            int pageSize = int.MaxValue;

            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() }
            });

            var result = PageParser.ParsePageQuery(queryCollection, 1, 100);

            Assert.Equal(page, result.Page);
            Assert.Equal(pageSize, result.PageSize);
        }

        [Fact]
        public void ParsePageParameters_WhenCalledWithOverflowIntParameters_ThrowsException()
        {
            //overflowing page
            var queryCollection1 = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "page", int.MaxValue.ToString() + "1" },
                { "pageSize", "100" }
            });

            //overflowing pageSize
            var queryCollection2 = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "page", "1" },
                { "pageSize", int.MaxValue.ToString() + "1" }
            });

            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection1, 1, 100));
            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePageQuery(queryCollection2, 1, 100));
        }
    }
}
