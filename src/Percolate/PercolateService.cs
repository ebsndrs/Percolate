namespace Percolate
{
    public class PercolateService : IPercolateService
    {
        public PercolateService()
        {
            Options = new PercolateOptions();
        }

        public PercolateOptions Options { get; set; }
    }
}
