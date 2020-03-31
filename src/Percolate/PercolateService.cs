using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Models;
using Percolate.Models.Paging;
using Percolate.Parsers;
using Percolate.Validation;
using Percolate.Validation.Paging;
using System.Linq;

namespace Percolate
{
    public class PercolateService<TPercolateModal> : IPercolateService<TPercolateModal> where TPercolateModal : PercolateModel
    {
        public PercolateService(TPercolateModal model, IOptions<PercolateOptions> options)
        {
            Model = model;
            Options = options.Value;
        }

        public TPercolateModal Model { get; set; }

        public PercolateOptions Options { get; set; }

        public QueryModel BuildQuery(IQueryCollection queryCollection)
        {
            return new QueryModel
            {
                PageQueryModel = PageParser.ParsePagingParameters(queryCollection),
                SortQueryModel = SortParser.ParseSortParameter(queryCollection),
                FilterQueryModel = FilterParser.ParseFilterQuery(queryCollection)
            };
        }

        public void ValidateQuery(IQueryable queryObject, QueryModel queryModel, EnablePercolateAttribute attribute)
        {
            var genericTypeArgument = queryObject.GetType().GetGenericArguments().First();

            if (!Model.Types.Any(t => t.Type == genericTypeArgument))
            {
                throw new TypeNotSupportedException($"The type {genericTypeArgument.Name} is not a part of the {Model.GetType().Name} configuration. Add it to enable Percolate for the type.");
            }

            var validationRules = ValidationHelper.BuildValidationRules(genericTypeArgument, Options, Model, attribute);

            PageValidator.ValidatePageParameters(queryModel.PageQueryModel, validationRules.PageValidationRules);

            //validate sort

            //validate filter
        }

        public IQueryable ApplyQuery(IQueryable queryObject, QueryModel queryModel)
        {
            var genericQueryObject = queryObject as IQueryable<dynamic>;

            return ApplyPaging(genericQueryObject, queryModel.PageQueryModel);
        }

        private IQueryable<dynamic> ApplyPaging(IQueryable<dynamic> query, PageQueryModel queryModel)
        {
            var skip = queryModel.Page.Value == 1 ? 0 : queryModel.Page.Value * queryModel.PageSize.Value;
            var take = queryModel.PageSize.Value;

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            query = query.Take(take);

            return query;
        }
    }
}
