using Percolate.Filtering;
using Percolate.Paging;
using Percolate.Sorting;

namespace Percolate.Models
{
    public class QueryModel
    {
        public PageQuery PageQueryModel { get; set; }

        public SortQuery SortQueryModel { get; set; }

        public FilterQuery FilterQueryModel { get; set; }
    }
}
