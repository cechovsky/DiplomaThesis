using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Cluster
    {
        protected ClusterPair clusterPair;
        protected CovarianceMatrix covarianceMatrix;
        protected List<Vector> items;
        protected Vector mean;

        public Cluster()
        {
            
            this.clusterPair = null;
            this.covarianceMatrix = null;
            this.items = new List<Vector>();
            this.mean = null;
        }

        public Cluster(Sample sample)
        {
            this.items = new List<Vector>();
        }

        public void SetClusterPair(ClusterPair clusterPair)
        {
            this.clusterPair = clusterPair;
        }
    }
}
