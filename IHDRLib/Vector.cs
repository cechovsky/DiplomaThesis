using ILNumerics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IHDRLib
{
    [Serializable]
    public class Vector : ISerializable
    {
        double label;
        ILArray<double> valuesMDF;
        ILArray<double> values;
        int id;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("label", label, typeof(double));
            info.AddValue("valuesMDF", valuesMDF, typeof(ILArray<double>));
            info.AddValue("values", values, typeof(ILArray<double>));
            info.AddValue("id", id, typeof(int));
        }

        // The special constructor is used to deserialize values. 
        public Vector(SerializationInfo info, StreamingContext context)
        {
            label = (double)info.GetValue("label", typeof(double));
            valuesMDF = (ILArray<double>)info.GetValue("valuesMDF", typeof(ILArray<double>));
            values = (ILArray<double>)info.GetValue("values", typeof(ILArray<double>));
            id = (int)info.GetValue("id", typeof(int));
        }


        /// <summary>
        /// insert attributes in array to vector
        /// </summary>
        /// <param name="vector"></param>
        public Vector(double[] vector) : base()
        {
            this.values = vector.ToArray();
        }

        public Vector(double[] vector, double label, int id)
            : base()
        {
            this.values = vector.ToArray();
            this.label = label;
            this.Id = id;
        }

        public ILArray<double> ValuesMDF
        {
            get 
            {
                return this.valuesMDF;
            }
            set
            {
                this.valuesMDF = value;
            }
        }

        public ILArray<double> Values
        {
            get
            {
                return this.values;
            }
            set
            {
                this.values = value;
            }
        }

        public double Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        public void CountMDF(ILArray<double> GSOmanifold, ILArray<double> C)
        {
            ILArray<double> scaterPart = this.values - C;

            this.valuesMDF = ILMath.multiply(GSOmanifold.T, scaterPart);
        }

        public Vector(int dimension, double value)
        {
            this.values = ILMath.array<double>(value, dimension);
        }

        public void Add(Vector vector)
        {
            if (this.values.Length != vector.values.Length) throw new InvalidOperationException("Not the same count of attributes");

            this.values = this.values + vector.values;
        }

        public void Subtract(Vector vector)
        {
            if (this.values.Length != vector.values.Length) throw new InvalidOperationException("Not the same count of attributes");

            this.values = this.values - vector.values;
        }

        public void Divide(double divider)
        {
            this.values = this.values / divider;
        }

        public void Multiply(double multiplier)
        {
            this.values = this.values * multiplier;
        }

        public bool EqualsToVector(Vector vector)
        {
            if (this.values.Length != vector.values.Length) throw new InvalidOperationException("Not the same count of attributes");
            for (int i = 0; i < this.values.Length; i++)
            {
                if (this.values[i] != vector.values[i]) return false;
            }
            return true;
        }

        public double GetDistance(Vector vector)
        {
            if (this.values.Count() != vector.values.Count()) throw new InvalidOperationException("Not the same count of attributes");

            double sum = 0.0;
            for (int i = 0; i < this.values.Length; i++)
            {
                sum += Math.Pow((this.values[i] - vector.values[i]).ToArray()[0], 2);
            }
            if (sum == 0) return 0;
            return Math.Sqrt(sum);
        }

        public double GetMDFDistance(ILArray<double> vector)
        {
            ILArray<double> delta = this.valuesMDF - vector;
            double result = ILMath.multiply(delta.T, delta).ToArray()[0];
            if (result == 0) return 0;
            return Math.Sqrt(result);
        }
        

        public static Vector GetMeanOfVectors(List<Vector> vectors)
        {
            Vector result = new Vector(vectors[0].values.Length, 0.0);

            foreach (Vector item in vectors)
            {
                result.Add(item);
            }

            result.Divide((double)vectors.Count);

            return result;
        }

        public static int GetIdOfClosestVector(Vector vector, List<Vector> vectors)
        {
            int result = 0;
            double minDistance = double.MaxValue;

            foreach (Vector item in vectors)
            {
                double distance = vector.GetDistance(item);
                if (distance < minDistance)
                {
                    result = item.Id;
                    minDistance = distance;
                }
            }

            return result;
        }

        public void SaveToBitmap(string locationPath, bool isMean)
        {
            Bitmap bitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    bitmap.SetPixel(j, i, Color.FromArgb((int)this.values[i * 28 + j], (int)this.values[i * 28 + j], (int)this.values[i * 28 + j]));
                }
            }

            // create directory
            DirectoryInfo dir = new DirectoryInfo(locationPath);
            if (!dir.Exists)
            {
                dir.Create();
            }


            if (isMean)
            {
                bitmap.Save(locationPath + @"\mean.bmp");
            }
            else
            {
                bitmap.Save(locationPath + @"\vector_" + this.Id + "_" + this.label.ToString() + ".bmp");
            }
        }

        public void SaveToBitmap(string locationPath, string fileName)
        {
            Bitmap bitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    bitmap.SetPixel(j, i, Color.FromArgb((int)this.values[i * 28 + j], (int)this.values[i * 28 + j], (int)this.values[i * 28 + j]));
                }
            }

            // create directory
            DirectoryInfo dir = new DirectoryInfo(locationPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
            bitmap.Save(locationPath + @"\"+ fileName + ".bmp");
        }


        /// <summary>
        /// return id of closest vector from list
        /// </summary>
        /// <param name="vectors">vector list</param>
        /// <returns></returns>
        public int GetIdOfClosestVector(List<Vector> vectors)
        {
            int result = 0;
            double minDistance = double.MaxValue;

            foreach (Vector item in vectors)
            {
                double distance = this.GetDistance(item);
                if (distance < minDistance)
                {
                    result = item.Id;
                    minDistance = distance;
                }
            }

            return result;
        }

        public static double GetNormalisationNum(ILArray<double> vector)
        {
            double sum = ILMath.multiply(vector.T, vector).ToArray()[0];
            return Math.Sqrt(sum);
        }

        #region Properties

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

        #endregion
    }
}
