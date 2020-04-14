using System;
using System.Collections.Generic;

namespace Percolate.Models
{
    public interface IPercolateEntity
    {
        public ICollection<PercolateProperty> Properties { get; set; }

        public Type Type { get; set; }

        public bool? IsPagingEnabled { get; set; }

        public bool? IsSortingEnabled { get; set; }

        public bool? IsFilteringEnabled { get; set; }

        public int? DefaultPageSize { get; set; }

        public int? MaximumPageSize { get; set; }
    }
}
