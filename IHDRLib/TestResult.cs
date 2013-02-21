using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILNumerics;

namespace IHDRLib
{
    public class TestResult
    {
        public int Id { get; set; }
        public double Label { get; set; }
        public Vector ClusterMeanX { get; set; }
        public Vector ClusterMeanY { get; set; }
        public Sample Input { get; set; }
        public double LabelByClosestYMean { get; set; }
    }
}
