using Percolate.Exceptions;
using Percolate.Models;
using System;
using System.ComponentModel;
using System.Linq;

namespace Percolate.Filtering
{
    public static class FilterValidator
    {
        public static FilterValidationRules GetFilterQueryValidationRules(IPercolateType type)
        {
            var disallowedProperties = type.Properties
                .Where(property => property.IsFilteringAllowed.HasValue && property.IsFilteringAllowed == false);

            return new FilterValidationRules
            {
                DisallowedProperties = disallowedProperties
            };
        }

        public static void ValidateFilterQuery(FilterQuery query, IPercolateType type, FilterValidationRules rules)
        {
            foreach (var node in query.Nodes)
            {
                //ensure that all properties that were parsed in the node exist in the type configuration
                var typePropertyNames = type.Properties
                    .Select(property => property.Name);

                var propertiesThatAreNotOnType = node.Properties
                    .Where(np => !typePropertyNames.Contains(np.Name, StringComparer.InvariantCultureIgnoreCase));

                if (propertiesThatAreNotOnType.Any())
                {
                    throw new ParameterValidationException();
                }

                //ensure that all properties that exist on the type but are allowed to filter on by property level configuration
                var disallowedPropertyNames = rules.DisallowedProperties
                    .Select(property => property.Name);

                var propertiesThatAreNotAllowedToFilterOn = node.Properties
                    .Where(np => disallowedPropertyNames.Contains(np.Name, StringComparer.InvariantCultureIgnoreCase));

                if (propertiesThatAreNotAllowedToFilterOn.Any())
                {
                    throw new ParameterValidationException();
                }

                //ensure that all included properties share the same underlying type
                var propertyTypes = type.Properties
                    .Where(p => node.Properties.Select(node => node.Name).Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
                    .Select(p => p.Type)
                    .Distinct();

                if (propertyTypes.Count() > 1)
                {
                    throw new ParameterValidationException();
                }

                //ensure that all piped values can be parsed into the shared type
                var validType = propertyTypes.First();
                var typeConverter = TypeDescriptor.GetConverter(validType);

                foreach (var value in node.Values)
                {
                    try
                    {
                        typeConverter.ConvertFromString(value);
                    }
                    catch (ArgumentException)
                    {
                        throw new ParameterValidationException();
                    }
                }

            }
        }
    }
}
