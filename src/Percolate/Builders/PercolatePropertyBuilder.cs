using Percolate.Models;
using System.Reflection;

namespace Percolate.Builders
{
    public class PercolatePropertyBuilder<TProperty>
    {
        public PercolatePropertyBuilder(PropertyInfo propertyInfo)
        {
            Model = new PercolatePropertyModel(propertyInfo);
        }

        public PercolatePropertyBuilder(PercolatePropertyModel model)
        {
            Model = model;
        }

        internal PercolatePropertyModel Model { get; set; }
     
        public PercolatePropertyBuilder<TProperty> AllowsSorting(bool allowsSorting = true)
        {
            Model.IsSortingEnabled = allowsSorting;
            return this;
        }

        public PercolatePropertyBuilder<TProperty> AllowsFiltering(bool allowsFiltering = true)
        {
            Model.IsFilteringEnabled = allowsFiltering;
            return this;
        }
    }
}
