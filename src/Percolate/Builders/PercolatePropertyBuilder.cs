using Percolate.Models;
using System.Reflection;

namespace Percolate.Builders
{
    public class PercolatePropertyBuilder<TProperty>
    {
        protected PercolatePropertyModel model;

        public PercolatePropertyBuilder(PropertyInfo propertyInfo)
        {
            model = new PercolatePropertyModel(propertyInfo);
        }

        public PercolatePropertyBuilder(PercolatePropertyModel model)
        {
            this.model = model;
        }
     
        public PercolatePropertyBuilder<TProperty> AllowSorting(bool allowSorting = true)
        {
            model.IsSortable = allowSorting;
            return this;
        }

        public PercolatePropertyBuilder<TProperty> AllowFiltering(bool allowFiltering = true)
        {
            model.IsFilterable = allowFiltering;
            return this;
        }

        internal PercolatePropertyModel Build() => model;
    }
}
