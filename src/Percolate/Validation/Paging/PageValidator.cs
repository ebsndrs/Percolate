using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Models;
using Percolate.Models.Paging;

namespace Percolate.Validation.Paging
{
    public static class PageValidator
    {
        public static PageValidationRules BuildPageValidationRules(IPercolateTypeModel typeModel, PercolateOptions options, EnablePercolateAttribute attribute)
        {
            var pageValidationRules = new PageValidationRules
            {
                IsPagingEnabled = attribute.PagingSetting switch
                {
                    PercolateAttributeSetting.Enabled => true,
                    PercolateAttributeSetting.Disabled => false,
                    _ => typeModel.IsPagingEnabled ?? options.IsPagingEnabled,
                },

                MaximumPageSize = attribute.MaximumPageSize switch
                {
                    0 => typeModel.MaximumPageSize ?? options.MaximumPageSize,
                    _ => attribute.MaximumPageSize
                }
            };

            return pageValidationRules;
        }

        public static void ValidatePageParameters(PageQueryModel queryModel, PageValidationRules rules)
        {
            if (!rules.IsPagingEnabled && (queryModel.Page.HasValue || queryModel.PageSize.HasValue))
                throw new ParameterValidationException("Paging is not allowed with the current configuration.");

            if (queryModel.Page.HasValue)
            {
                if (queryModel.Page.Value < 1)
                    throw new ParameterValidationException($"The page query parameter \"{queryModel.Page.Value}\" is less than 1.");
            }

            if (queryModel.PageSize.HasValue)
            {
                if (queryModel.PageSize.Value < 1)
                    throw new ParameterValidationException($"The page size query parameter \"{queryModel.PageSize.Value}\" is less than 1.");

                if (queryModel.PageSize.Value > rules.MaximumPageSize)
                    throw new ParameterValidationException($"The page size query parameter \"{queryModel.PageSize.Value}\" exceeds the maximum page size of \"{rules.MaximumPageSize}\"");
            }
        }
    }
}
