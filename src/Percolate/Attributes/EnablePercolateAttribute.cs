using System;

namespace Percolate.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class EnablePercolateAttribute : Attribute
    {
        public PercolateAttributeSetting PagingSetting { get; set; }

        public PercolateAttributeSetting SortingSetting { get; set; }

        public PercolateAttributeSetting FilteringSetting { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaximumPageSize { get; set; }
    }
}
