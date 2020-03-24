using Percolate.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Percolate.Builders
{
    public class PercolateTypeBuilder<TType> where TType : class
    {
        protected IPercolateTypeModel model;

        public PercolateTypeBuilder()
        {
            model = new PercolateTypeModel<TType>();
        }

        public PercolateTypeBuilder(IPercolateTypeModel model)
        {
            this.model = model;
        }

        public PercolateTypeBuilder<TType> AllowPaging(bool allowPaging = true)
        {
            model.IsPageable = allowPaging;
            return this;
        }

        public PercolatePropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TType, TProperty>> propertyExpression)
        {
            PercolatePropertyBuilder<TProperty> builder;

            var memberExpression = propertyExpression.Body as MemberExpression;
            var property = memberExpression.Member as PropertyInfo;

            var existingProperty = model.Properties.FirstOrDefault(p => p.Name == property.Name);

            if (existingProperty != null)
            {
                builder = new PercolatePropertyBuilder<TProperty>(existingProperty);
            }
            else
            {
                builder = new PercolatePropertyBuilder<TProperty>(property);
                model.Properties.Add(builder.Build());
            }

            return builder;
        }

        public IPercolateTypeModel Build() => model;
    }
}
