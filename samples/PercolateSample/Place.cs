using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PercolateSample
{
    public class Place
    {
        public string Name { get; set; }

        public bool CanVisit { get; set; }

        public PlaceDetails Details { get; set; }
    }
}
