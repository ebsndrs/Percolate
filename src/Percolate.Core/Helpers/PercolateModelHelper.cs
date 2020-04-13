using Percolate.Models;
using System.Linq;

namespace Percolate.Core.Helpers
{
    public static class PercolateModelHelper
    {
        public static IPercolateType GetPercolateType<T>(PercolateModel model)
        {
            return model.Types
                .FirstOrDefault(type => type.Type == typeof(T));
        }
    }
}
