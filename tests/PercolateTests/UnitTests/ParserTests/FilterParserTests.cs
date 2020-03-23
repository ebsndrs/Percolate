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

            var result = FilterParser.ParseFilterQuery(queryCollection);

            Assert.Empty(result.Nodes);
        }

        [Fact]
        public void ParseFilterParameters_WhenCalledWithValidQueryParameter_ReturnsParsedValues()
        {
            var expectedResults = new List<FilterNode>
            {
                new FilterNode
                {
                    RawNode = "name=James",
                    Properties = new string[] { "name" },
                    Values = new string[] { "James" },
                    Operator = "=",
                    ParsedOperator = FilterOperator.Equals,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = "country!=USA",
                    Properties = new string[] { "country" },
                    Values = new string[] { "USA" },
                    Operator = "!=",
                    ParsedOperator = FilterOperator.DoesNotEqual,
                    IsOperatorNegated = true
                },
                new FilterNode
                {
                    RawNode = "age>20",
                    Properties = new string[] { "age" },
                    Values = new string[] { "20" },
                    Operator = ">",
                    ParsedOperator = FilterOperator.GreaterThan,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = $"birthday<12/2/1994",
                    Properties = new string[] { "birthday" },
                    Values = new string[] { "12/2/1994" },
                    Operator = "<",
                    ParsedOperator = FilterOperator.LessThan,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = "likes>=20",
                    Properties = new string[] { "likes" },
                    Values = new string[] { "20" },
                    Operator = ">=",
                    ParsedOperator = FilterOperator.GreaterThanOrEqual,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = "posts<=10",
                    Properties = new string[] { "posts" },
                    Values = new string[] { "10" },
                    Operator = "<=",
                    ParsedOperator = FilterOperator.LessThanOrEqual,
                    IsOperatorNegated = false
                },

                //some wonky ones with multiple operators
                new FilterNode
                {
                    RawNode = "x=!>=!==y",
                    Properties = new string[] { "x" },
                    Values = new string[] { "!>=!==y" },
                    Operator = "=",
                    ParsedOperator = FilterOperator.Equals,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = "i=<=>=!==<>j",
                    Properties = new string[] { "i" },
                    Values = new string[] { "<=>=!==<>j" },
                    Operator = "=",
                    ParsedOperator = FilterOperator.Equals,
                    IsOperatorNegated = false
                },

                //some ones with pipe delimited properties and values
                new FilterNode
                {
                    RawNode = "age|posts>20",
                    Properties = new string[] { "age", "posts"},
                    Values = new string[] { "20"},
                    Operator = ">",
                    ParsedOperator = FilterOperator.GreaterThan,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = "name=Amy|Joe",
                    Properties = new string[] { "name" },
                    Values = new string[] { "Amy", "Joe"},
                    Operator = "=",
                    ParsedOperator = FilterOperator.Equals,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = "age|posts!=13|45",
                    Properties = new string[] { "age", "posts" },
                    Values = new string[] { "13", "45" },
                    Operator = "!=",
                    ParsedOperator = FilterOperator.DoesNotEqual,
                    IsOperatorNegated = true
                },

                //and some with escaped pipes and commas in the value
                new FilterNode
                {
                    RawNode = @"text=hello\, world!",
                    Properties = new string[] { "text" },
                    Values = new string[] { "hello, world!" },
                    Operator = "=",
                    ParsedOperator = FilterOperator.Equals,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = @"text=i\|hate\|regex",
                    Properties = new string[] { "text" },
                    Values = new string[] { "i|hate|regex" },
                    Operator = "=",
                    ParsedOperator = FilterOperator.Equals,
                    IsOperatorNegated = false
                },
                new FilterNode
                {
                    RawNode = "name|city!=Jane Doe|Pawnee",
                    Properties = new string[] { "name", "city" },
                    Values = new string[] { "Jane Doe", "Pawnee" },
                    Operator = "!=",
                    ParsedOperator = FilterOperator.DoesNotEqual,
                    IsOperatorNegated = true
                }
            };

            var store = new Dictionary<string, StringValues>()
            {
                { "filter", string.Join(',', expectedResults.Select(node => node.RawNode)) }
            };

            var queryCollection = new QueryCollection(store);

            var result = FilterParser.ParseFilterQuery(queryCollection);

            var resultNodes = result.Nodes.ToList();
            
            foreach (var node in resultNodes)
            {
                //for any given node, the index of expected results and the parsed results should match
                var expectedResult = expectedResults.ElementAt(resultNodes.IndexOf(node));

                Assert.Equal(expectedResult.RawNode, node.RawNode);
                Assert.Equal(expectedResult.Properties, node.Properties);
                Assert.Equal(expectedResult.Values, node.Values);
                Assert.Equal(expectedResult.Operator, node.Operator);
                Assert.Equal(expectedResult.ParsedOperator, node.ParsedOperator);
                Assert.Equal(expectedResult.IsOperatorNegated, node.IsOperatorNegated);
            }
        }

        [Fact]
        public void ParseFilterParameters_WhenCalledWithInvalidParameter_ThrowsException()
        {
            //invalid operator "=!="
            var filterString = "foo=";

            var store = new Dictionary<string, StringValues>()
            {
                { "filter", filterString }
            };

            var queryCollection = new QueryCollection(store);

            Assert.Throws<ParameterParsingException>(() => FilterParser.ParseFilterQuery(queryCollection));
        }
    }
}
