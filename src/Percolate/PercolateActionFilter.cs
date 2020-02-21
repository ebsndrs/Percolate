using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Percolate.Attributes;

namespace Percolate
{
    public class PercolateActionFilter : IActionFilter
    {
        private readonly IPercolateService service;

        public PercolateActionFilter(IPercolateService service)
        {
            this.service = service;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (ShouldApplyQuery(service.Options.EnablePercolateGlobally, context.ActionDescriptor))
            {
                //percolate!
            }
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
