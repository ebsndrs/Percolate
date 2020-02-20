using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Percolate
{
    public class PercolateActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
