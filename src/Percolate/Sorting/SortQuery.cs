using System.Collections.Generic;

namespace Percolate.Sorting
{
    public class SortQuery
    {
        public SortQuery()
        {
            Nodes = new HashSet<SortQueryNode>();
        }

        public IEnumerable<SortQueryNode> Nodes { get; set; }
    }
}
