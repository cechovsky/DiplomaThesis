using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;

namespace IHDRLib
{
    [Serializable]
    public class Sample : ISerializable
    {
        private Vector x;
        private Vector y;
        private double label;
        private int id;

        public int CenterId { get; set; }
        public int ClusterAssignemntOld { get; set; }
        public int ClusterAssignemntNew { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", x, typeof(Vector));
            info.AddValue("y", y, typeof(Vector));
            info.AddValue("label", label, typeof(double));
            info.AddValue("id", id, typeof(int));
        }

        // The special constructor is used to deserialize values. 
        public Sample(SerializationInfo info, StreamingContext context)
        {
            x = (Vector)info.GetValue("x", typeof(Vector));
            y = (Vector)info.GetValue("y", typeof(Vector));
            label = (double)info.GetValue("label", typeof(double));
            id = (int)info.GetValue("id", typeof(int));
        }

        public Sample(double[] input, double label, int id)
        {
            this.x = new Vector(input);
            this.label = label;
            this.id = id;
        }

        public Sample(double[] input, double[] output, double label, int id)
        {
            this.x = new Vector(input);
            this.y = new Vector(output);
            this.id = id;
            this.label = label;
        }

        public Vector X
        {
            get
            {
                return x;
            }
            set
            {
                this.x = value;
            }
        }

        public Vector Y
        {
            get
            {
                return y;
            }
        }

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
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
            int count = this.x.Values.Length;
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += Math.Pow(this.x.Values.ToArray()[i] - sample.X.Values.ToArray()[i], 2);
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

        public void SaveXToBitmap(string locationPath)
        {
            Bitmap bitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    bitmap.SetPixel(j, i, Color.FromArgb((int)x.Values[i * 28 + j], (int)x.Values[i * 28 + j], (int)x.Values[i * 28 + j]));
                }
            }

            bitmap.Save(locationPath + @"\sample_" + this.Id + "_" + this.Label + ".bmp");
        }

        public void SaveYToBitmap(string locationPath)
        {
            Bitmap bitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    bitmap.SetPixel(j, i, Color.FromArgb((int)y.Values[i * 28 + j], (int)y.Values[i * 28 + j], (int)y.Values[i * 28 + j]));
                }
            }

            bitmap.Save(locationPath + @"\sample_" + this.Id + "_" + this.Label + ".bmp");
        }
    }
}
