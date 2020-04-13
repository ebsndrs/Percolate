using Microsoft.Extensions.Primitives;
using Percolate.Attributes;
using Percolate.Models;
using System.Collections.Generic;
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

        public static int GetDefaultSkip()
        {
            return 0;
        }

        public static int GetDefaultTake(EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
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

        public static PageQuery ParsePageQuery(Dictionary<string, StringValues> queryCollection)
        {
            return PageParser.ParsePageQuery(queryCollection);
        }

        public static void ValidatePageQuery(PageQuery query, EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            var rules = PageValidator.GetPageQueryValidationRules(attribute, type, options);
            PageValidator.ValidatePageQuery(query, rules);
        }

        public static IQueryable<T> ApplyPageQuery<T>(IQueryable<T> queryable, PageQuery query, EnablePercolateAttribute attribute, IPercolateType type, PercolateOptions options)
        {
            int skip;
            int take;

            if (query.PageSize.HasValue)
            {
                take = query.PageSize.Value;
            }
            else
            {
                take = GetDefaultTake(attribute, type, options);
            }

            if (query.Page.HasValue)
            {
                skip = query.Page.Value == 1 ? GetDefaultSkip() : query.Page.Value * (take - 1);
            }
            else
            {
                skip = GetDefaultSkip();
            }

            return queryable
                .Skip(skip)
                .Take(take);
        }
    }
}
