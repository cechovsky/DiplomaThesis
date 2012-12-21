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
            this.samples = new Samples();
            this.tree = new Tree();

            SetSettings();
        }

        private void SetSettings()
        {
            Params.useClassMeanLikeY = false;
            Params.inputDataDimension = 784;
            Params.outputDataDimension = 784;
            Params.q = 4;
            Params.bs = 15;
            Params.outputIsDefined = false;
            Params.deltaX = 1000.0;
            Params.deltaY = 1000.0;
            Params.blx = 20;
            Params.bly = 20;
            Params.p = 20;
        }

        public void AddSample(double[] sample, double label)
        {
            samples.AddSample(new Sample(sample, label));
        }

        public void AddSamples(List<double[]> samples, double[] labels)
        {
            if (samples.Count != labels.Length) throw new InvalidOperationException("Number of samples is not equal to number of labels");

            int iterator = 0;
            foreach (double[] item in samples)
            {
                this.samples.AddSample(new Sample(item, labels[iterator]));
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
                foreach (Sample sample in samples)
                {
                    this.tree.UpdateTree(sample);
                }
            }
        }
    }
}

