using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Models;
using Percolate.Models.Filtering;
using System;
using System.Linq;

namespace Percolate.Validation.Filtering
{
    public static class FilterValidator
    {
        public static FilterValidationRules BuildFilterValidationRules(IPercolateTypeModel typeModel, PercolateOptions options, EnablePercolateAttribute attribute)
        {
            var filterValidationRules = new FilterValidationRules
            {
                IsFilteringEnabled = attribute.FilteringSetting switch
                {
                    PercolateAttributeSetting.Enabled => true,
                    PercolateAttributeSetting.Disabled => false,
                    _ => typeModel.IsFilteringEnabled ?? options.IsFilteringEnabled,
                }
            };

            return filterValidationRules;
        }

        public static void ValidateFilterParameters(FilterQueryModel queryModel, FilterValidationRules rules)
        {
            if (!rules.IsFilteringEnabled && queryModel.Nodes.Any())
                throw new ParameterValidationException($"Filtering is not allowed with the current configuration.");
        }
    }
}
