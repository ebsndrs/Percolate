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
     
        public PercolatePropertyBuilder<TProperty> EnableSorting(bool enableSorting = true)
        {
            Model.IsSortingEnabled = enableSorting;
            return this;
        }

        public PercolatePropertyBuilder<TProperty> EnableFiltering(bool enableFiltering = true)
        {
            Model.IsFilteringEnabled = enableFiltering;
            return this;
        }
    }
}
