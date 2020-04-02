//using Percolate.Filtering;
//using System.Collections.Generic;
//using Xunit;

//namespace PercolateTests.UnitTests.ValidatorTests
//{
//    public class FilterValidatorTests
//    {
//        [Fact]
//        public void ValidateFilterParameters_WhenCalledWithValidParameters_DoesNotThrowException()
//        {
//            var query = new FilterQuery
//            {
//                Nodes = new List<FilterQueryNode>()
//                {
//                    new FilterQueryNode
//                    {
//                        RawNode = "foo=bar",
//                        Properties = new string[] { "foo" },
//                        Values = new string[] { "bar" },
//                        RawOperator = "=",
//                        ParsedOperator = FilterQueryOperator.Equals
//                    },
//                    new FilterQueryNode
//                    {
//                        RawNode = "spam>1",
//                        Properties = new string[] { "spam" },
//                        Values = new string[] { "1" },
//                        RawOperator = ">",
//                        ParsedOperator = FilterQueryOperator.GreaterThan
//                    }
//                }
//            };

//            var rules = new FilterValidationRules
//            {
//                //IsFilteringEnabled = true
//            };

//            FilterValidator.ValidateFilterQuery(query, rules);
//        }
//    }
//}
