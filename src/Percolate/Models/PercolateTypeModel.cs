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
            Properties = new List<PercolatePropertyModel>();

            var properties = typeof(TType).GetProperties();

            Properties = typeof(TType)
                .GetProperties()
                .Select(p => new PercolatePropertyModel(p))
                .ToList();

            Type = typeof(TType);
            IsPageable = true;
        }

        public List<PercolatePropertyModel> Properties { get; set; }

        public Type Type { get; set; }

        public bool IsPageable { get; set; }
    }
}
