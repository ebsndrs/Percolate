using Percolate.Attributes;
using Percolate.Validation.Paging;
using System;
using System.Linq;
using Percolate.Models;
using Percolate.Exceptions;

namespace Percolate.Validation
{
    public static class ValidationHelper
    {
        public static ValidationRules BuildValidationRules(Type type, PercolateOptions options, PercolateModel model, EnablePercolateAttribute attribute)
        {
            var typeModel = model.Types
                .FirstOrDefault(t => t.Type == type);

            if (typeModel == null)
            {
                throw new ParameterValidationException();
            }

            return new ValidationRules
            {
                PageValidationRules = BuildPageValidationRules(typeModel, options, attribute)
            };
        }

        private static PageValidationRules BuildPageValidationRules(IPercolateTypeModel typeModel, PercolateOptions options, EnablePercolateAttribute attribute)
        {
            var pageValidationRules = new PageValidationRules
            {
                IsPagingAllowed = attribute.PagingSetting switch
                {
                    PercolateAttributeSetting.Enabled => true,
                    PercolateAttributeSetting.Disabled => false,
                    _ => typeModel.IsPagingEnabled ?? options.IsPagingEnabled,
                },

                MaxPageSize = attribute.MaximumPageSize switch
                {
                    0 => typeModel.MaximumPageSize ?? options.MaximumPageSize,
                    _ => attribute.MaximumPageSize
                }
            };

            return pageValidationRules;
        }
    }
}
