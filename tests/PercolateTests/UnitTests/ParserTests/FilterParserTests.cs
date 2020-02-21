using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Percolate.Exceptions;
using Percolate.Models.Filtering;
using Percolate.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;

namespace PercolateTests.UnitTests.ParserTests
{
    public class FilterParserTests
    {
        [Fact]
        public void ParseFilterParameters_WhenCalledWithNoQueryParameter_ReturnsDefaultValues()
        {
            var queryCollection = new QueryCollection();

            var result = FilterParser.ParseFilterParameter(queryCollection);

            Assert.Empty(result.Nodes);
        }

        [Fact]
        public void ParseFilterParameters_WhenCalledWithValidQueryParameter_ReturnsParsedValues()
        {
            //Disable the switch code warning because we know our switch will always return some value
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

            //first, let's add some "normal" filter strings
            var filterString = "foo==bar,spam!=eggs,pie>1,lol<49,bippity>=boppity,giveMe<=theZoppity";

            //next, let's add some oddities
            filterString.Concat(",x=!>=!===y,i==<=>=!===<>j");

            var filterArray = filterString.Split(',');

            var store = new Dictionary<string, StringValues>()
            {
                { "filter", filterString }
            };

            var queryCollection = new QueryCollection(store);

            var result = FilterParser.ParseFilterParameter(queryCollection);

            for (int i = 0; i < filterArray.Length; i++)
            {
                var item = filterArray[i];
                var resultToCompare = result.Nodes.ElementAt(i);

                var operatorToTest = item switch
                {
                    "foo==bar" => FilterOperator.Equals,
                    "spam!=eggs" => FilterOperator.DoesNotEqual,
                    "pie>1" => FilterOperator.GreaterThan,
                    "lol<49" => FilterOperator.LessThan,
                    "bippity>=boppity" => FilterOperator.GreaterThanOrEqual,
                    "giveMe<=theZoppity" => FilterOperator.LessThanOrEqual,
                    "x=!>=!===y" => FilterOperator.GreaterThanOrEqual,
                    "i==<=>=!===<>j" => FilterOperator.Equals
                };

                var splitItem = operatorToTest switch
                {
                    FilterOperator.Equals => item.Split("==", 2),
                    FilterOperator.DoesNotEqual => item.Split("!=", 2),
                    FilterOperator.GreaterThan => item.Split(">", 2),
                    FilterOperator.LessThan => item.Split("<", 2),
                    FilterOperator.GreaterThanOrEqual => item.Split(">=", 2),
                    FilterOperator.LessThanOrEqual => item.Split("<=", 2)
                };

                Assert.Equal(splitItem[0], resultToCompare.PropertyName);
                Assert.Equal(operatorToTest, resultToCompare.Operator);
                Assert.Equal(splitItem[1], resultToCompare.FilterValue);
            }
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        }

        [Fact]
        public void ParserFilterParameters_WhenCalledWithInvalidParameter_ThrowsException()
        {
            //invalid operator "=!="
            var filterString = "foo=!=bar";

            var store = new Dictionary<string, StringValues>()
            {
                { "filter", filterString }
            };

            var queryCollection = new QueryCollection(store);

            try
            {
                var result = FilterParser.ParseFilterParameter(queryCollection);
            }
            catch (Exception e)
            {
                Assert.True(e is ParameterParsingException);
            }
        }
    }
}
