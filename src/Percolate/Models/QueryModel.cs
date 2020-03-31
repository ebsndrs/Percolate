using Percolate.Models.Filtering;
using Percolate.Models.Paging;
using Percolate.Models.Sorting;

namespace Percolate.Models
{
    public class QueryModel
    {
        public PageQueryModel PageQueryModel { get; set; }

        public SortQueryModel SortQueryModel { get; set; }

        public FilterQueryModel FilterQueryModel { get; set; }
    }
}
