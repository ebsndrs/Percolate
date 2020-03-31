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

            var result = PageParser.ParsePagingParameters(queryCollection);

            Assert.Null(result.Page);
            Assert.Null(result.PageSize);
        }

        [Fact]
        public void ParsePagingParameters_WhenCalledWithNonIntParameters_ThrowsException()
        {
            //non-int page
            var queryCollection1 = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "page", "foo" },
                { "pageSize", "10" }
            });

            //non-int pageSize
            var queryCollection2 = new QueryCollection(new Dictionary<string, StringValues>()
            {
                { "page", "1" },
                { "pageSize", "bar" }
            });

            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePagingParameters(queryCollection1));
            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePagingParameters(queryCollection2));
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

            var result = PageParser.ParsePagingParameters(queryCollection);

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

            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePagingParameters(queryCollection1));
            Assert.Throws<ParameterParsingException>(() => PageParser.ParsePagingParameters(queryCollection2));
        }
    }
}
