using Microsoft.AspNetCore.Http;
using Percolate.Exceptions;
using Percolate.Models.Filtering;
using System.Linq;
using System.Text.RegularExpressions;

namespace Percolate.Parsers
{
    public static class FilterParser
    {
        //we use these regexes to split the query by non-escaped commas and pipes
        private const string escapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";
        private const string escapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";


        public static FilterModel ParseFilterQuery(IQueryCollection queryCollection)
        {
            var filterModel = new FilterModel();

            if (queryCollection.ContainsKey("filter"))
            {
                var filterQueries = Regex.Split(queryCollection["filter"], escapedCommaPattern);

                try
                {
                    filterModel.Nodes = filterQueries.Select(queryString => ParseFilterNode(queryString));
                }
                catch (ParameterParsingException e)
                {
                    throw;
                }
            }

            return filterModel;
        }

        private static FilterNode ParseFilterNode(string value)
        {
            string @operator = FindOperator(value);

            FilterOperator? parsedOperator = @operator switch
            {
                "=" => FilterOperator.Equals,
                "!=" => FilterOperator.DoesNotEqual,
                ">=" => FilterOperator.GreaterThanOrEqual,
                "<=" => FilterOperator.LessThanOrEqual,
                ">" => FilterOperator.GreaterThan,
                "<" => FilterOperator.LessThan,
                _ => null
            };

            if (parsedOperator == null)
                throw new ParameterParsingException();

            //now split the value by the operator
            var filterSplit = value
                .Split(@operator, 2, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToArray();

            if (filterSplit.Length != 2)
                throw new ParameterParsingException();

            var filterProperties = Regex
                .Split(filterSplit[0], escapedPipePattern)
                .Select(filterProperty => filterProperty.Replace("\\", ""));
            var filterValues = Regex
                .Split(filterSplit[1], escapedPipePattern)
                .Select(filterProperty => filterProperty.Replace("\\", ""));

            return new FilterNode
            {
                RawNode = value,
                Properties = filterProperties,
                Values = filterValues,
                Operator = @operator,
                ParsedOperator = parsedOperator.Value,
                IsOperatorNegated = IsFilterOperatorNegated(parsedOperator.Value)
            };
        }

        private static bool IsFilterOperatorNegated(FilterOperator filterOperator) =>
            filterOperator == FilterOperator.DoesNotEqual;

        private static string FindOperator(string value)
        {
            var array = value.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                char current = array[i];

                if (current == '=')
                    return $"{current}";

                if (i != array.Length - 1)
                {
                    if (current == '!' || current == '>' || current == '<')
                    {
                        var next = array[i + 1];
                        if (next == '=')
                            return $"{current}{next}";
                        else if (current != '!')
                            return $"{current}";
                    }
                }
            }

            return null;
        }
    }
}
