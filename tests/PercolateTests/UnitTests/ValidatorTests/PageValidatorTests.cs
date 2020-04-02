using Percolate.Exceptions;
using Percolate.Paging;
using Xunit;

namespace PercolateTests.UnitTests.ValidatorTests
{
    public class PageValidatorTests
    {
        [Fact]
        public void ValidatePageParameters_WhenCalledWithValidParameters_DoesNotThrowException()
        {
            var query = new PageQuery
            {
                Page = 1,
                PageSize = 100
            };

            var rules = new PageValidationRules
            {
                MaximumPageSize = 100
            };

            PageValidator.ValidatePageQuery(query, rules);
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithPagingDisabled_ThrowsException()
        {
            var query = new PageQuery
            {
                Page = 1,
                PageSize = 100
            };

            var rules = new PageValidationRules
            {
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageQuery(query, rules));
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithOutOfUpperBoundsPageSize_ThrowsException()
        {
            var query = new PageQuery
            {
                Page = 1,
                PageSize = 101
            };

            var rules = new PageValidationRules
            {
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageQuery(query, rules));
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithOutOfLowerBoundsPage_ThrowsException()
        {
            var query = new PageQuery
            {
                Page = 0,
                PageSize = 100
            };

            var rules = new PageValidationRules
            {
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageQuery(query, rules));
        }

        [Fact]
        public void ValidatePageParameters_WhenCalledWithOutOfLowerBoundsPageSize_ThrowsException()
        {
            var query = new PageQuery
            {
                Page = 1,
                PageSize = 0
            };

            var rules = new PageValidationRules
            {
                MaximumPageSize = 100
            };

            Assert.Throws<ParameterValidationException>(() => PageValidator.ValidatePageQuery(query, rules));
        }
    }
}
