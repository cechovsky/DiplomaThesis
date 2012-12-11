using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Vector : List<double>
    {
        double label;
        
        /// <summary>
        /// insert attributes in array to vector
        /// </summary>
        /// <param name="vector"></param>
        public Vector(double[] vector) : base()
        {
            this.InsertRange(0, vector);
        }

        public Vector(double[] vector, double label) : base()
        {
            this.InsertRange(0, vector);
            this.label = label;
        }

        public Vector(double[] vector, double label, int id)
            : base()
        {
            this.InsertRange(0, vector);
            this.label = label;
            this.Id = id;
        }

        public Vector(int dimension, double value): base(dimension)
        {
            for (int i = 0; i < dimension; i++)
            {
                this.Add(value);
            }
        }

        public void Add(Vector vector)
        {
            if (this.Count != vector.Count) throw new InvalidOperationException("Not the same count of attributes");

            for (int i = 0; i < this.Count; i++)
            {
                decimal res = (decimal)this[i] + (decimal)vector[i];
                this[i] = (double)res;
            }
        }

        public void Subtract(Vector vector)
        {
            if (this.Count != vector.Count) throw new InvalidOperationException("Not the same count of attributes");

            for (int i = 0; i < this.Count; i++)
            {
                decimal res = (decimal)this[i] - (decimal)vector[i];
                this[i] = (double)res;
            }
        }

        public void Divide(decimal divider)
        {
            for (int i = 0; i < this.Count; i++)
            {
                decimal res = (decimal)this[i] / (decimal)divider;
                this[i] = (double)res;
            }
        }

        public void Multiply(decimal multiplier)
        {
            for (int i = 0; i < this.Count; i++)
            {
                decimal res = (decimal)this[i] * multiplier;
                this[i] = (double)res;
            }
        }

        public bool EqualsToVector(Vector vector)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != vector[i]) return false;
            }
            return true;
        }

        public double GetDistance(Vector vector)
        {
            if (this.Count != vector.Count) throw new InvalidOperationException("Different count of attributes");

            double sum = 0.0;
            for (int i = 0; i < this.Count; i++)
            {
                sum += Math.Pow(this[i] - vector[i], 2);
            }
            return Math.Sqrt(sum);
        }

        public static Vector GetMeanOfVectors(List<Vector> vectors)
        {
            if(vectors.Count == null) throw new InvalidOperationException("impossible to return mean from 0 vectors");
            Vector result = new Vector(vectors[0].Count, 0.0);

            foreach (Vector item in vectors)
            {
                result.Add(item);
            }

            result.Divide((decimal)vectors.Count);

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

        #region Properties

        public int Id { get; set; }

        #endregion
    }
}
