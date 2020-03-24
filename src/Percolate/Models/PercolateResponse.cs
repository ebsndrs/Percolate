namespace Percolate.Models
{
    public class PercolateResponse<T> where T : class
    {
        public T Value { get; set; }

        public QueryModel Metadata { get; set; }
    }
}
