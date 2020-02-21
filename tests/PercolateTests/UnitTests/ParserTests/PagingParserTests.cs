using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Parsers;
using System;
using System.Collections.Generic;
using Xunit;

namespace PercolateTests.UnitTests.ParserTests
{
    public class PagingParserTests
    {
        [Fact]
        public void ParsePagingParameters_WhenCalledWithNoQueryParameters_ReturnsNullValues()
        {
            var queryCollection = new QueryCollection();

            var result = PagingParser.ParsePagingParameters(queryCollection);

            Assert.Null(result.Page);
            Assert.Null(result.PageSize);
        }

        [Fact]
        public void ParsePagingParameters_WhenCalledWithNonIntParameters_ThrowsException()
        {
            var store = new Dictionary<string, StringValues>()
            {
                { "page", "foo" },
                { "pageSize", "bar" }
            };

            var queryCollection = new QueryCollection(store);

            try
            {
                var result = PagingParser.ParsePagingParameters(queryCollection);
            }
            catch (Exception e)
            {
                Assert.True(e is ParameterParsingException);
            }
        }

        [Fact]
        public void ParsePagingParameters_WhenCalledWithValidParameters_ReturnsParsedValues()
        {
            int page = 1;
            int pageSize = 10;

            var store = new Dictionary<string, StringValues>()
            {
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            var queryCollection = new QueryCollection(store);

            var result = PagingParser.ParsePagingParameters(queryCollection);

            Assert.Equal(page, result.Page);
            Assert.Equal(pageSize, result.PageSize);
        }
    }
}
