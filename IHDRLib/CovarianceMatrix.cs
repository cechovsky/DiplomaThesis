using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace IHDRLib
{
    public class CovarianceMatrix
    {
        private DenseMatrix matrix;
        private Vector mean;
        private int dimension;

        public CovarianceMatrix(Vector mean, int dimension)
        {
            // reference to mean from ClusterX
            this.mean = mean;
            this.dimension = dimension;
            this.matrix = new DenseMatrix(dimension, dimension, 0.0);
        }

        public DenseMatrix Matrix
        {
            get
            {
                return this.matrix;
            }
        }

        public void UpdateMatrix(Vector vector, Vector newMean, int t)
        {
            #warning this must be remage according to F. Amnesic average with parameters t1, t2

            // newCov = t-1/t * cov(t-1) + 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // oldPart = t-1/t * cov(t-1)
            // incrementalPart = 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // vector1 = (newVector - mean(t))
            // vector2 = (newVector - mean(t))T
            // newCovPart = vector1 * vector2

            DenseMatrix vector1 = new DenseMatrix(this.dimension, 1);
            vector1.SetColumn(0, vector.ToArray());
            DenseMatrix vector2 = new DenseMatrix(1, this.dimension);
            vector2.SetRow(0, vector.ToArray());

            double tt = (double)t;
            double fragment1 = (tt - 1) / tt;
            double fragment2 = 1 / tt;


            DenseMatrix oldPart = this.matrix * fragment1;
            DenseMatrix newCovPart = vector1 * vector2;
            
            DenseMatrix incrementalPart = newCovPart * fragment2;

            this.matrix = oldPart + incrementalPart;
        }
    }
}
