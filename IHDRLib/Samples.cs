using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IHDRLib
{
    [Serializable]
    public class Samples : ISerializable
    {
        private List<Sample> items;

        public Samples()
        {
            this.items = new List<Sample>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("items", items, typeof(List<Sample>));
        }

        // The special constructor is used to deserialize values. 
        public Samples(SerializationInfo info, StreamingContext context)
        {
            items = (List<Sample>)info.GetValue("items", typeof(List<Sample>));
        }

        public List<Sample> Items 
        {
            get
            {
                return this.items;
            }
        }        

        public void AddSample(Sample sample)
        {
            this.items.Add(sample);
        }

        public double GetAverageDisanceBetweenSamples()
        {
            int samplesCount = this.items.Count;

            double sum = 0;
            int count = 0; 
            for (int i = 0; i < samplesCount; i++)
            {
                for (int j = i + 1; j < samplesCount; j++)
                {
                    sum += this.items.ElementAt(i).GetXDistanceFromSample(this.items.ElementAt(j));
                    count++;
                }
            }

            return sum / count;
        }

        public double GetAverageDisanceBetweenSamplesOfOneLabel(double label)
        {
            int samplesCount = this.items.Count;

            double sum = 0;
            int count = 0;
            for (int i = 0; i < samplesCount; i++)
            {
                for (int j = i + 1; j < samplesCount; j++)
                {
                    if (this.items.ElementAt(i).Label == label && this.items.ElementAt(j).Label == label)
                    {
                        sum += this.items.ElementAt(i).GetXDistanceFromSample(this.items.ElementAt(j));
                        count++;
                    }
                        
                }
            }

            return sum / count;
        }

        public double GetMaxDisanceBetweenSamples()
        {
            int samplesCount = this.items.Count;

            double max = 0;
            for (int i = 0; i < samplesCount; i++)
            {
                for (int j = i + 1; j < samplesCount; j++)
                {
                    double distance = this.items.ElementAt(i).GetXDistanceFromSample(this.items.ElementAt(j));
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

            foreach (Sample item in this.items)
            {
                item.SetY(new Vector(labelsMeans[item.Label].Values.ToArray()));
            }
        }

        public List<double> GetLabels()
        {
            List<double> labels = this.items.GroupBy(i => i.Label).Select(group => group.Key).ToList();
            return labels;
        }

        public List<Sample> GetSamplesOfLabel(double label)
        {
            List<Sample> result = this.items.Where(l => l.Label == label).ToList();
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

        public void SaveItemsX(string path)
        {
            foreach (var item in items)
            {
                item.SaveXToBitmap(path);
            }
        }

        public void SaveItemsY(string path)
        {
            foreach (var item in items)
            {
                item.SaveYToBitmap(path);
            }
        }


    }
}
