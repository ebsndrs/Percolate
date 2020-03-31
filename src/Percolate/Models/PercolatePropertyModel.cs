using System;
using System.Reflection;

namespace Percolate.Models
{
    public class PercolatePropertyModel
    {
        public PercolatePropertyModel(PropertyInfo propertyInfo)
        {
            Name = propertyInfo.Name;
            Type = propertyInfo.PropertyType;
            IsSortingEnabled = null;
            IsFilteringEnabled = null;
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public bool? IsSortingEnabled { get; set; }

        public bool? IsFilteringEnabled { get; set; }
    }
}
