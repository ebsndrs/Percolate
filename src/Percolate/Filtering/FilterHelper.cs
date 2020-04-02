using Microsoft.AspNetCore.Mvc.Filters;
using Percolate.Attributes;
using Percolate.Models;
using System.Linq;

namespace Percolate.Filtering
{
    public static class FilterHelper
    {
        public static bool IsFilteringEnabled(EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            if (attribute != default && attribute.FilterSetting != PercolateAttributeSetting.Unset)
            {
                return attribute.FilterSetting == PercolateAttributeSetting.Enabled;
            }
            else if (type.IsFilteringEnabled.HasValue)
            {
                return type.IsFilteringEnabled.Value;
            }
            else
            {
                return options.IsFilteringEnabled;
            }
        }

        public static FilterQuery GetFilterQuery(ActionExecutedContext context)
        {
            return FilterParser.ParseFilterQuery(context.HttpContext.Request.Query);
        }

        public static void ValidateFilterQuery(FilterQuery query, IPercolateType type)
        {
            if (query.Nodes.Any())
            {
                var rules = FilterValidator.GetFilterQueryValidationRules(type);
                FilterValidator.ValidateFilterQuery(query, type, rules);
            }
        }

        public static IQueryable<dynamic> ApplyFilterQuery(IQueryable<dynamic> queryable, FilterQuery query)
        {
            return queryable;
        }
    }
}
