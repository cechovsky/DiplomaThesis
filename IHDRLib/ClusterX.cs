using ILNumerics;
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
            this.dimension = Params.inputDataDimension;
        }

        public ClusterX(Sample sample) : base(sample)
        {
            this.child = null;
            this.dimension = Params.inputDataDimension;

            items.Add(new Vector(sample.X.ToArray(), sample.Label, 1));

            this.mean = new Vector(sample.X.ToArray());

            //#warning TODO count covariance matrix 
            this.covarianceMatrix = ILMath.zeros(Params.inputDataDimension, Params.inputDataDimension);
        }

        public void SetClusterPair(ClusterPair clusterPair)
        {
            this.clusterPair = clusterPair;
        }

        public void SaveSamples()
        {
            foreach (var item in items)
            {
                item.SaveToBitmap(this.SavePath);
            }
        }
    }
}
