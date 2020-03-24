using Microsoft.AspNetCore.Http;
using Percolate.Models;

namespace Percolate
{
    public interface IPercolateService<TPercolateModel> where TPercolateModel : PercolateModel
    {
        public PercolateOptions Options { get; set; }

        public TPercolateModel Model { get; set; }

        public QueryModel BuildQueryModel(IQueryCollection queryCollection);

        public void ValidateQueryModel(QueryModel queryModel, PercolateModel percolateModel);

        public void ApplyModel(QueryModel queryModel);//, IQueryable? IEnumerable?);
    }
}
