using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Percolate.Attributes;
using Percolate.Models;
using Percolate.Parsers;

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
                var temp = new PercolateModel()
                {
                    PageModel = PagingParser.ParsePagingParameters(context.HttpContext.Request.Query),
                    SortModel = SortParser.ParseSortParameter(context.HttpContext.Request.Query),
                    FilterModel = FilterParser.ParseFilterQuery(context.HttpContext.Request.Query)
                };

                context.Result = new OkObjectResult(temp);
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
