using Microsoft.AspNetCore.Http;
using Percolate.Models;

namespace Percolate
{
    public interface IPercolateService
    {
        public PercolateOptions Options { get; set; }

        public PercolateModel BuildModel(IQueryCollection queryCollection);
    }
}
