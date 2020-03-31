using Percolate.Validation.Filtering;
using Percolate.Validation.Paging;
using Percolate.Validation.Sorting;

namespace Percolate.Validation
{
    public class ValidationRules
    {
        public PageValidationRules PageValidationRules { get; set; }

        public SortValidationRules SortValidationRules { get; set; }

        public FilterValidationRules FilterValidationRules { get; set; }
    }
}
