using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Models;
using System;
using System.Collections;
using System.Linq;

namespace Percolate
{
    public class PercolateActionFilter<TPercolateModel> : IActionFilter where TPercolateModel : PercolateModel
    {
        private readonly IPercolateService<TPercolateModel> service;

        public PercolateActionFilter(IPercolateService<TPercolateModel> service)
        {
            this.service = service;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult result)
            {
                if (ShouldApplyQuery(service.Options.IsPercolateEnabledGlobally, context.ActionDescriptor, result))
                {
                    try
                    {
                        var queryObject = GetQueryObject(result);
                        var queryModel = service.BuildQuery(context.HttpContext.Request.Query);

                        service.ValidateQuery(queryObject, queryModel, GetEnablePercolateAttribute(context.ActionDescriptor));

                        var newResultValue = service.ApplyQuery(queryObject, queryModel);

                        if (service.Options.DoResponsesIncludeMetadata)
                        {
                            context.Result = new OkObjectResult(new PercolateResponse
                            {
                                Value = newResultValue,
                                Metadata = queryModel
                            });
                        }
                        else
                        {
                            context.Result = new OkObjectResult(newResultValue);
                        }
                    }
                    catch (PercolateException)
                    {
                        if (!service.Options.DoExceptionsFailSilently)
                            throw;
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        private bool ShouldApplyQuery(bool enablePercolateGlobally, ActionDescriptor actionDescriptor, OkObjectResult result)
        {
            var isEnablePercolateOnController = AttributeHelper.GetAttributeFromController<EnablePercolateAttribute>(actionDescriptor) != null;
            var isDisablePercolateOnController = AttributeHelper.GetAttributeFromController<DisablePercolateAttribute>(actionDescriptor) != null;
            var isEnablePercolateOnAction = AttributeHelper.GetAttributeFromAction<EnablePercolateAttribute>(actionDescriptor) != null;
            var isDisablePercolateOnAction = AttributeHelper.GetAttributeFromAction<DisablePercolateAttribute>(actionDescriptor) != null;

            return (isEnablePercolateOnAction ||
                (isEnablePercolateOnController && !isDisablePercolateOnAction) ||
                (enablePercolateGlobally && !isDisablePercolateOnController && !isDisablePercolateOnAction)) &&
                (result.Value is IQueryable || result.Value is IEnumerable);
        }

        private EnablePercolateAttribute GetEnablePercolateAttribute(ActionDescriptor actionDescriptor)
        {
            var actionAttribute = AttributeHelper.GetAttributeFromAction<EnablePercolateAttribute>(actionDescriptor);

            if (actionAttribute != null)
                return actionAttribute;

            var controllerAttribute = AttributeHelper.GetAttributeFromController<EnablePercolateAttribute>(actionDescriptor);

            if (controllerAttribute != null)
                return controllerAttribute;

            return null;
        }

        private IQueryable GetQueryObject(OkObjectResult result)
        {
            IQueryable query;

            if (result.Value is IEnumerable)
                query = (result.Value as IEnumerable).AsQueryable();
            else if (result.Value is IQueryable)
                query = result.Value as IQueryable;
            else
                throw new TypeNotSupportedException($"Percolate can only manipulate collection types that implement IEnumerable.");

            return query;
        }
    }
}
