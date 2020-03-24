using Microsoft.AspNetCore.Http;
using Percolate.Models;
using Percolate.Parsers;

namespace Percolate
{
    public class PercolateService<TPercolateModal> : IPercolateService<TPercolateModal> where TPercolateModal : PercolateModel
    {
        public PercolateService(TPercolateModal model)
        {
            Options = new PercolateOptions();
            Model = model;
        }

        public PercolateOptions Options { get; set; }

        public TPercolateModal Model { get; set; }

        public void ApplyModel(QueryModel queryModel)
        {
            throw new System.NotImplementedException();
        }

        public QueryModel BuildQueryModel(IQueryCollection queryCollection)
        {
            return new QueryModel
            {
                PageModel = PageParser.ParsePagingParameters(queryCollection),
                SortModel = SortParser.ParseSortParameter(queryCollection),
                FilterModel = FilterParser.ParseFilterQuery(queryCollection)
            };
        }

        public void ValidateQueryModel(QueryModel queryModel, PercolateModel percolateModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
