using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Percolate.Attributes;
using Percolate.Filtering;
using Percolate.Models;
using Percolate.Paging;
using Percolate.Sorting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Percolate
{
    public class PercolateService<TPercolateModal> : IPercolateService<TPercolateModal> where TPercolateModal : PercolateModel
    {
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

            //At runtime, we don't know the Type of the object that Percolate is processing.
            //The individual processing methods are written as generic methods because they are more maintainable
            //We must discover the Type and invoke those generic methods.

            Type genericType = GetResultValueAsIQueryable(result)
                .GetType()
                .GetGenericArguments()
                .First();

            var queryable = GetType()
                .GetMethod(nameof(GetResultValueAsGenericIQueryable), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(genericType)
                .Invoke(this, new[] { result });

            var attribute = GetEnablePercolateAttribute(context.ActionDescriptor);

            var type = GetType()
                .GetMethod(nameof(GetPercolateType), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(genericType)
                .Invoke(this, null) as IPercolateType;

            //this is used as the binding flags on the GetMethod calls that find the appropriate apply method
            var bindingFlags = BindingFlags.Public | BindingFlags.Static;

            if (FilterHelper.IsFilteringEnabled(attribute, type, Options))
            {
                var filterQuery = FilterHelper.ParseFilterQuery(context);
                //FilterHelper.ValidateFilterQuery(filterQuery, type);

                queryable = typeof(FilterHelper)
                    .GetMethod(nameof(FilterHelper.ApplyFilterQuery), bindingFlags)
                    .MakeGenericMethod(genericType)
                    .Invoke(this, new[] { queryable, filterQuery });
            }

            if (SortHelper.IsSortingEnabled(attribute, type, Options))
            {
                var sortQuery = SortHelper.ParseSortQuery(context);
                SortHelper.ValidateSortQuery(sortQuery, type);

                queryable = typeof(SortHelper)
                    .GetMethod(nameof(SortHelper.ApplySortQuery), bindingFlags)
                    .MakeGenericMethod(genericType)
                    .Invoke(this, new[] { queryable, sortQuery });
            }

            if (PageHelper.IsPagingEnabled(attribute, type, Options))
            {
                var pageQuery = PageHelper.GetPageQuery(context, attribute, type, Options);
                PageHelper.ValidatePageQuery(pageQuery, attribute, type, Options);

                queryable = typeof(PageHelper)
                    .GetMethod(nameof(PageHelper.ApplyPageQuery), bindingFlags)
                    .MakeGenericMethod(genericType)
                    .Invoke(this, new[] { queryable, pageQuery });
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

        private IPercolateType GetPercolateType<T>()
        {
            return Model.Types
                .FirstOrDefault(type => type.Type == typeof(T));
        }

        private Type GetGenericTypeFromIQueryable(IQueryable queryable)
        {
            var genericType = queryable.GetType().GetGenericArguments().First();

            return genericType;
        }

        private IQueryable GetResultValueAsIQueryable(OkObjectResult result)
        {
            if (!(result.Value is IEnumerable))
            {
                return null;
            }

            IQueryable value;

            if (result.Value is IQueryable)
            {
                value = result.Value as IQueryable;
            }
            else
            {
                value = (result.Value as IEnumerable).AsQueryable();
            }

            return value;

        }

        private IQueryable<T> GetResultValueAsGenericIQueryable<T>(OkObjectResult result)
        {
            if (!(result.Value is IEnumerable))
            {
                return null;
            }

            IQueryable<T> value;

            if (result.Value is IQueryable)
            {
                value = result.Value as IQueryable<T>;
            }
            else
            {
                value = (result.Value as IEnumerable<T>).AsQueryable<T>();
            }

            return value;
        }
    }
}
