using System.Linq;

namespace Percolate.Builders
{
    public class PercolateModelBuilder
    {
        protected PercolateModel model;

        public PercolateModelBuilder()
        {
            model = new PercolateModel(this);
        }

        public PercolateTypeBuilder<TType> Type<TType>() where TType : class
        {
            PercolateTypeBuilder<TType> builder;

            var existingModel = model.Types.FirstOrDefault(t => t.Type is TType);

            if (existingModel != null)
            {
                builder = new PercolateTypeBuilder<TType>(existingModel);
            }
            else
            {
                builder = new PercolateTypeBuilder<TType>();
                model.Types.Add(builder.Build());
            }

            return builder;
        }

        internal PercolateModel Build() => model;
    }
}
