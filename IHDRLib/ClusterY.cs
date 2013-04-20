using ILNumerics;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IHDRLib
{
    [Serializable]
    public class ClusterY : Cluster
    {

        public ClusterY(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public ClusterY(Node parent)
            : base(parent)
        {
            this.dimension = Params.outputDataDimension;
        }

        public ClusterY(Sample sample, Node parent)
            : base(sample, parent)
        {
            this.dimension = Params.outputDataDimension;

            this.items.Add(new Vector(sample.Y.Values.ToArray(), sample.Label, this.items.Count + 1));
            this.mean = new Vector(sample.Y.Values.ToArray());
        }

        public void AddItem(Vector vector, double label)
        {
            Vector newItem = new Vector(vector.Values.ToArray());
            newItem.Label = label;
            newItem.Id = this.items.Count + 1;

            this.items.Add(newItem);
            // update mean
            this.UpdateMean(newItem);
        }

    }
}
