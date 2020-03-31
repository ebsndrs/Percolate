using System.Collections.Generic;

namespace Percolate.Models.Filtering
{
    public class FilterQueryNode
    {
        public FilterQueryNode()
        {
            Properties = new List<string>();
            Values = new List<string>();
        }

        public string RawNode { get; set; }

        public IEnumerable<string> Properties { get; set; }

        public IEnumerable<string> Values { get; set; }

        public string Operator { get; set; }

        public FilterQueryOperator ParsedOperator { get; set; }

        public bool IsOperatorNegated { get; set; }
    }
}
