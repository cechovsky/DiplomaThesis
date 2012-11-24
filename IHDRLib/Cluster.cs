using MathNet.Numerics.LinearAlgebra.Double;
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

        

        /// <summary>
        /// Update mean with amnesic average
        /// </summary>
        /// <param name="vector">Vector, which should be add to mean</param>
        protected void UpdateMean(Vector vector)
        {
            #warning this must be remade according to F. Amnesic average with parameters t1, t2
            
            double t = this.items.Count;
            this.mean.Multiply((t-1)/t);

            Vector incrementPart = new Vector(vector.ToArray());
            incrementPart.Multiply(1 / t);

            this.mean.Add(incrementPart);
        }
    }
}
