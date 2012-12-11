using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public static class Params
    {
        // number of maximum children for each internal note
        public static int q { get; set; }

        // number of maximum micro-clusters in node
        public static int qmc { get; set; }

        // number of samples needed per scalar parameter ( boundary of NSPP in spawning )
        public static double bs { get; set; }

        //
        public static double l  { get; set; }

        // when is true, we count Y like mean of mean of samples of same label
        public static bool useClassMeanLikeY { get; set; }

        // is true when output of samples is defined
        public static bool outputIsDefined { get; set; }

        // dimension of input data
        public static int inputDataDimension { get; set; }

        // dimension of output data
        public static int outputDataDimension { get; set; }

        // bl - bound on the number of micro-clusters
        public static double bl { get; set; }

        // deltaX - resolution in input space X
        public static double deltaX { get; set; }
                
    }
}
