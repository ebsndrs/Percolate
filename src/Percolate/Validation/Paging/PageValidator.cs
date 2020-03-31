using Percolate.Exceptions;
using Percolate.Models.Paging;

namespace Percolate.Validation.Paging
{
    public static class PageValidator
    {
        public static void ValidatePageParameters(PageQueryModel query, PageValidationRules rules)
        {
            if (!rules.IsPagingAllowed && (query.Page.HasValue || query.PageSize.HasValue))
                throw new ParameterValidationException("Paging is not allowed with the current configuration.");

            if (query.Page.HasValue)
            {
                if (query.Page.Value < 1)
                    throw new ParameterValidationException($"The page query parameter \"{query.Page.Value}\" is less than 1.");
            }

            if (query.PageSize.HasValue)
            {
                if (query.PageSize.Value < 1)
                    throw new ParameterValidationException($"The page size query parameter \"{query.PageSize.Value}\" is less than 1.");

                if (query.PageSize.Value > rules.MaxPageSize)
                    throw new ParameterValidationException($"The page size query parameter \"{query.PageSize.Value}\" exceeds the maximum page size of \"{rules.MaxPageSize}\"");
            }
        }
    }
}
