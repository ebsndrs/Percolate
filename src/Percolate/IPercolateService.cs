using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Percolate
{
    public interface IPercolateService<TPercolateModel> where TPercolateModel : PercolateModel
    {
        public PercolateOptions Options { get; set; }

        public TPercolateModel Model { get; set; }

        public IActionResult Process(ActionExecutedContext context);
    }
}
