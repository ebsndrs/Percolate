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
            Entities = new List<IPercolateEntity>();

            if (ModelBuilder == null)
            {
                ModelBuilder = new PercolateModelBuilder();
            }

            Configure(ModelBuilder);

            Entities = ModelBuilder.Model.Entities;
        }

        public PercolateModel(PercolateModelBuilder modelBuilder)
        {
            Entities = new List<IPercolateEntity>();

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

        public List<IPercolateEntity> Entities { get; set; }

        public IPercolateEntity GetEntity<T>()
        {
            return Entities
                .FirstOrDefault(entity => entity.Type == typeof(T));
        }

        public virtual void Configure(PercolateModelBuilder modelBuilder) { }
    }
}
