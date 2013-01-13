using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class IHDR
    {
        private int attributesCount;
        private Samples samples;
        private Tree tree;

        #region Properties

        public Samples Samples
        {
            get { return samples; }
        }

        #endregion

        public IHDR()
        {
            SetSettings();

            this.samples = new Samples();
            this.tree = new Tree();
        }

        private void SetSettings()
        {
            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 784;
            Params.outputDataDimension = 784;
            Params.q = 4;
            Params.bs = 30;
            Params.outputIsDefined = false;
            Params.deltaX = 1200.0;
            Params.deltaY = 1200.0;
            Params.blx = 10;
            Params.bly = 10;
            Params.p = 20;
            Params.confidenceValue = 0.05;
            Params.digitizationNoise = 1;
            Params.ContainsSingularCovarianceMatrixes = true;
            Params.savePath = @"D:\IHDRTree\";
            Params.SaveCovMatrices = false;
            Params.SaveCovMatricesMDF = true;
            Params.SaveMeans = false;
            Params.SaveMeansMDF = true;
        }

        public void AddSample(double[] sample, double label)
        {
            samples.AddSample(new Sample(sample, label, samples.Count + 1));
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

        public void BuildTree()
        {
            if (Params.useClassMeanLikeY)
            {
                samples.CountOutputsFromClassLabels();
            }

            if (samples != null && samples.Count > 100)
            {
                int i = 0;
                foreach (Sample sample in samples)
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

    }
}

