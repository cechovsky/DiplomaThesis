using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHDRLib
{
    public static class Settings
    {
        public static void SetSettings_MNISTMyOutput()
        {
            Params.q = 2;
            Params.bs = 50;
            Params.outputIsDefined = false;
            Params.deltaX = 300.0;
            Params.deltaY = 300.0;
            Params.deltaMultiplyReduction = 0.5;
            Params.deltaXReduction = 50.0;
            Params.deltaXReduction = 50.0;
            Params.deltaXMin = 60.0;
            Params.deltaYMin = 60.0;
            Params.blx = 3;
            Params.bly = 3;
            Params.p = 0.0;
            Params.l = 10;
            Params.confidenceValue = 0.1;
            Params.digitizationNoise = 1;


            //amnesic parameters
            Params.t1 = 3000000;
            Params.t2 = 1000;
            Params.c = 5.0;
            Params.m = 1000.0;

            // swap type
            Params.SwapType = 3;

            #region Not Important Settings

            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 784;
            Params.outputDataDimension = 1600;

            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            Params.StoreSamples = true;
            Params.StoreItems = true;
            Params.WidthOfTesting = 3;
            Params.Epochs = 2;

            Params.inputBmpWidth = 28;
            Params.inputBmpHeight = 28;
            Params.outputBmpWidth = 40;
            Params.outputBmpHeight = 40;

            #endregion

        }

        public static void SetSettings_Faces()
        {
            Params.q = 20;
            Params.bs = 3;
            Params.outputIsDefined = false;
            Params.deltaX = 1200.0;
            Params.deltaY = 1200.0;
            Params.deltaMultiplyReduction = 0.5;
            Params.deltaXReduction = 50.0;
            Params.deltaXReduction = 50.0;
            Params.deltaXMin = 60.0;
            Params.deltaYMin = 60.0;
            Params.blx = 40;
            Params.bly = 40;
            Params.p = 0.0;
            Params.l = 10;
            Params.confidenceValue = 0.05;
            Params.digitizationNoise = 1;


            //amnesic parameters		
            Params.t1 = 3000000;
            Params.t2 = 1000;
            Params.c = 5.0;
            Params.m = 1000.0;

            // swap type		
            Params.SwapType = 3;

            #region Not Important Settings

            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 361;
            Params.outputDataDimension = 361;

            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTreeFaces\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            Params.StoreSamples = true;
            Params.StoreItems = true;
            Params.WidthOfTesting = 3;
            Params.Epochs = 3;

            Params.inputBmpWidth = 19;
            Params.inputBmpHeight = 19;
            Params.outputBmpWidth = 19;
            Params.outputBmpHeight = 19;

            #endregion
        }

        public static void SetSettings_Faces2()
        {
            Params.q = 3;
            Params.bs = 10;
            Params.outputIsDefined = false;
            Params.deltaX = 1200.0;
            Params.deltaY = 1200.0;
            Params.deltaMultiplyReduction = 0.5;
            Params.deltaXReduction = 50.0;
            Params.deltaXReduction = 50.0;
            Params.deltaXMin = 60.0;
            Params.deltaYMin = 60.0;
            Params.blx = 3;
            Params.bly = 3;
            Params.p = 0.0;
            Params.l = 10;
            Params.confidenceValue = 0.005;
            Params.digitizationNoise = 1;


            //amnesic parameters		
            Params.t1 = 3000000;
            Params.t2 = 1000;
            Params.c = 5.0;
            Params.m = 1000.0;

            // swap type		
            Params.SwapType = 3;

            #region Not Important Settings

            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 3888;
            Params.outputDataDimension = 3888;

            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTreeFaces\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            Params.StoreSamples = true;
            Params.StoreItems = true;
            Params.WidthOfTesting = 3;
            Params.Epochs = 1;

            Params.inputBmpWidth = 54;
            Params.inputBmpHeight = 72;
            Params.outputBmpWidth = 54;
            Params.outputBmpHeight = 72;

            #endregion
        }

        public static void SetSettings_MNIST()
        {
            Params.q = 20;
            Params.bs = 3;
            Params.outputIsDefined = false;
            Params.deltaX = 1200.0;
            Params.deltaY = 1200.0;
            Params.deltaMultiplyReduction = 0.5;
            Params.deltaXReduction = 50.0;
            Params.deltaXReduction = 50.0;
            Params.deltaXMin = 60.0;
            Params.deltaYMin = 60.0;
            Params.blx = 20;
            Params.bly = 20;
            Params.p = 0.0;
            Params.l = 10;
            Params.confidenceValue = 0.005;
            Params.digitizationNoise = 1;


            //amnesic parameters		
            Params.t1 = 3000000;
            Params.t2 = 1000;
            Params.c = 5.0;
            Params.m = 1000.0;

            // swap type		
            Params.SwapType = 3;

            #region Not Important Settings

            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 784;
            Params.outputDataDimension = 784;

            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            Params.StoreSamples = true;
            Params.StoreItems = false;
            Params.WidthOfTesting = 3;
            Params.Epochs = 3;

            Params.inputBmpWidth = 28;
            Params.inputBmpHeight = 28;
            Params.outputBmpWidth = 28;
            Params.outputBmpHeight = 28;

            #endregion
        }

        public static void SetSettings_Arcene()
        {
            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 10000;
            Params.outputDataDimension = 10000;
            Params.q = 5;
            Params.bs = 1;
            Params.outputIsDefined = false;
            Params.deltaX = 1200.0;
            Params.deltaY = 1200.0;
            Params.deltaMultiplyReduction = 0.5;
            Params.deltaXReduction = 50.0;
            Params.deltaXReduction = 50.0;
            Params.deltaXMin = 60.0;
            Params.deltaYMin = 60.0;
            Params.blx = 5;
            Params.bly = 5;
            Params.p = 0.0;
            Params.l = 10;
            Params.confidenceValue = 0.005;
            Params.digitizationNoise = 1;
            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            Params.StoreSamples = true;
            Params.StoreItems = false;

            //amnesic parameters		
            Params.t1 = 1000;
            Params.t2 = 20;
            Params.c = 5.0;
            Params.m = 100.0;
            Params.WidthOfTesting = 3;
            Params.Epochs = 50;

            // swap type		
            Params.SwapType = 3;
        }

        public static void SetSettings_Synthetic()
        {
            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 10000;
            Params.outputDataDimension = 10000;
            Params.q = 5;
            Params.bs = 1;
            Params.outputIsDefined = false;
            Params.deltaX = 1200.0;
            Params.deltaY = 1200.0;
            Params.deltaMultiplyReduction = 0.5;
            Params.deltaXReduction = 50.0;
            Params.deltaXReduction = 50.0;
            Params.deltaXMin = 60.0;
            Params.deltaYMin = 60.0;
            Params.blx = 5;
            Params.bly = 5;
            Params.p = 0.0;
            Params.l = 10;
            Params.confidenceValue = 0.005;
            Params.digitizationNoise = 1;
            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            Params.StoreSamples = true;
            Params.StoreItems = false;

            //amnesic parameters		
            Params.t1 = 1000;
            Params.t2 = 20;
            Params.c = 5.0;
            Params.m = 100.0;
            Params.WidthOfTesting = 3;
            Params.Epochs = 3;

            // swap type		
            Params.SwapType = 3;
        }

        // for dataset gisette
        public static void SetSettings_Gisette()
        {
            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 5000;
            Params.outputDataDimension = 5000;
            Params.q = 10;
            Params.bs = 1;
            Params.outputIsDefined = false;
            Params.deltaX = 1200.0;
            Params.deltaY = 1200.0;
            Params.deltaMultiplyReduction = 0.5;
            Params.deltaXReduction = 50.0;
            Params.deltaXReduction = 50.0;
            Params.deltaXMin = 60.0;
            Params.deltaYMin = 60.0;
            Params.blx = 20;
            Params.bly = 20;
            Params.p = 0.0;
            Params.l = 10;
            Params.confidenceValue = 0.005;
            Params.digitizationNoise = 1;
            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            Params.StoreSamples = true;
            Params.StoreItems = false;

            //amnesic parameters		
            Params.t1 = 1000;
            Params.t2 = 20;
            Params.c = 5.0;
            Params.m = 100.0;
            Params.WidthOfTesting = 3;
            Params.Epochs = 3;

            // swap type		
            Params.SwapType = 3;
        }

    }
}
