//using Percolate.Sorting;
//using System.Collections.Generic;
//using Xunit;

//namespace PercolateTests.UnitTests.ValidatorTests
//{
//    public class SortValidatorTests
//    {
//        [Fact]
//        public void ValidateSortParameters_WhenCalledWithValidParameters_DoesNotThrowException()
//        {
//            var query = new SortQuery
//            {
//                Nodes = new List<SortQueryNode>()
//                {
//                    new SortQueryNode
//                    {
//                        Name = "foo",
//                        Direction = SortQueryDirection.Ascending
//                    },
//                    new SortQueryNode
//                    {
//                        Name = "bar",
//                        Direction = SortQueryDirection.Descending
//                    }
//                }
//            };

//            var rules = new SortValidationRules
//            {
//                IsSortingEnabled = true
//            };

//            SortValidator.ValidateSortQuery(query, rules);
//        }
//    }
//}
