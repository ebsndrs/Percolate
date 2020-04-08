using Microsoft.AspNetCore.Mvc.Filters;
using Percolate.Attributes;
using Percolate.Models;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Percolate.Sorting
{
    public static class SortHelper
    {
        public static bool IsSortingEnabled(EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            if (attribute != default && attribute.SortSetting != PercolateAttributeSetting.Unset)
            {
                return attribute.SortSetting == PercolateAttributeSetting.Enabled;
            }
            else if (type.IsSortingEnabled.HasValue)
            {
                return type.IsSortingEnabled.Value;
            }
            else
            {
                return options.IsSortingEnabled;
            }
        }

        public static SortQuery GetSortQuery(ActionExecutedContext context)
        {
            return SortParser.ParseSortQuery(context.HttpContext.Request.Query);
        }

        public static void ValidateSortQuery(SortQuery query, IPercolateType type)
        {
            if (query.Nodes.Any())
            {
                var rules = SortValidator.GetSortQueryValidationRules(type);
                SortValidator.ValidateSortQuery(query, type, rules);
            }
        }

        public static IQueryable<T> ApplySortQuery<T>(IQueryable<T> queryable, SortQuery query)
        {
            if (query.Nodes.Any())
            {
                //convert nodes into a format dynamic LINQ understands: 'property1 ASC, property2 DESC, etc'
                var sortStrings = query.Nodes
                    .Select(node => $"{node.Name} {(node.Direction == SortQueryDirection.Ascending ? "ASC" : "DESC")}");

                var sortString = string.Join(',', sortStrings);

                queryable = queryable.OrderBy(sortString);
            }

            return queryable;
        }
    }
}
