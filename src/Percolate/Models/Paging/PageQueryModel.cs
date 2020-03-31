namespace Percolate.Models.Paging
{
    public class PageQueryModel
    {
        public PageQueryModel()
        {
            Page = null;
            PageSize = null;
        }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}
