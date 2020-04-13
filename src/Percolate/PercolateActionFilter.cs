using Microsoft.AspNetCore.Mvc.Filters;

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
            context.Result = service.ApplyQuery(context);
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
