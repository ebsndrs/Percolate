using System.Collections.Generic;

namespace Percolate.Models.Filtering
{
    public class FilterQueryModel
    {
        public FilterQueryModel()
        {
            Nodes = new List<FilterQueryNode>();
        }

        public IEnumerable<FilterQueryNode> Nodes { get; set; }
    }
}
