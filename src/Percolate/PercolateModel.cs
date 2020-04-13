using Percolate.Builders;
using Percolate.Models;
using System.Collections.Generic;
using System.Linq;

namespace Percolate
{
    public class PercolateModel
    {
        public PercolateModel()
        {
            Types = new List<IPercolateType>();

            if (ModelBuilder == null)
            {
                ModelBuilder = new PercolateModelBuilder();
            }

            Configure(ModelBuilder);

            Types = ModelBuilder.Model.Types;
        }

        public PercolateModel(PercolateModelBuilder modelBuilder)
        {
            Types = new List<IPercolateType>();

            if (modelBuilder == null && ModelBuilder == null)
            {
                ModelBuilder = new PercolateModelBuilder();
            }
            else
            {
                ModelBuilder = modelBuilder;
            }
        }

        public PercolateModelBuilder ModelBuilder { get; set; }

        public List<IPercolateType> Types { get; set; }

        public IPercolateType GetType<T>()
        {
            return Types
                .FirstOrDefault(type => type.Type == typeof(T));
        }

        public virtual void Configure(PercolateModelBuilder modelBuilder) { }
    }
}
