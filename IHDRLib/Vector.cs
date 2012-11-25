﻿using System;
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

        public Vector(double[] vector, double label)
            : base()
        {
            this.InsertRange(0, vector);
            this.label = label;
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
                this[i] = this[i] + vector[i];
            }
        }

        public void Substract(Vector vector)
        {
            if (this.Count != vector.Count) throw new InvalidOperationException("Not the same count of attributes");

            for (int i = 0; i < this.Count; i++)
            {
                double res = this[i] - vector[i];
                this[i] =res;
            }
        }

        public void Divide(double divider)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i] = this[i] / divider;
            }
        }

        public void Multiply(double multiplier)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i] = this[i] * multiplier;
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
    }
}
