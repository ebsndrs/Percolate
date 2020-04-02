using System.Collections.Generic;

namespace Percolate.Sorting
{
    public class SortQuery
    {
        public SortQuery()
        {
            Nodes = new List<SortQueryNode>();
        }

        public IEnumerable<SortQueryNode> Nodes { get; set; }
    }
}
