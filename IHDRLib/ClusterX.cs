using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class ClusterX : Cluster
    {
        private Node child;

        public ClusterX()
            : base()
        {
            this.child = null;
        }

        public ClusterX(Sample sample) : base(sample)
        {
            this.child = null;

            items.Add(new Vector(sample.X.ToArray(), sample.Label));

            this.mean = new Vector(sample.X.ToArray());

            //#warning TODO count covariance matrix 
            this.covarianceMatrix = new CovarianceMatrix(this.mean);
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

        public CovarianceMatrix CovMatrix
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

    }
}
