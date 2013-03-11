﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IHDRLib
{
    [Serializable]
    public class IHDR : ISerializable
    {
        private Samples samples;
        private Samples testingSamples;
        private Tree tree;
        private Dictionary<double, Vector> labelOutputVectors;

        #region Properties

        public Samples Samples
        {
            get { return samples; }
        }

        #endregion

        public IHDR()
        {
            SetSettings2();
            this.samples = new Samples();
            this.testingSamples = new Samples();
            this.tree = new Tree();
            this.labelOutputVectors = new Dictionary<double, Vector>();
        }

        private void SetSettings()
        {
            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 784;
            Params.outputDataDimension = 784;
            Params.q = 6;
            Params.bs = 3.5;
            Params.outputIsDefined = false;
            Params.deltaX = 800.0;
            Params.deltaY = 800.0;
            Params.deltaXReduction = 150.0;
            Params.deltaXReduction = 150.0;
            Params.deltaXMin = 200;
            Params.deltaYMin = 200;
            Params.blx = 12;
            Params.bly = 12;
            Params.p = 0.2;
            Params.l = 3;
            Params.confidenceValue = 0.05;
            Params.digitizationNoise = 1;
            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
            
            //amnesic parameters
            Params.t1 = 10.0;
            Params.t2 = 50.0;
            Params.c = 5.0;
            Params.m = 1000.0;
            Params.WidthOfTesting = 3;

            // swap type
            Params.SwapType = 3;
        }

        private void SetSettings2()
        {
            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 3;
            Params.outputDataDimension = 2;
            Params.q = 2;
            Params.bs = 10;
            Params.outputIsDefined = false;
            Params.deltaX = 60.0;
            Params.deltaY = 60.0;
            Params.deltaXReduction = 10.0;
            Params.deltaXReduction = 10.0;
            Params.deltaXMin = 12;
            Params.deltaYMin = 12;
            Params.blx = 3;
            Params.bly = 3;
            Params.p = 0.0;
            Params.l = 3;
            Params.confidenceValue = 0.05;
            Params.digitizationNoise = 1;
            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;

            //amnesic parameters
            Params.t1 = 10.0;
            Params.t2 = 50.0;
            Params.c = 3.0;
            Params.m = 100.0;
            Params.WidthOfTesting = 3;

            // swap type
            Params.SwapType = 3;
        }

        public void AddSample(double[] sample, double label)
        {
            samples.AddSample(new Sample(sample, label, samples.Items.Count + 1));
        }

        public void AddSample(double[] sample, double[] sampleY, double label)
        {
            samples.AddSample(new Sample(sample, sampleY, label, samples.Items.Count + 1));
        }

        public void AddSamples(List<double[]> samples, double[] labels)
        {
            if (samples.Count != labels.Length) throw new InvalidOperationException("Number of samples is not equal to number of labels");

            int iterator = 0;
            foreach (double[] item in samples)
            {
                this.samples.AddSample(new Sample(item, labels[iterator], samples.Count + 1));
                iterator++;
            }
        }

        public void AddTestingSample(double[] sample, double label)
        {
            if (this.testingSamples == null)
            {
                this.testingSamples = new Samples();
            }
            this.testingSamples.AddSample(new Sample(sample, label, samples.Items.Count + 1));
        }

        public void AddTestingSamples(List<double[]> samples, double[] labels)
        {
            if (samples.Count != labels.Length) throw new InvalidOperationException("Number of samples is not equal to number of labels");

            int iterator = 0;
            foreach (double[] item in samples)
            {
                this.testingSamples.AddSample(new Sample(item, labels[iterator], samples.Count + 1));
                iterator++;
            }
        }

        public void BuildTree()
        {
            if (Params.useClassMeanLikeY)
            {
                samples.CountOutputsFromClassLabels();
            }

            //samples.SaveItemsY(@"D:\SamplesY\");

            if (samples != null && samples.Items.Count > 100)
            {
                int i = 0;
                foreach (Sample sample in samples.Items)
                {
                    Console.WriteLine("Update tree Sample " + i.ToString());
                    this.tree.UpdateTree(sample);
                    i++;
                }
            }
        }

        public void SaveTreeToFileHierarchy()
        {
            if (this.tree != null)
            {
                this.tree.SaveToFileHierarchy();
            }
        }

        private void CountYMeanOfLabels()
        {
            var labels = this.samples.Items.GroupBy(i => i.Label).Select(i => i.First().Label);
            this.labelOutputVectors = new Dictionary<double, Vector>();
            foreach (var item in labels)
            {
                labelOutputVectors.Add(item, samples.GetMeanOfDataWithLabel(item));
            }
        }

        public void CountYOfSamplesLabelsMeans()
        {
            this.CountYMeanOfLabels();
            foreach (var item in this.samples.Items)
            {
                item.SetY(labelOutputVectors[item.Label]);
            }
        }

        public void EvaluateClustersLabels()
        {
            if (tree != null)
            {
                this.tree.EvaluateClustersLabels();
            }
        }

        public void EvaluateAllClustersLabels()
        {
            if (tree != null)
            {
                this.tree.EvaluateAllClustersLabels();
            }
        }

        public void SaveLeafClustersToPicture()
        {
            List<ClusterPair> clusterPairs = this.tree.GetAllLeafClusterPairs();

            Bitmap bitmap = new Bitmap(400, 400);

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
            }

            for (int i = 0; i < 400; i++)
            {
                bitmap.SetPixel(i, 200, Color.FromArgb(0, 0, 0));
                bitmap.SetPixel(200, i, Color.FromArgb(0, 0, 0));
            }

            foreach (var item in clusterPairs)
            {
                switch ((int)item.X.Label)
                {
                    case 1:
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 200, 400 - ((int)item.Y.Mean.Values[1] + 200), Color.FromArgb(255, 0, 0));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 200, 400 - ((int)item.Y.Mean.Values[1] + 201), Color.FromArgb(255, 0, 0));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 201, 400 - ((int)item.Y.Mean.Values[1] + 200), Color.FromArgb(255, 0, 0));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 201, 400 - ((int)item.Y.Mean.Values[1] + 201), Color.FromArgb(255, 0, 0));
                        this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(255, 0, 0));
                        break;
                    case 2:
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 200, 400 - ((int)item.Y.Mean.Values[1] + 200), Color.FromArgb(0, 255, 0));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 201, 400 - ((int)item.Y.Mean.Values[1] + 201), Color.FromArgb(0, 255, 0));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 200, 400 - ((int)item.Y.Mean.Values[1] + 201), Color.FromArgb(0, 255, 0));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 201, 400 - ((int)item.Y.Mean.Values[1] + 200), Color.FromArgb(0, 255, 0));
                        //Console.WriteLine("X : " + ((int)item.ClusterMeanY.Values[0]).ToString());
                        //Console.WriteLine("Y : " + ((int)item.ClusterMeanY.Values[1]).ToString());
                        this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(0, 255, 0));
                        break;
                    case 3:
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 200, 400 - ((int)item.Y.Mean.Values[1] + 200), Color.FromArgb(0, 0, 255));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 201, 400 - ((int)item.Y.Mean.Values[1] + 201), Color.FromArgb(0, 0, 255));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 200, 400 - ((int)item.Y.Mean.Values[1] + 201), Color.FromArgb(0, 0, 255));
                        bitmap.SetPixel((int)item.Y.Mean.Values[0] + 201, 400 - ((int)item.Y.Mean.Values[1] + 200), Color.FromArgb(0, 0, 255));
                        this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(0, 0, 255));
                        break;
                    default:
                        break;
                }
            }

            bitmap.Save(@"D:\LeafClusters.bmp");
        }

        public void ExecuteTesting()
        {
            List<TestResult> testResults = new List<TestResult>();
            int i = 0;
            foreach (var item in this.testingSamples.Items)
            {
                i++;
                TestResult testResult = this.tree.GetLabelOfCategory(item);
                testResult.Id = i;
                testResult.Input = item;
                testResults.Add(testResult);
            }

            //this.SaveTestResultsNonEqual(@"D:\IHDR\Results\NonEqual", testResults);
            //this.SaveTestResultsEqual(@"D:\IHDR\Results\Equal", testResults);

            int same = 0;
            int different = 0;
            foreach (var item in testResults)
            {
                if (item.Label == item.Input.Label)
                {
                    same++;
                }
                else
                {
                    different++;
                }
            }
            Console.WriteLine("The same: " + same.ToString());
            Console.WriteLine("Different: " + different.ToString());

            this.Save2DResultToBmp(testResults);
        }

        private void Save2DResultToBmp(List<TestResult> testResults)
        {
            Bitmap bitmap = new Bitmap(400, 400);

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
            }

            for (int i = 0; i < 400; i++)
            {
                bitmap.SetPixel(i, 200, Color.FromArgb(0, 0, 0));
                bitmap.SetPixel(200, i, Color.FromArgb(0, 0, 0));
            }

            foreach (var item in testResults)
            {
                switch ((int)item.Label)
                {
                    case 1:
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 200, 400 - ((int)item.ClusterMeanY.Values[1] + 200), Color.FromArgb(255, 0, 0));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 201, 400 - ((int)item.ClusterMeanY.Values[1] + 201), Color.FromArgb(255, 0, 0));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 200, 400 - ((int)item.ClusterMeanY.Values[1] + 201), Color.FromArgb(255, 0, 0));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 201, 400 - ((int)item.ClusterMeanY.Values[1] + 200), Color.FromArgb(255, 0, 0));
                        this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(255, 0, 0));
                        break;
                    case 2:
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 200, 400 - ((int)item.ClusterMeanY.Values[1] + 200), Color.FromArgb(0, 255, 0));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 201, 400 - ((int)item.ClusterMeanY.Values[1] + 201), Color.FromArgb(0, 255, 0));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 200, 400 - ((int)item.ClusterMeanY.Values[1] + 201), Color.FromArgb(0, 255, 0));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 201, 400 - ((int)item.ClusterMeanY.Values[1] + 200), Color.FromArgb(0, 255, 0));
                        //Console.WriteLine("X : " + ((int)item.ClusterMeanY.Values[0]).ToString());
                        //Console.WriteLine("Y : " + ((int)item.ClusterMeanY.Values[1]).ToString());
                        this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(0, 255, 0));
                        break;
                    case 3:
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 200, 400 - ((int)item.ClusterMeanY.Values[1] + 200), Color.FromArgb(0, 0, 255));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 201, 400 - ((int)item.ClusterMeanY.Values[1] + 201), Color.FromArgb(0, 0, 255));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 200, 400 - ((int)item.ClusterMeanY.Values[1] + 201), Color.FromArgb(0, 0, 255));
                        bitmap.SetPixel((int)item.ClusterMeanY.Values[0] + 201, 400 - ((int)item.ClusterMeanY.Values[1] + 200), Color.FromArgb(0, 0, 255));
                        this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(0, 0, 255));
                        break;
                    default:
                        break;
                }
            }

            bitmap.Save(@"D:\syntheticdatgraph.bmp");
        }

        public void SaveSamplesToBitmap(Bitmap bmp, List<Sample> samples, Color color)
        {
            foreach (var item in samples)
            {
                this.DrawCircleToBmp((int)item.Y.Values[0] + 400, 800 - ((int)item.Y.Values[1] + 400), bmp, color, 5);
                //bmp.SetPixel((int)item.Y.Values[0] + 200, 400 - ((int)item.Y.Values[1] + 200), color);
            }
        }

        public void ExecuteTestingByY()
        {
            List<TestResult> testResults = new List<TestResult>();
            int i = 0;
            foreach (var item in this.testingSamples.Items)
            {
                i++;
                TestResult testResult = this.tree.GetLabelOfCategory(item);
                testResult.Id = i;
                testResult.Input = item;
                testResults.Add(testResult);
                testResult.LabelByClosestYMean = this.GetLabelOfClosestY(testResult.ClusterMeanY);
            }

            //this.SaveTestResultsNonEqual(@"D:\IHDR\Results\NonEqual", testResults);
            //this.SaveTestResultsEqual(@"D:\IHDR\Results\Equal", testResults);

            int same = 0;
            int different = 0;
            foreach (var item in testResults)
            {
                if (item.LabelByClosestYMean == item.Input.Label)
                {
                    same++;
                }
                else
                {
                    different++;
                }
            }
            Console.WriteLine("The same: " + same.ToString());
            Console.WriteLine("Different: " + different.ToString());
        }

        public void ExecuteWideTesting()
        {
            List<TestResult> testResults = new List<TestResult>();

            int i = 0;
            foreach (var item in this.testingSamples.Items)
            {
                i++;
                TestResult testResult = this.tree.GetTestResultByWidthSearch(item);
                testResult.Id = i;
                testResult.Input = item;
                testResult.LabelByClosestYMean = this.GetLabelOfClosestY(testResult.ClusterMeanY);
                testResults.Add(testResult);
                
            }

            this.SaveTestResultsNonEqual(@"D:\IHDR\Results\NonEqual\", testResults);
            this.SaveTestResultsEqual(@"D:\IHDR\Results\Equal\", testResults);

            int same = 0;
            int different = 0;
            foreach (var item in testResults)
            {
                if (item.LabelByClosestYMean == item.Input.Label)
                {
                    same++;
                }
                else
                {
                    different++;
                }
            }
            Console.WriteLine("The same: " + same.ToString());
            Console.WriteLine("Different: " + different.ToString());

        }

        private double GetLabelOfClosestY(Vector YMean)
        {
            double minDistance = double.MaxValue;
            double resultLabel = 0;
            foreach (var item in this.labelOutputVectors)
            {
                double distance = item.Value.GetDistance(YMean);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    resultLabel = item.Key;
                }
            }
            return resultLabel;
        }

        public void SaveTestResults(string path, List<TestResult> results)
        {
            int i = 1;
            foreach (var item in results)
            {
                //string newPath = path + "\\" + i.ToString() + "\\";

                string fileName1 = i.ToString() + "_input";
                item.Input.X.SaveToBitmap(path, fileName1);

                string fileNameX = i.ToString() + "_Xresult";
                item.ClusterMeanX.SaveToBitmap(path, fileNameX);

                string fileNameY = i.ToString() + "_Yresult";
                item.ClusterMeanY.SaveToBitmap(path, fileNameY);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Input label: " + item.Input.Label);
                sb.AppendLine("Result label: " + item.Label);

                File.WriteAllText(path  + "ResultInfo.txt", sb.ToString());
                i++;
            }
        }

        public void SaveTestResultsNonEqual(string path, List<TestResult> results)
        {
            int i = 1;
            foreach (var item in results)
            {
                if (item.LabelByClosestYMean != item.Input.Label)
                {
                    //string newPath = path + "\\" + i.ToString() + "\\";
                    string fileName1 = i.ToString() + "_input";
                    item.Input.X.SaveToBitmap(path, fileName1);

                    string fileNameX = i.ToString() + "_Xresult";
                    item.ClusterMeanX.SaveToBitmap(path, fileNameX);

                    string fileNameY = i.ToString() + "_Yresult";
                    item.ClusterMeanY.SaveToBitmap(path, fileNameY);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Input label: " + item.Input.Label);
                    sb.AppendLine("Result label: " + item.Label);

                    File.WriteAllText(path + "ResultInfo" + i.ToString() + ".txt", sb.ToString());
                }
                i++;
            }
        }

        public void SaveTestResultsEqual(string path, List<TestResult> results)
        {
            int i = 1;
            foreach (var item in results)
            {
                if (item.LabelByClosestYMean == item.Input.Label)
                {

                    //string newPath = path + "\\" + i.ToString() + "\\";
                    string fileName1 = i.ToString() + "_input";
                    item.Input.X.SaveToBitmap(path, fileName1);

                    string fileNameX = i.ToString() + "_Xresult";
                    item.ClusterMeanX.SaveToBitmap(path, fileNameX);

                    string fileNameY = i.ToString() + "_Yresult";
                    item.ClusterMeanY.SaveToBitmap(path, fileNameY);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Input label: " + item.Input.Label);
                    sb.AppendLine("Result label: " + item.Label);

                    File.WriteAllText(path + "ResultInfo" + i.ToString() + ".txt", sb.ToString());
                }
                i++;
            }
        }

        #region Serialization

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("samples", samples, typeof(Samples));
            info.AddValue("tree", tree, typeof(Tree));
        }

        public IHDR(SerializationInfo info, StreamingContext context)
        {
            samples = (Samples)info.GetValue("samples", typeof(Samples));
            tree = (Tree)info.GetValue("tree", typeof(Tree));
        }

        #endregion


        public void EvaluateDepth()
        {
            this.tree.EvaluateDepth();
        }

        public void SaveLayersToBmp(string p)
        {
            List<ClusterPair> clusterPairs = this.tree.GetAllClusterPairs();

            int maxDepth = clusterPairs.Select(i => i.Depth).Max();

            for (int f = 1; f <= maxDepth; f++)
            {
                var clusterPairsInDepthI = clusterPairs.Where(clp => clp.Depth == f).ToList();

                Bitmap bitmap = new Bitmap(800, 800);

                for (int i = 0; i < 800; i++)
                {
                    for (int j = 0; j < 800; j++)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    }
                }

                for (int i = 0; i < 800; i++)
                {
                    bitmap.SetPixel(i, 400, Color.FromArgb(0, 0, 0));
                    bitmap.SetPixel(400, i, Color.FromArgb(0, 0, 0));
                }

                foreach (var item in clusterPairsInDepthI)
                {
                    switch ((int)item.X.Label)
                    {
                        case 1:
                            this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(255, 150, 150));
                            this.DrawCircleToBmp((int)item.Y.Mean.Values[0] + 400, 800 - ((int)item.Y.Mean.Values[1] + 400), bitmap, Color.FromArgb(255, 0, 0), 10);
                            break;
                        case 2:
                            this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(150, 255, 150));
                            this.DrawCircleToBmp((int)item.Y.Mean.Values[0] + 400, 800 - ((int)item.Y.Mean.Values[1] + 400), bitmap, Color.FromArgb(0, 255, 0), 10);
                            break;
                        case 3:
                            this.SaveSamplesToBitmap(bitmap, item.Samples, Color.FromArgb(150, 150, 255));
                            this.DrawCircleToBmp((int)item.Y.Mean.Values[0] + 400, 800 - ((int)item.Y.Mean.Values[1] + 400), bitmap, Color.FromArgb(0, 0, 255), 10);
                            break;
                        default:
                            break;
                    }
                }

                bitmap.Save(p + @"\Level" + f.ToString() + ".bmp");
            }
        }

        public void DrawCircleToBmp(int x, int y, Bitmap bmp, Color color, int size)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Brush b = new SolidBrush(color))
                {
                    g.FillEllipse(b, x, y, size, size);
                }
            }
        }
    
    }

    
}

