using System.Collections.Generic;

namespace Percolate.Models.Sorting
{
    public class SortModel
    {
        public SortModel()
        {
            Nodes = new List<SortNode>();
        }

        public IEnumerable<SortNode> Nodes { get; set; }
    }
}
