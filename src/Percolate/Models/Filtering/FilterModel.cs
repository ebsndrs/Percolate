using System.Collections.Generic;

namespace Percolate.Models.Filtering
{
    public class FilterModel
    {
        public FilterModel()
        {
            Nodes = new List<FilterNode>();
        }

        public IEnumerable<FilterNode> Nodes { get; set; }
    }
}
