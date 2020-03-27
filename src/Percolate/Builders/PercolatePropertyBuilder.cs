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
     
        public PercolatePropertyBuilder<TProperty> AllowSorting(bool allowSorting = true)
        {
            Model.IsSortable = allowSorting;
            return this;
        }

        public PercolatePropertyBuilder<TProperty> AllowFiltering(bool allowFiltering = true)
        {
            Model.IsFilterable = allowFiltering;
            return this;
        }
    }
}
