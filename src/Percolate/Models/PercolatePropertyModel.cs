using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Percolate.Models
{
    public class PercolatePropertyModel
    {
        public PercolatePropertyModel(PropertyInfo propertyInfo)
        {
            Name = propertyInfo.Name;
            Type = propertyInfo.PropertyType;
            IsSortable = true;
            IsFilterable = true;
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public bool IsSortable { get; set; }

        public bool IsFilterable { get; set; }
    }
}
