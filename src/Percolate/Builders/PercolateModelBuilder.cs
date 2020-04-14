using System.Linq;

namespace Percolate.Builders
{
    public class PercolateModelBuilder
    {
        public PercolateModelBuilder()
        {
            Model = new PercolateModel(this);
        }

        public PercolateModel Model { get; set; }

        public PercolateEntityBuilder<TEntity> Entity<TEntity>() where TEntity : class
        {
            PercolateEntityBuilder<TEntity> entityBuilder;

            var existingEntity = Model.Entities
                .FirstOrDefault(t => t.Type is TEntity);

            if (existingEntity == default)
            {
                entityBuilder = new PercolateEntityBuilder<TEntity>();
                Model.Entities.Add(entityBuilder.Model);
            }
            else
            {
                entityBuilder = new PercolateEntityBuilder<TEntity>(existingEntity);
            }

            return entityBuilder;
        }
    }
}
