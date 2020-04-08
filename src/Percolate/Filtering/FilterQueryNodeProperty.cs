namespace Percolate.Filtering
{
    public class FilterQueryNodeProperty
    {
        public string Name { get; set; }

        public FilterQueryClauseOperator PreviousOperator { get; set; }
    }
}
