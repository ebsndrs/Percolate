using Percolate.Exceptions;
using Percolate.Models.Paging;
using Percolate.Validation.Paging;
using Xunit;

namespace PercolateTests.UnitTests.ValidatorTests
{
    public class PageValidatorTests
    {
        [Fact]
        public void ValidatePageParameters_WhenCalledWithValidParameters_DoesNotThrowException()
        {
            var query = new PageQueryModel
            {
                Page = 1,
                PageSize = 100
            };

            var rules = new PageValidationRules
            {
                IsPagingEnabled = true,
                MaximumPageSize = 100
            };

            PageValidator.ValidatePageParameters(query, rules);
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithPagingDisabled_ThrowsException()
        {
            var query = new PageQueryModel
            {
                Page = 1,
                PageSize = 100
            };

            var rules = new PageValidationRules
            {
                IsPagingEnabled = false,
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageParameters(query, rules));
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithOutOfUpperBoundsPageSize_ThrowsException()
        {
            var query = new PageQueryModel
            {
                Page = 1,
                PageSize = 101
            };

            var rules = new PageValidationRules
            {
                IsPagingEnabled = true,
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageParameters(query, rules));
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithOutOfLowerBoundsPage_ThrowsException()
        {
            var query = new PageQueryModel
            {
                Page = 0,
                PageSize = 100
            };

            var rules = new PageValidationRules
            {
                IsPagingEnabled = true,
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageParameters(query, rules));
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithOutOfLowerBoundsPageSize_ThrowsException()
        {
            var query = new PageQueryModel
            {
                Page = 1,
                PageSize = 0
            };

            var rules = new PageValidationRules
            {
                IsPagingEnabled = true,
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageParameters(query, rules));
        }
    }
}
