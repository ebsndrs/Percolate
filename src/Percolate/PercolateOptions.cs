namespace Percolate
{
    public class PercolateOptions
    {
        public PercolateOptions()
        {
            IsPercolateEnabledGlobally = false;
            DoExceptionsFailSilently = true;
            DoResponsesIncludeMetadata = true;
            IsPagingEnabled = true;
            IsSortingEnabled = true;
            IsFilteringEnabled = true;
            DefaultPageSize = 100;
            MaximumPageSize = 1000;
        }

        public bool IsPercolateEnabledGlobally { get; set; }

        public bool DoExceptionsFailSilently { get; set; }

        public bool DoResponsesIncludeMetadata { get; set; }

        public bool IsPagingEnabled { get; set; }

        public bool IsSortingEnabled { get; set; }

        public bool IsFilteringEnabled { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaximumPageSize { get; set; }
    }
}
