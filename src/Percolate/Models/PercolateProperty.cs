using System;
using System.Reflection;

namespace Percolate.Models
{
    public class PercolateProperty
    {
        public PercolateProperty(PropertyInfo propertyInfo)
        {
            Name = propertyInfo.Name;
            Type = propertyInfo.PropertyType;
            IsSortingAllowed = null;
            IsFilteringAllowed = null;
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public bool? IsSortingAllowed { get; set; }

        public bool? IsFilteringAllowed { get; set; }
    }
}
