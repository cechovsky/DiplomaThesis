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

        public CovarianceMatrix(Vector mean)
        {
            // reference to mean from ClusterX
            this.mean = mean;
            this.matrix = new DenseMatrix(Params.inputDataDimension, Params.inputDataDimension, 0.0);
        }

        public DenseMatrix Matrix
        {
            get
            {
                return this.matrix;
            }
        }
    }
}
