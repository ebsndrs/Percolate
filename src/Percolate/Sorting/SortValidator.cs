using Percolate.Exceptions;
using Percolate.Models;
using System;
using System.Linq;

namespace Percolate.Sorting
{
    public static class SortValidator
    {
        public static SortValidationRules GetSortQueryValidationRules(IPercolateEntity type)
        {
            return new SortValidationRules
            {
                DisallowedProperties = type.Properties
                    .Where(property => property.IsSortingAllowed.HasValue && property.IsFilteringAllowed == false)
            };
        }

        public static void ValidateSortQuery(SortQuery query, IPercolateEntity type, SortValidationRules rules)
        {
            if (query == null)
            {
                return;
            }

            foreach (var node in query.Nodes)
            {
                var splitName = node.Name
                    .Split('.')
                    .Select(segment => segment.Trim());

                //ensure that the parsed property name in the node exists in the type configuration
                var isPropertyOnType = type.Properties
                    .Any(property => string.Equals(property.Name, splitName.ElementAt(0), StringComparison.InvariantCultureIgnoreCase));

                if (!isPropertyOnType)
                {
                    throw new ParameterValidationException();
                }

                //ensure that the parsed property on the node is not on the list of disallowed properties to sort on
                var isPropertyDisallowed = rules.DisallowedProperties
                    .Any(property => string.Equals(property.Name, splitName.ElementAt(0), StringComparison.InvariantCultureIgnoreCase));

                if (isPropertyDisallowed)
                {
                    throw new ParameterValidationException();
                }

                //ensure that the type of the property is IComparable (otherwise we can't sort it)
                //var propertyType = type.Properties
                //    .Single(property => string.Equals(property.Name, splitName.ElementAt(0), StringComparison.InvariantCultureIgnoreCase))
                //    .Type;

                //if (!typeof(IComparable).IsAssignableFrom(propertyType))
                //{
                //    throw new ParameterValidationException();
                //}
            }
        }
    }
}
