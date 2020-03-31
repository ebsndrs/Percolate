using Microsoft.AspNetCore.Http;
using Percolate.Attributes;
using Percolate.Models;
using System.Linq;

namespace Percolate
{
    public interface IPercolateService<TPercolateModel> where TPercolateModel : PercolateModel
    {
        public PercolateOptions Options { get; set; }

        public TPercolateModel Model { get; set; }

        public QueryModel BuildQuery(IQueryCollection queryCollection);

        public void ValidateQuery(IQueryable queryObject, QueryModel queryModel, EnablePercolateAttribute attribute);

        public IQueryable ApplyQuery(IQueryable queryObject, QueryModel queryModel);
    }
}
