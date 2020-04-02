using Percolate.Models;
using System.Collections.Generic;

namespace Percolate.Filtering
{
    public class FilterValidationRules
    {
        public FilterValidationRules()
        {
            DisallowedProperties = new List<PercolateProperty>();
        }

        public IEnumerable<PercolateProperty> DisallowedProperties { get; set; }
    }
}
