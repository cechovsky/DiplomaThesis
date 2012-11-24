using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Sample
    {
        private Vector x;
        private Vector y;
        private double label;

        public Sample(double[] input, double label)
        {
            this.x = new Vector(input);
            this.label = label;
        }

        public Sample(double[] input, double[] output)
        {
            this.x = new Vector(input);
            this.y = new Vector(input);
        }

        public Vector X
        {
            get
            {
                return x;
            }
        }

        public Vector Y
        {
            get
            {
                return y;
            }
        }

        public double Label
        {
            get
            {
                return label;
            }
        }

        public double GetXDistanceFromSample(Sample sample)
        {
            int count = this.x.Count;
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += Math.Pow(this.x.ElementAt(i) - sample.X.ElementAt(i), 2);
            }
            return Math.Sqrt(sum);
        }

        public void SetY(Vector y)
        {
            this.y = y; 
        }

        public static Vector GetXMeanOfSamples(List<Sample> samples)
        {
            if (samples.Count == 0) throw new InvalidOperationException("impossible to return mean from 0 samples");

            int count = samples.Count;
            
            Vector result = new Vector(Params.inputDataDimension, 0.0);
            foreach (Sample sample in samples)
            {
                result.Add(sample.X);
            }

            result.Divide(count);
            return result;
        }

        public static Vector GetYMeanOfSamples(List<Sample> samples)
        {
            int count = samples.Count;

            Vector result = new Vector(Params.inputDataDimension, 0.0);

            foreach (Sample sample in samples)
            {
                result.Add(sample.Y);
            }

            result.Divide(count);

            return result;
        }
        
    }
}
