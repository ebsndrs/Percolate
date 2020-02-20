namespace Percolate
{
    public class PercolateOptions
    {
        public bool EnablePercolateGlobally { get; set; }

        public bool EnablePagingGlobally { get; set; }

        public bool EnableSortingGlobally { get; set; }

        public bool EnableFilteringGlobally { get; set; }

        public bool EnableSelectingGlobally { get; set; }

        public bool EnableExpandingGlobally { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaxPageSize { get; set; }
    }
}
