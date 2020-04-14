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
            try
            {
                context.Result = service.ProcessResult(context);
            }
            catch
            {
                if (!service.Options.DoExceptionsFailSilently)
                {
                    throw;
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
