using System.Collections.Generic;

namespace Percolate.Filtering
{
    public class FilterQueryNode
    {
        public FilterQueryNode()
        {
            Properties = new List<string>();
            Values = new List<string>();
        }

        public IEnumerable<string> Properties { get; set; }

        public IEnumerable<string> Values { get; set; }

        public FilterQueryOperator Operator { get; set; }

        public bool IsOperatorNegated { get; set; }
    }
}
