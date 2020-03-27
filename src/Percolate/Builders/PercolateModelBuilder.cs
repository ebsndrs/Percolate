using System.Linq;

namespace Percolate.Builders
{
    public class PercolateModelBuilder
    {
        public PercolateModelBuilder()
        {
            Model = new PercolateModel(this);
        }

        internal PercolateModel Model { get; set; }

        public PercolateTypeBuilder<TType> Type<TType>() where TType : class
        {
            PercolateTypeBuilder<TType> typeBuilder;

            var existingTypeModel = Model.Types.FirstOrDefault(t => t.Type is TType);

            if (existingTypeModel == null)
            {
                typeBuilder = new PercolateTypeBuilder<TType>();
                Model.Types.Add(typeBuilder.Model);
            }
            else
            {
                typeBuilder = new PercolateTypeBuilder<TType>(existingTypeModel);
            }

            return typeBuilder;
        }
    }
}
