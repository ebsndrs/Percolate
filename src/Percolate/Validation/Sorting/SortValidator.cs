using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Models;
using Percolate.Models.Sorting;
using System.Linq;

namespace Percolate.Validation.Sorting
{
    public static class SortValidator
    {
        public static SortValidationRules BuildSortValidationRules(IPercolateTypeModel typeModel, PercolateOptions options, EnablePercolateAttribute attribute)
        {
            var sortValidationRules = new SortValidationRules
            {
                IsSortingEnabled = attribute.SortingSetting switch
                {
                    PercolateAttributeSetting.Enabled => true,
                    PercolateAttributeSetting.Disabled => false,
                    _ => typeModel.IsSortingEnabled ?? options.IsSortingEnabled,
                }
            };

            return sortValidationRules;
        }

        public static void ValidateSortParameters(SortQueryModel queryModel, SortValidationRules rules)
        {
            if (!rules.IsSortingEnabled && queryModel.Nodes.Any())
                throw new ParameterValidationException($"Sorting is not allowed with the current configuration.");
        }
    }
}
