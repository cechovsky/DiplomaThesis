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
    public class ClusterPair : ISerializable
    {
        private ClusterX clusterX;
        private ClusterY clusterY;
        private List<Sample> samples;
        private Node correspondChild;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("clusterX", clusterX, typeof(ClusterX));
            info.AddValue("clusterY", clusterY, typeof(ClusterY));
            info.AddValue("correspondChild", correspondChild, typeof(Node));
        }

        // The special constructor is used to deserialize values. 
        public ClusterPair(SerializationInfo info, StreamingContext context)
        {
            clusterX = (ClusterX)info.GetValue("clusterX", typeof(ClusterX));
            clusterY = (ClusterY)info.GetValue("clusterY", typeof(ClusterY));
            correspondChild = (Node)info.GetValue("correspondChild", typeof(Node));
        }

        /// <summary>
        /// create cluster pair with clusterX and clusterY, not set Clusters parents
        /// </summary>
        public ClusterPair()
        {
            this.clusterX = new ClusterX(null);
            this.clusterX.SetClusterPair(this);
            this.clusterY = new ClusterY(null);
            this.clusterY.SetClusterPair(this);
            this.samples = new List<Sample>();
        }

        public ClusterPair(ClusterX cX, ClusterY cY, Sample sample)
        {
            clusterX = cX;
            clusterY = cY;

            this.PreviousCenter = 0;
            this.CurrentCenter = 0;

            this.samples = new List<Sample>() { sample };
        }

        public ClusterPair GetClone()
        {
            ClusterPair cp = new ClusterPair();

            foreach (var x in this.X.Items)
            {
                cp.X.AddItemWithoutUpdatingStats(x);
                cp.X.ClusterPair = cp;

            }
            foreach (var y in this.Y.Items)
            {
                cp.Y.AddItemWithoutUpdatingStats(y);
            }

            try
            {
                //cp.X.CovMatrix = ILMath.array(this.X.CovMatrix.ToArray(), Params.inputDataDimension, Params.inputDataDimension);
                cp.X.Mean = new Vector(this.X.Mean.Values.ToArray());

                cp.Y.Mean = new Vector(this.Y.Mean.Values.ToArray());
            }
            catch (Exception ee)
            {
                throw new InvalidOperationException("");
            }

            if (cp.X.Mean == null || cp.Y.Mean == null)
            {
                throw new InvalidCastException("Bad clone");
            }
            
            return cp;
        }

        public void SetParentsToXCluster(Node parent)
        {
            this.clusterX.Parent = parent;
        }

        public ClusterX X
        {
            get
            {
                return clusterX;
            }
        }

        public ClusterY Y
        {
            get
            {
                return clusterY;
            }
        }

        public List<Sample> Samples
        {
            get
            {
                return this.samples;
            }
            set
            {
                this.samples = value;
            }
        }

        public Node CorrespondChild
        {
            get
            {
                return this.correspondChild;
            }
            set
            {
                this.correspondChild = value;
            }
        }

        public void CurrentToPrev()
        {
            this.PreviousCenter = this.CurrentCenter;
            this.CurrentCenter = -1;
        }

        public void AddItem(Sample s)
        {
            this.X.AddItem(s.X, s.Label);
            this.Y.AddItem(s.Y, s.Label);
            this.samples.Add(s);
        }


        public int Id { get; set; }
        public int PreviousCenter { get; set; }
        public int CurrentCenter { get; set; }

    }
}
