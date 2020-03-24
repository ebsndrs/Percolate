using Microsoft.AspNetCore.Http;
using Percolate.Models;
using Percolate.Parsers;

namespace Percolate
{
    public class PercolateService : IPercolateService
    {
        public PercolateService()
        {
            Options = new PercolateOptions();
        }

        public PercolateOptions Options { get; set; }

        public PercolateModel BuildModel(IQueryCollection queryCollection)
        {
            return new PercolateModel
            {
                PageModel = PageParser.ParsePagingParameters(queryCollection),
                SortModel = SortParser.ParseSortParameter(queryCollection),
                FilterModel = FilterParser.ParseFilterQuery(queryCollection)
            };
        }
    }
}
