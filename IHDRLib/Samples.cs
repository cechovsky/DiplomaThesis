using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Samples : List<Sample>
    {

        public Samples() : base()
        {
        }

        public void AddSample(Sample sample)
        {
            this.Add(sample);
        }

        public double GetAverageDisanceBetweenSamples()
        {
            int samplesCount = this.Count;

            double sum = 0;
            int count = 0; 
            for (int i = 0; i < samplesCount; i++)
            {
                for (int j = i + 1; j < samplesCount; j++)
                {
                    sum += this.ElementAt(i).GetXDistanceFromSample(this.ElementAt(j));
                    count++;
                }
            }

            return sum / count;
        }

        public double GetAverageDisanceBetweenSamplesOfOneLabel(double label)
        {
            int samplesCount = this.Count;

            double sum = 0;
            int count = 0;
            for (int i = 0; i < samplesCount; i++)
            {
                for (int j = i + 1; j < samplesCount; j++)
                {
                    if (this.ElementAt(i).Label == label && this.ElementAt(j).Label == label)
                    {
                        sum += this.ElementAt(i).GetXDistanceFromSample(this.ElementAt(j));
                        count++;
                    }
                        
                }
            }

            return sum / count;
        }

        public double GetMaxDisanceBetweenSamples()
        {
            int samplesCount = this.Count;

            double max = 0;
            for (int i = 0; i < samplesCount; i++)
            {
                for (int j = i + 1; j < samplesCount; j++)
                {
                    double distance = this.ElementAt(i).GetXDistanceFromSample(this.ElementAt(j));
                    if (distance > max) max = distance;
                }
            }

            return max;
        }

        public void CountOutputsFromClassLabels()
        {
            List<double> labels = GetLabels();

            Dictionary<double, Vector> labelsMeans = new Dictionary<double, Vector>();          
            foreach (double item in labels)
            {
                labelsMeans.Add(item, this.GetMeanOfDataWithLabel(item));
            }

            foreach (Sample item in this)
            {
                item.SetY(labelsMeans[item.Label]);
            }
        }

        public List<double> GetLabels()
        {
            List<double> labels = this.GroupBy(i => i.Label).Select(group => group.Key).ToList();
            return labels;
        }

        public List<Sample> GetSamplesOfLabel(double label)
        {
            List<Sample> result = this.Where(l => l.Label == label).ToList();
            return result; 
        }

        /// <summary>
        /// return mean of samples with the same label
        /// </summary>
        /// <param name="label">label of samples</param>
        /// <returns>return mean of samples with the same label</returns>
        public Vector GetMeanOfDataWithLabel(double label)
        {
            List<Sample> samples = this.GetSamplesOfLabel(label);

            return Sample.GetXMeanOfSamples(samples);
        }


    }
}
