using Microsoft.AspNetCore.Mvc.ModelBinding;
using Percolate.Filtering;
using Percolate.Models;
using Percolate.Paging;
using Percolate.Sorting;
using System.Linq;
using System.Threading.Tasks;

namespace Percolate.ModelBinders
{
    public class QueryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var query = bindingContext.HttpContext.Request.Query
                .ToDictionary(q => q.Key, q => q.Value);

            try
            {
                var filterQuery = FilterHelper.ParseFilterQuery(query);
                var sortQuery = SortHelper.ParseSortQuery(query);
                var pageQuery = PageHelper.ParsePageQuery(query);

                var queryModel = new QueryModel
                {
                    FilterQueryModel = filterQuery,
                    SortQueryModel = sortQuery,
                    PageQueryModel = pageQuery
                };

                bindingContext.Result = ModelBindingResult.Success(queryModel);

                return Task.CompletedTask;
            }
            catch
            {
                throw;
            }
        }
    }
}
