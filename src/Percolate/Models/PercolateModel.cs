using Percolate.Models.Filtering;
using Percolate.Models.Paging;
using Percolate.Models.Sorting;

namespace Percolate.Models
{
    class PercolateModel
    {
        public PagingModel PageModel { get; set; }

        public SortModel SortModel { get; set; }

        public FilterModel FilterModel { get; set; }
    }
}
