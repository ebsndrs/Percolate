using Percolate.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Percolate.Models
{
    public class PercolateTypeModel<TType> : IPercolateTypeModel where TType : class
    {
        public PercolateTypeModel()
        {
            Properties = typeof(TType)
                .GetProperties()
                .Select(p => new PercolatePropertyModel(p))
                .ToList();

            Type = typeof(TType);
            IsPagingEnabled = null;
            IsSortingEnabled = null;
            IsFilteringEnabled = null;
            DefaultPageSize = null;
            MaximumPageSize = null;
        }

        public ICollection<PercolatePropertyModel> Properties { get; set; }

        public Type Type { get; set; }

        public bool? IsPagingEnabled { get; set; }

        public bool? IsSortingEnabled { get; set; }

        public bool? IsFilteringEnabled { get; set; }

        public int? DefaultPageSize { get; set; }

        public int? MaximumPageSize { get; set; }
    }
}
