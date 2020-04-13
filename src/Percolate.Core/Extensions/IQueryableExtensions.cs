using Percolate.Core.Helpers;
using Percolate.Filtering;
using Percolate.Models;
using Percolate.Paging;
using Percolate.Sorting;
using System.Linq;

namespace Percolate.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyQuery<TModel, T>(this IQueryable<T> queryable, QueryModel query, PercolateService<TModel> service) where TModel : PercolateModel
        {
            var type = PercolateModelHelper.GetPercolateType<T>(service.Model);

            if (FilterHelper.IsFilteringEnabled(null, type, service.Options))
            {
                //validate
                queryable = FilterHelper.ApplyFilterQuery(queryable, query.FilterQueryModel);
            }

            if (SortHelper.IsSortingEnabled(null, type, service.Options))
            {
                SortHelper.ValidateSortQuery(query.SortQueryModel, type);
                queryable = SortHelper.ApplySortQuery(queryable, query.SortQueryModel);
            }

            if (PageHelper.IsPagingEnabled(null, type, service.Options))
            {
                PageHelper.ValidatePageQuery(query.PageQueryModel, null, type, service.Options);
                queryable = PageHelper.ApplyPageQuery(queryable, query.PageQueryModel);
            }

            return queryable;
        }
    }
}
