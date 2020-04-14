using System;
using System.Collections.Generic;
using System.Linq;

namespace Percolate.Models
{
    public class PercolateType<TType> : IPercolateEntity where TType : class
    {
        public PercolateType()
        {
            Properties = typeof(TType)
                .GetProperties()
                .Select(p => new PercolateProperty(p))
                .ToList();

            Type = typeof(TType);
            IsPagingEnabled = null;
            IsSortingEnabled = null;
            IsFilteringEnabled = null;
            DefaultPageSize = null;
            MaximumPageSize = null;
        }

        public ICollection<PercolateProperty> Properties { get; set; }

        public Type Type { get; set; }

        public bool? IsPagingEnabled { get; set; }

        public bool? IsSortingEnabled { get; set; }

        public bool? IsFilteringEnabled { get; set; }

        public int? DefaultPageSize { get; set; }

        public int? MaximumPageSize { get; set; }
    }
}
