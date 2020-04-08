using Microsoft.AspNetCore.Mvc.Filters;
using Percolate.Attributes;
using Percolate.Models;
using System.Linq;

namespace Percolate.Paging
{
    public static class PageHelper
    {
        public static bool IsPagingEnabled(EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            if (attribute != default && attribute.PageSetting != PercolateAttributeSetting.Unset)
            {
                return attribute.PageSetting == PercolateAttributeSetting.Enabled;
            }
            else if (type.IsPagingEnabled.HasValue)
            {
                return type.IsPagingEnabled.Value;
            }
            else
            {
                return options.IsPagingEnabled;
            }
        }

        public static int GetDefaultPage()
        {
            return 1;
        }

        public static int GetDefaultPageSize(EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            if (attribute != default && attribute.DefaultPageSize != 0)
            {
                return attribute.DefaultPageSize;
            }
            else if (type.DefaultPageSize.HasValue)
            {
                return type.DefaultPageSize.Value;
            }
            else
            {
                return options.DefaultPageSize;
            }
        }

        public static int GetMaximumPageSize(EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            if (attribute != default && attribute.MaximumPageSize != 0)
            {
                return attribute.MaximumPageSize;
            }
            else if (type.MaximumPageSize.HasValue)
            {
                return type.MaximumPageSize.Value;
            }
            else
            {
                return options.MaximumPageSize;
            }
        }

        public static PageQuery GetPageQuery(ActionExecutedContext context, EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            var defaultPage = GetDefaultPage();
            var defaultPageSize = GetDefaultPageSize(attribute, type, options);

            return PageParser.ParsePageQuery(context.HttpContext.Request.Query, defaultPage, defaultPageSize);
        }

        public static void ValidatePageQuery(PageQuery query, EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            var rules = PageValidator.GetPageQueryValidationRules(attribute, type, options);
            PageValidator.ValidatePageQuery(query, rules);
        }

        public static IQueryable<T> ApplyPageQuery<T>(IQueryable<T> queryable, PageQuery query)
        {
            var skip = query.Page == 1 ? 0 : query.Page * (query.PageSize - 1);
            var take = query.PageSize;

            return queryable
                .Skip(skip)
                .Take(take);
        }
    }
}
