using Percolate.Models;
using System.Reflection;

namespace Percolate.Builders
{
    public class PercolatePropertyBuilder<TProperty>
    {
        public PercolatePropertyBuilder(PropertyInfo propertyInfo)
        {
            Model = new PercolateProperty(propertyInfo);
        }

        public PercolatePropertyBuilder(PercolateProperty model)
        {
            Model = model;
        }

        public PercolateProperty Model { get; set; }

        public PercolatePropertyBuilder<TProperty> CanSort(bool canSort = true)
        {
            Model.IsSortingAllowed = canSort;
            return this;
        }

        public PercolatePropertyBuilder<TProperty> CanFilter(bool canFilter = true)
        {
            Model.IsFilteringAllowed = canFilter;
            return this;
        }
    }
}
