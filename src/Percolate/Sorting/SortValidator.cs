using Percolate.Exceptions;
using Percolate.Models;
using System;
using System.Linq;

namespace Percolate.Sorting
{
    public static class SortValidator
    {
        public static SortValidationRules GetSortQueryValidationRules(IPercolateType type)
        {
            var disallowedProperties = type.Properties
                .Where(property => property.IsSortingAllowed.HasValue && property.IsFilteringAllowed == false);

            return new SortValidationRules
            {
                DisallowedProperties = disallowedProperties
            };
        }

        public static void ValidateSortQuery(SortQuery query, IPercolateType type, SortValidationRules rules)
        {
            foreach (var node in query.Nodes)
            {
                //ensure that the parsed property name in the node exists in the type configuration
                var isPropertyOnType = type.Properties
                    .Select(property => property.Name)
                    .Contains(node.Name, StringComparer.InvariantCultureIgnoreCase);

                if (!isPropertyOnType)
                {
                    throw new ParameterValidationException();
                }

                //ensure that the parsed property on the node is allowed to sort on by property level configuration
                var isPropertyDisallowed = rules.DisallowedProperties
                    .Select(property => property.Name)
                    .Contains(node.Name, StringComparer.InvariantCultureIgnoreCase);

                if (isPropertyDisallowed)
                {
                    throw new ParameterValidationException();
                }

                //ensure that the type of the property is comparable (otherwise dynamic LINQ can't sort it)
                var typeToCheck = type.Properties
                    .Single(property => string.Equals(property.Name, node.Name, StringComparison.InvariantCultureIgnoreCase))
                    .Type;

                if (!typeof(IComparable).IsAssignableFrom(typeToCheck))
                {
                    throw new ParameterValidationException();
                }
            }
        }
    }
}
