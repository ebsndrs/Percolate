using System.Collections.Generic;

namespace Percolate.Models.Sorting
{
    public class SortQueryModel
    {
        public SortQueryModel()
        {
            Nodes = new List<SortQueryNode>();
        }

        public IEnumerable<SortQueryNode> Nodes { get; set; }
    }
}
