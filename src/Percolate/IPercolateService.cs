using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Percolate
{
    public interface IPercolateService
    {
        PercolateOptions Options { get; set; }

        PercolateModel Model { get; set; }

        IActionResult ProcessResult(ActionExecutedContext context);
    }
}
