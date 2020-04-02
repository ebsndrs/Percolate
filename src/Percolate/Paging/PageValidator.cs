using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Models;

namespace Percolate.Paging
{
    public static class PageValidator
    {
        public static PageValidationRules GetPageQueryValidationRules(EnablePercolateAttribute attribute, IPercolateType model, PercolateOptions options)
        {
            return new PageValidationRules
            {
                MaximumPageSize = PageHelper.GetMaximumPageSize(attribute, model, options)
            };
        }

        public static void ValidatePageQuery(PageQuery query, PageValidationRules rules)
        {
            if (query.Page < 1)
            {
                throw new ParameterValidationException($"The page query parameter \"{query.Page}\" is less than 1.");
            }

            if (query.PageSize < 1)
            {
                throw new ParameterValidationException($"The page size query parameter \"{query.PageSize}\" is less than 1.");
            }

            if (query.PageSize > rules.MaximumPageSize)
            {
                throw new ParameterValidationException($"The page size query parameter \"{query.PageSize}\" exceeds the maximum page size of \"{rules.MaximumPageSize}\"");
            }
        }
    }
}
