using System;

namespace Percolate.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class EnablePercolateAttribute : Attribute
    {
        public bool AllowPaging { get; set; }

        public bool AllowSorting { get; set; }

        public bool AllowFiltering { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaxPageSize { get; set; }
    }
}
