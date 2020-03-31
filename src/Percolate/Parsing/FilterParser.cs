using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using Percolate.Models.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Percolate.Parsers
{
    public static class FilterParser
    {
        //we use these regexes to split the query by non-escaped commas and pipes
        //this enables filters with commas and pipes in them while still allowing
        //multiple filters and logical OR operations
        private const string escapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";
        private const string escapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";
        private static readonly Dictionary<string, FilterQueryOperator> operators = new Dictionary<string, FilterQueryOperator>()
        {
            { "=", FilterQueryOperator.Equals },
            { "!=", FilterQueryOperator.DoesNotEqual },
            { ">=", FilterQueryOperator.GreaterThanOrEqual },
            { "<=", FilterQueryOperator.LessThanOrEqual },
            { ">", FilterQueryOperator.GreaterThan },
            { "<", FilterQueryOperator.LessThan }
        };

        public static FilterQueryModel ParseFilterQuery(IQueryCollection queryCollection)
        {
            var filterQueryModel = new FilterQueryModel();

            if (queryCollection.ContainsKey("filter"))
            {
                var filterQueries = Regex.Split(queryCollection["filter"], escapedCommaPattern);

                filterQueryModel.Nodes = filterQueries.Select(queryString => ParseFilterNode(queryString));
            }

            return filterQueryModel;
        }

        private static FilterQueryNode ParseFilterNode(string value)
        {
            //find the operator; it'll be the first operator that appears in the string
            string filterQueryOperator = IdentifyOperator(value);

            FilterQueryOperator parsedOperator = operators[filterQueryOperator];

            //split the value by the identified operator
            var splitFilterArray = value
                .Split(filterQueryOperator, 2)
                .Select(item => item.Trim())
                .ToArray();

            if (string.IsNullOrWhiteSpace(splitFilterArray[0]))
                throw new ParameterParsingException($"The filter query parameter \"{value}\" has an empty properties set.");

            if (string.IsNullOrWhiteSpace(splitFilterArray[1]))
                throw new ParameterParsingException($"The filter query parameter \"{value}\" has an empty values set.");

            //split any pipe delimited strings clean up the escape characters, and cull any duplicates
            //we considered throwing an exception in the case of duplicate properties or values, but it's easy
            //to account for and doesn't change the meaning of the filter query string
            var filterProperties = Regex
                .Split(splitFilterArray[0], escapedPipePattern)
                .Select(filterProperty => filterProperty.Replace("\\", ""))
                .Distinct();

            var filterValues = Regex
                .Split(splitFilterArray[1], escapedPipePattern)
                .Select(filterProperty => filterProperty.Replace("\\", ""))
                .Distinct();

            return new FilterQueryNode
            {
                RawNode = value,
                Properties = filterProperties,
                Values = filterValues,
                Operator = filterQueryOperator,
                ParsedOperator = parsedOperator,
                IsOperatorNegated = IsFilterOperatorNegated(parsedOperator)
            };
        }

        private static bool IsFilterOperatorNegated(FilterQueryOperator filterOperator) =>
            filterOperator == FilterQueryOperator.DoesNotEqual;
        
        private static string IdentifyOperator(string node)
        {
            var nodeAsArray = node.ToCharArray();

            for (int i = 0; i < nodeAsArray.Length; i++)
            {
                char currentChar = nodeAsArray[i];

                if (currentChar == '=')
                    return $"{currentChar}";

                if (i != nodeAsArray.Length - 1)
                {
                    if (currentChar == '!' || currentChar == '>' || currentChar == '<')
                    {
                        var nextChar = nodeAsArray[i + 1];
                        if (nextChar == '=')
                            return $"{currentChar}{nextChar}";
                        else if (currentChar != '!')
                            return $"{currentChar}";
                    }
                }
            }

            throw new ParameterParsingException($"No valid operator was found in the filter query parameter \"{node}\".");
        }
    }
}
