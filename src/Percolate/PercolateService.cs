using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Percolate.Attributes;
using Percolate.Filtering;
using Percolate.Models;
using Percolate.Paging;
using Percolate.Sorting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Percolate
{
    public class PercolateService<TPercolateModal> : IPercolateService<TPercolateModal> where TPercolateModal : PercolateModel
    {
        private EnablePercolateAttribute attribute;
        private IQueryable<dynamic> queryable;
        private IPercolateType type;

        public PercolateService(TPercolateModal model, IOptions<PercolateOptions> options)
        {
            Model = model;
            Options = options.Value;
        }

        public TPercolateModal Model { get; set; }

        public PercolateOptions Options { get; set; }

        public IActionResult Process(ActionExecutedContext context)
        {
            if (!(context.Result is OkObjectResult result))
            {
                return context.Result;
            }

            if (!ShouldPercolateProcess(context.ActionDescriptor))
            {
                return context.Result;
            }

            if (!IsCollectionResult(result))
            {
                return context.Result;
            }

            queryable = GetResultValueAsQueryable(result);
            attribute = GetEnablePercolateAttribute(context.ActionDescriptor);
            type = GetPercolateTypeFromQueryable(queryable);

            if (FilterHelper.IsFilteringEnabled(attribute, type, Options))
            {
                var filterQuery = FilterHelper.GetFilterQuery(context);
                FilterHelper.ValidateFilterQuery(filterQuery, type);
                queryable = FilterHelper.ApplyFilterQuery(queryable, filterQuery);
            }

            if (SortHelper.IsSortingEnabled(attribute, type, Options))
            {
                var sortQuery = SortHelper.GetSortQuery(context);
                SortHelper.ValidateSortQuery(sortQuery, type);
                queryable = SortHelper.ApplySortQuery(queryable, sortQuery);
            }

            if (PageHelper.IsPagingEnabled(attribute, type, Options))
            {
                var pageQuery = PageHelper.GetPageQuery(context, attribute, type, Options);
                PageHelper.ValidatePageQuery(pageQuery, attribute, type, Options);
                queryable = PageHelper.ApplyPageQuery(queryable, pageQuery);
            }

            return new OkObjectResult(queryable);
        }

        private bool ShouldPercolateProcess(ActionDescriptor actionDescriptor)
        {
            var isEnablePercolateOnController = AttributeHelper.GetAttributeFromController<EnablePercolateAttribute>(actionDescriptor) != null;
            var isDisablePercolateOnController = AttributeHelper.GetAttributeFromController<DisablePercolateAttribute>(actionDescriptor) != null;
            var isEnablePercolateOnAction = AttributeHelper.GetAttributeFromAction<EnablePercolateAttribute>(actionDescriptor) != null;
            var isDisablePercolateOnAction = AttributeHelper.GetAttributeFromAction<DisablePercolateAttribute>(actionDescriptor) != null;

            return (isEnablePercolateOnAction ||
                (isEnablePercolateOnController && !isDisablePercolateOnAction) ||
                (Options.IsPercolateEnabledGlobally && !isDisablePercolateOnController && !isDisablePercolateOnAction));
        }

        private EnablePercolateAttribute GetEnablePercolateAttribute(ActionDescriptor actionDescriptor)
        {
            var actionAttribute = AttributeHelper.GetAttributeFromAction<EnablePercolateAttribute>(actionDescriptor);

            if (actionAttribute != null)
            {
                return actionAttribute;
            }

            var controllerAttribute = AttributeHelper.GetAttributeFromController<EnablePercolateAttribute>(actionDescriptor);

            if (controllerAttribute != null)
            {
                return controllerAttribute;
            }

            return null;
        }

        private bool IsCollectionResult(OkObjectResult result)
        {
            return result.Value is IEnumerable || result.Value is IQueryable;
        }

        private IPercolateType GetPercolateTypeFromQueryable(IQueryable queryable)
        {
            var genericType = queryable
                .GetType()
                .GetGenericArguments()
                .Single();

            return Model.Types
                .FirstOrDefault(type => type.Type == genericType);
        }

        private IQueryable<dynamic> GetResultValueAsQueryable(OkObjectResult result)
        {
            if (result.Value is IQueryable<dynamic>)
            {
                return result.Value as IQueryable<dynamic>;
            }
            else
            {
                return (result.Value as IEnumerable<dynamic>)
                    .AsQueryable<dynamic>();
            }
        }
    }
}
