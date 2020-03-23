using System.Collections.Generic;

namespace Percolate.Models.Filtering
{
    public class FilterNode
    {
        public string RawNode { get; set; }

        public IEnumerable<string> Properties { get; set; }

        public IEnumerable<string> Values { get; set; }

        public string Operator { get; set; }

        public FilterOperator ParsedOperator { get; set; }

        public bool IsOperatorNegated { get; set; }
    }
}
