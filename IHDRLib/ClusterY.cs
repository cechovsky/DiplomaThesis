using ILNumerics;
using MathNet.Numerics.LinearAlgebra.Double;
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
            this.dimension = Params.outputDataDimension;
        }

        public ClusterY(Sample sample) : base(sample)
        {
            this.dimension = Params.outputDataDimension;

            this.items.Add(new Vector(sample.Y.ToArray(), sample.Label, this.items.Count + 1));
            this.mean = new Vector(sample.Y.ToArray());

            this.covarianceMatrix = ILMath.zeros(Params.outputDataDimension, Params.outputDataDimension);
        }

        

        



    }
}
