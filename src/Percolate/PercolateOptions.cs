namespace Percolate
{
    public class PercolateOptions
    {
        public PercolateOptions()
        {
            EnablePercolateGlobally = false;
            DefaultPageSize = 10;
            MaxPageSize = 1000;
            FailSilently = false;
        }

        public bool EnablePercolateGlobally { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaxPageSize { get; set; }

        public bool FailSilently { get; set; }
    }
}
