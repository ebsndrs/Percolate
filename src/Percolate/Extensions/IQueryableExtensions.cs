using Percolate.Filtering;
using Percolate.Models;
using Percolate.Paging;
using Percolate.Sorting;
using System.Linq;

namespace Percolate.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyQuery<T>(this IQueryable<T> queryable, QueryModel query, IPercolateService service)
        {
            var entityType = service.Model.GetEntity<T>();

            if (FilterHelper.IsFilteringEnabled(null, entityType, service.Options))
            {
                FilterHelper.ValidateFilterQuery(query.FilterQueryModel, entityType);
                queryable = FilterHelper.ApplyFilterQuery(queryable, query.FilterQueryModel);
            }

            if (SortHelper.IsSortingEnabled(null, entityType, service.Options))
            {
                SortHelper.ValidateSortQuery(query.SortQueryModel, entityType);
                queryable = SortHelper.ApplySortQuery(queryable, query.SortQueryModel);
            }

            if (PageHelper.IsPagingEnabled(null, entityType, service.Options))
            {
                PageHelper.ValidatePageQuery(query.PageQueryModel, null, entityType, service.Options);
                queryable = PageHelper.ApplyPageQuery(queryable, query.PageQueryModel, null, entityType, service.Options);
            }

            return queryable;
        }
    }
}
