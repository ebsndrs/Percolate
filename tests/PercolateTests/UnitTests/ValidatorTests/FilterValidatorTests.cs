using Percolate.Exceptions;
using Percolate.Models.Filtering;
using Percolate.Validation.Filtering;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PercolateTests.UnitTests.ValidatorTests
{
    public class FilterValidatorTests
    {
        [Fact]
        public void ValidateFilterParameters_WhenCalledWithValidParameters_DoesNotThrowException()
        {
            var query = new FilterQueryModel
            {
                Nodes = new List<FilterQueryNode>()
                {
                    new FilterQueryNode
                    {
                        RawNode = "foo=bar",
                        Properties = new string[] { "foo" },
                        Values = new string[] { "bar" },
                        Operator = "=",
                        ParsedOperator = FilterQueryOperator.Equals
                    },
                    new FilterQueryNode
                    {
                        RawNode = "spam>1",
                        Properties = new string[] { "spam" },
                        Values = new string[] { "1" },
                        Operator = ">",
                        ParsedOperator = FilterQueryOperator.GreaterThan
                    }
                }
            };

            var rules = new FilterValidationRules
            {
                IsFilteringEnabled = true
            };

            FilterValidator.ValidateFilterParameters(query, rules);
        }

        [Fact]
        public void ValidateFilterParameters_WhenCalledWithFilteringDisabled_ThrowsException()
        {
            var query = new FilterQueryModel
            {
                Nodes = new List<FilterQueryNode>()
                {
                    new FilterQueryNode
                    {
                        RawNode = "foo=bar",
                        Properties = new string[] { "foo" },
                        Values = new string[] { "bar" },
                        Operator = "=",
                        ParsedOperator = FilterQueryOperator.Equals
                    },
                    new FilterQueryNode
                    {
                        RawNode = "spam>1",
                        Properties = new string[] { "spam" },
                        Values = new string[] { "1" },
                        Operator = ">",
                        ParsedOperator = FilterQueryOperator.GreaterThan
                    }
                }
            };

            var rules = new FilterValidationRules
            {
                IsFilteringEnabled = false
            };

            Assert.Throws<ParameterValidationException>(() => FilterValidator.ValidateFilterParameters(query, rules));
        }

        [Fact]
        public void ValidateFilterParameters_WhenCalledWithFilteringDisabledButNoNodes_DoesNotThrowException()
        {
            var query = new FilterQueryModel
            {
                Nodes = new List<FilterQueryNode>()
            };

            var rules = new FilterValidationRules
            {
                IsFilteringEnabled = false
            };

            FilterValidator.ValidateFilterParameters(query, rules);
        }
    }
}
