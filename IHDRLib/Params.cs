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

        // for marking node like plastic, if node is spawn more like l times, it is non-plastic
        public static double l  { get; set; }

        // when is true, we count Y like mean of mean of samples of same label
        public static bool useClassMeanLikeY { get; set; }

        // when is true, we count Y like mean of mean of samples of same label
        public static bool useClassMeanOfAdded { get; set; }

        // is true when output of samples is defined
        public static bool outputIsDefined { get; set; }

        // dimension of input data
        public static int inputDataDimension { get; set; }

        // dimension of output data
        public static int outputDataDimension { get; set; }

        // bl - bound on the number of micro-clusters by x
        public static double blx { get; set; }

        // bl - bound on the number of micro-clusters by y
        public static double bly { get; set; }

        // deltaX - resolution in input space X
        public static double deltaX { get; set; }

        // deltaX - resolution in input space Y
        public static double deltaY { get; set; }

        // reduction of deltaX in next node
        public static double deltaXReduction { get; set; }

        // reduction of deltaX in next node
        public static double deltaYReduction { get; set; }

        // minimal deltaX
        public static double deltaXMin { get; set; }

        // minimal deltaY
        public static double deltaYMin { get; set; }

        public static double searchWidth { get; set; }

        public static bool useExtendedSearch { get; set; }

        // amnesic average params 

        // t1
        public static double t1 { get; set; }

        // t2
        public static double t2 { get; set; }

        // c
        public static double c { get; set; }

        // m
        public static double m { get; set; }

        // p - portion of y cluster that will be updated ( in percents )
        public static double p { get; set; }

        // it is value alpha. it is needed by computing bounds of NSPP
        public static double confidenceValue { get; set; }

        // it is value alpha. it is needed by computing bounds of NSPP
        public static double digitizationNoise { get; set; }

        // path for sample saving
        public static string savePath { get; set; }

        // width of testing
        public static int WidthOfTesting { get; set; }

        // if count nearest cluster by normal euclidean distance or by most discriminating features distance
        public static bool NearestClusterNormal { get; set; }

        // if during saving to hierarchy save also Means to text files
        public static bool SaveMeans { get; set; }

        // if during saving to hierarchy save also Means to text files
        public static bool SaveMeansMDF { get; set; }

        // if during saving to hierarchy save also CovMatrices to text files
        public static bool SaveCovMatrices { get; set; }

        // if during saving to hierarchy save also CovMatrices to text files
        public static bool SaveCovMatricesMDF { get; set; }

        // if during saving to hierarchy save also vectors to text files
        public static bool SaveVectors { get; set; }

        // if during saving to hierarchy save also vectors to text files
        public static bool ContainsSingularCovarianceMatrixes { get; set; }

        // 1 normal swap
        // 2 modified swap ( clusters by k-means )
        // 3 modified swap ( all samples by k-means )
        public static int SwapType { get; set; }
    }
}
