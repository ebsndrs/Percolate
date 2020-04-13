using Microsoft.AspNetCore.Mvc.ModelBinding;
using Percolate.Models;

namespace Percolate.ModelBinders
{
    public class QueryModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(QueryModel))
            {
                return new QueryModelBinder();
            }

            return null;
        }
    }
}
