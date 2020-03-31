using Percolate.Attributes;
using Percolate.Exceptions;
using Percolate.Validation.Paging;
using System;
using System.Linq;

namespace Percolate.Validation
{
    public static class ValidationHelper
    {
        public static ValidationRules BuildValidationRules(Type type, PercolateOptions options, PercolateModel model, EnablePercolateAttribute attribute)
        {
            var typeModel = model.Types
                .FirstOrDefault(t => t.Type == type);

            if (typeModel == null)
            {
                throw new ParameterValidationException();
            }

            return new ValidationRules
            {
                PageValidationRules = PageValidator.BuildPageValidationRules(typeModel, options, attribute)
            };
        }
    }
}
