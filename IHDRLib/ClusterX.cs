using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class ClusterX : Cluster
    {
        private Node child;

        public ClusterX() : base()
        {
            this.child = null;
        }

        public ClusterX(Sample sample) : base(sample)
        {
            this.child = null;
            this.dimension = Params.inputDataDimension;

            items.Add(new Vector(sample.X.ToArray(), sample.Label));

            this.mean = new Vector(sample.X.ToArray());

            //#warning TODO count covariance matrix 
            this.covarianceMatrix = new DenseMatrix(Params.inputDataDimension, Params.inputDataDimension, 0.0);
        }

        public void SetClusterPair(ClusterPair clusterPair)
        {
            this.clusterPair = clusterPair;
        }

        public Vector Mean
        {
            get
            {
                return this.mean;
            }
        }

        public DenseMatrix CovMatrix
        {
            get
            {
                return this.covarianceMatrix;
            }
        }

        public ClusterPair ClusterPair
        {
            get { return clusterPair; }
        }
        
        public void AddItem(Vector vector)
        {
            this.items.Add(vector);

            // update mean
            this.UpdateMean(vector);
            // update covariance matrix
            this.UpdateCovarianceMatrix(vector);
        }

        



       
    }
}
