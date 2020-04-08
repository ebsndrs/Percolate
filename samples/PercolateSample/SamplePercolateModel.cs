using Percolate;
using Percolate.Builders;
using PercolateSample.Models;

namespace PercolateSample
{
    public class SamplePercolateModel : PercolateModel
    {
        public override void Configure(PercolateModelBuilder modelBuilder)
        {
            modelBuilder.Type<Person>();
        }
    }
}
