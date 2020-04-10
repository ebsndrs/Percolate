using Percolate.Models;
using System.Collections.Generic;

namespace Percolate.Sorting
{
    public class SortValidationRules
    {
        public SortValidationRules()
        {
            DisallowedProperties = new HashSet<PercolateProperty>();
        }

        public IEnumerable<PercolateProperty> DisallowedProperties { get; set; }
    }
}
