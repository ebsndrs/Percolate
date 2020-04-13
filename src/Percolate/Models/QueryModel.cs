using Microsoft.AspNetCore.Mvc;
using Percolate.Filtering;
using Percolate.ModelBinders;
using Percolate.Paging;
using Percolate.Sorting;

namespace Percolate.Models
{
    //[ModelBinder(BinderType = typeof(QueryModelBinder))]
    public class QueryModel
    {
        public QueryModel()
        {
            PageQueryModel = new PageQuery();
            SortQueryModel = new SortQuery();
            FilterQueryModel = new FilterQuery();
        }

        public PageQuery PageQueryModel { get; set; }

        public SortQuery SortQueryModel { get; set; }

        public FilterQuery FilterQueryModel { get; set; }
    }
}
