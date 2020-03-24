using System;
using System.Collections.Generic;
using System.Text;

namespace Percolate.Models
{
    public interface IPercolateTypeModel
    {
        public List<PercolatePropertyModel> Properties { get; set; }

        public Type Type { get; set; }

        public bool IsPageable { get; set; }
    }
}
