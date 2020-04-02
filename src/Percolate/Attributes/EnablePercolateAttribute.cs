using System;

namespace Percolate.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class EnablePercolateAttribute : Attribute
    {
        public PercolateAttributeSetting PageSetting { get; set; }

        public PercolateAttributeSetting SortSetting { get; set; }

        public PercolateAttributeSetting FilterSetting { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaximumPageSize { get; set; }
    }
}
