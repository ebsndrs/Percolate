using System.Collections.Generic;

namespace Percolate.Filtering
{
    public class FilterQuery
    {
        public FilterQuery()
        {
            Nodes = new List<FilterQueryNode>();
        }

        public IEnumerable<FilterQueryNode> Nodes { get; set; }
    }
}
