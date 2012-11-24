using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class ClusterY : Cluster
    {
        public ClusterY() : base()
        {
            
        }

        public ClusterY(Sample sample) : base(sample)
        {
            this.items.Add(new Vector(sample.Y.ToArray(), sample.Label));
            this.mean = new Vector(sample.Y.ToArray());
            
            this.covarianceMatrix = new CovarianceMatrix(this.mean, Params.outputDataDimension);
        }

        public void AddItem(Vector vector)
        {
            this.items.Add(vector);

            // update mean
            this.UpdateMean(vector);
            // update covariance matrix
            this.covarianceMatrix.UpdateMatrix(vector, this.mean, this.items.Count);
        }

        



    }
}
