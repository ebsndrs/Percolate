using System.Linq;

namespace Percolate.Models
{
    public class PercolateResponse
    {
        public IQueryable Value { get; set; }

        public QueryModel Metadata { get; set; }
    }
}
