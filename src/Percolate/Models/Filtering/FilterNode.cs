namespace Percolate.Models.Filtering
{
    public class FilterNode
    {
        public string PropertyName { get; set; }

        public FilterOperator Operator { get; set; }

        public string FilterValue { get; set; }
    }
}
