using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Models;
using Percolate.Parsers;

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
            context.Result = new OkObjectResult(service.Model);

            //if (ShouldApplyQuery(service.Options.EnablePercolateGlobally, context.ActionDescriptor))
            //{
            //    try
            //    {
            //        var queryModel = service.BuildQueryModel(context.HttpContext.Request.Query);

            //    }
            //    catch (PercolateException)
            //    {
            //        if (!service.Options.FailSilently)
            //        {
            //            throw;
            //        }
            //    }

            //}
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        private bool ShouldApplyQuery(bool enablePercolateGlobally, ActionDescriptor actionDescriptor)
        {
            var isEnablePercolateOnController = AttributeHelper.GetAttributeFromController<EnablePercolateAttribute>(actionDescriptor) != null;
            var isDisablePercolateOnController = AttributeHelper.GetAttributeFromController<DisablePercolateAttribute>(actionDescriptor) != null;
            var isEnablePercolateOnAction = AttributeHelper.GetAttributeFromAction<EnablePercolateAttribute>(actionDescriptor) != null;
            var isDisablePercolateOnAction = AttributeHelper.GetAttributeFromAction<DisablePercolateAttribute>(actionDescriptor) != null;

            return isEnablePercolateOnAction ||
                (isEnablePercolateOnController && !isDisablePercolateOnAction) ||
                (enablePercolateGlobally && !isDisablePercolateOnController && !isDisablePercolateOnAction);
        }
    }
}
