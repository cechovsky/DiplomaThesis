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
        public ClusterY(Node parent)
            : base(parent)
        {
            this.dimension = Params.outputDataDimension;
        }

        public ClusterY(Sample sample, Node parent)
            : base(sample, parent)
        {
            this.dimension = Params.outputDataDimension;

            this.items.Add(new Vector(sample.Y.ToArray(), sample.Label, this.items.Count + 1));
            this.mean = new Vector(sample.Y.ToArray());

        }

        public void AddItem(Vector vector)
        {
            vector.Id = this.items.Count + 1;
            this.items.Add(vector);

            // update mean
            this.UpdateMean(vector);
        }

    }
}
