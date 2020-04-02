using Microsoft.AspNetCore.Mvc.Filters;

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
            context.Result = service.Process(context);
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
