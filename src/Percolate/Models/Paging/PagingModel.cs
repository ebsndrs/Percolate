namespace Percolate.Models.Paging
{
    public class PagingModel
    {
        public PagingModel()
        {
            Page = null;
            PageSize = null;
        }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}
