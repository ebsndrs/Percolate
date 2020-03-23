namespace Percolate.Models.Paging
{
    public class PageModel
    {
        public PageModel()
        {
            Page = null;
            PageSize = null;
        }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}
