using Percolate.Builders;
using Percolate.Models;
using System.Collections.Generic;

namespace Percolate
{
    public class PercolateModel
    {
        protected PercolateModelBuilder ModelBuilder;

        public PercolateModel()
        {
            Types = new List<IPercolateTypeModel>();

            if (ModelBuilder == null)
            {
                ModelBuilder = new PercolateModelBuilder();
            }

            Configure(ModelBuilder);

            Types = ModelBuilder.Build().Types;
        }

        public PercolateModel(PercolateModelBuilder modelBuilder)
        {
            Types = new List<IPercolateTypeModel>();

            if (modelBuilder == null && ModelBuilder == null)
            {
                ModelBuilder = new PercolateModelBuilder();
            }
            else
            {
                ModelBuilder = modelBuilder;
            }
        }

        public List<IPercolateTypeModel> Types { get; set; }

        public virtual void Configure(PercolateModelBuilder modelBuilder) { }
    }
}
