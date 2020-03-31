using System;
using System.Collections.Generic;

namespace Percolate.Models
{
    public interface IPercolateTypeModel
    {
        public ICollection<PercolatePropertyModel> Properties { get; set; }

        public Type Type { get; set; }

        public bool? IsPagingEnabled { get; set; }

        public int? DefaultPageSize { get; set; }

        public int? MaximumPageSize { get; set; }
    }
}
