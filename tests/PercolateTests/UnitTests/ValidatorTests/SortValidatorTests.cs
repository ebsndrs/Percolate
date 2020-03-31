using Percolate.Exceptions;
using Percolate.Models.Sorting;
using Percolate.Validation.Sorting;
using System.Collections.Generic;
using Xunit;

namespace PercolateTests.UnitTests.ValidatorTests
{
    public class SortValidatorTests
    {
        [Fact]
        public void ValidateSortParameters_WhenCalledWithValidParameters_DoesNotThrowException()
        {
            var query = new SortQueryModel
            {
                Nodes = new List<SortQueryNode>()
                {
                    new SortQueryNode
                    {
                        PropertyName = "foo",
                        Direction = SortQueryDirection.Ascending
                    },
                    new SortQueryNode
                    {
                        PropertyName = "bar",
                        Direction = SortQueryDirection.Descending
                    }
                }
            };

            var rules = new SortValidationRules
            {
                IsSortingEnabled = true
            };

            SortValidator.ValidateSortParameters(query, rules);
        }

        [Fact]
        public void ValidateSortParameters_WhenCalledWithSortingDisabled_ThrowsException()
        {
            var query = new SortQueryModel
            {
                Nodes = new List<SortQueryNode>()
                {
                    new SortQueryNode
                    {
                        PropertyName = "foo",
                        Direction = SortQueryDirection.Ascending
                    },
                    new SortQueryNode
                    {
                        PropertyName = "bar",
                        Direction = SortQueryDirection.Descending
                    }
                }
            };

            var rules = new SortValidationRules
            {
                IsSortingEnabled = false
            };

            Assert.Throws<ParameterValidationException>(() => SortValidator.ValidateSortParameters(query, rules));
        }

        [Fact]
        public void ValidateSortParameters_WhenCalledWithSortingDisabledButNoNodes_DoesNotThrowException()
        {
            var query = new SortQueryModel
            {
                Nodes = new List<SortQueryNode>()
            };

            var rules = new SortValidationRules
            {
                IsSortingEnabled = false
            };

            SortValidator.ValidateSortParameters(query, rules);
        }
    }
}
