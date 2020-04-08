using System.Collections.Generic;

namespace Percolate.Filtering
{
    public class FilterQueryNode
    {
        public FilterQueryNode()
        {
            Properties = new List<FilterQueryNodeProperty>();
            Values = new List<string>();
        }

        public string RawNode { get; set; }

        public IEnumerable<FilterQueryNodeProperty> Properties { get; set; }

        public IEnumerable<string> Values { get; set; }

        public FilterQueryNodeOperator Operator { get; set; }

        public bool IsOperatorNegated { get; set; }
    }
}
