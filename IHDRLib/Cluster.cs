using ILNumerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IHDRLib
{
    [Serializable]
    public class Cluster : ISerializable
    {
        protected ClusterPair clusterPair;
        
        protected List<Vector> items;
        protected Vector mean;
        protected ILArray<double> meanMDF;
        protected int dimension;
        protected Node parent;

        protected int itemsCount;

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("clusterPair", clusterPair, typeof(ClusterPair));
            info.AddValue("items", items, typeof(List<Vector>));
            info.AddValue("mean", mean, typeof(Vector));
            info.AddValue("meanMDF", meanMDF, typeof(ILArray<double>));
            info.AddValue("parent", parent, typeof(Node));
            info.AddValue("dimension", dimension, typeof(int));
        }

        // The special constructor is used to deserialize values. 
        public Cluster(SerializationInfo info, StreamingContext context)
        {
            clusterPair = (ClusterPair)info.GetValue("clusterPair", typeof(ClusterPair));
            items = (List<Vector>)info.GetValue("items", typeof(List<Vector>));
            mean = (Vector)info.GetValue("mean", typeof(Vector));
            meanMDF = (ILArray<double>)info.GetValue("meanMDF", typeof(ILArray<double>));
            parent = (Node)info.GetValue("parent", typeof(Node));

        }

        public Cluster(Node parent)
        {
            this.clusterPair = null;
            this.items = new List<Vector>();
            this.mean = null;
            this.parent = parent;
            this.itemsCount = 0;
        }

        public Cluster(Sample sample, Node parent)
        {
            this.items = new List<Vector>();
            this.parent = parent;
        }

        public void SetClusterPair(ClusterPair clusterPair)
        {
            this.clusterPair = clusterPair;
        }

        public void CountMDFMean()
        {
            if (this.items.Count == 1)
            {
                this.meanMDF = this.items[0].ValuesMDF.ToArray();
            }
            else
            {
                // initialize array with nulls
                ILArray<double> sum = ILMath.zeros(this.items[0].ValuesMDF.Length);
                for (int i = 0; i < this.items.Count; i++)
                {
                    sum = ILMath.add(sum, items[i].ValuesMDF);
                }
                this.meanMDF = sum / items.Count;
            }
        }

        public double GetAmnesicParameter(double t)
        {
            if (t < Params.t1)
            {
                return 0;
            }
            if (t >= Params.t1 && t < Params.t2)
            {
                return Params.c * ((t - Params.t1) / (Params.t2 - Params.t1));
            }
            if (t >= Params.t2)
            {
                return Params.c + ((t - Params.t2) / Params.m);
            }
            throw new InvalidOperationException("Bad t");
        }

        /// <summary>
        /// The dispose items.
        /// </summary>
        public void DisposeItems()
        {
            this.items = new List<Vector>();
        }

        /// <summary>
        /// Update mean with amnesic average
        /// </summary>
        /// <param name="vector">Vector, which should be add to mean</param>
        protected void UpdateMean(Vector vector)
        {
            #warning this must be remade according to F. Amnesic average with parameters t1, t2
            
            double t = (double)this.items.Count;

            //double multiplier1 = (t - 1) / t;
            //double multiplier2 = 1 / t;
            double multiplier1 = (t - 1 - this.GetAmnesicParameter(t)) / t;
            double multiplier2 = (1 + this.GetAmnesicParameter(t)) / t;

            this.mean.Multiply(multiplier1);
            Vector incrementPart = new Vector(vector.Values.ToArray());
            
            incrementPart.Multiply(multiplier2);

            this.mean.Add(incrementPart);
        }

        /// <summary>
        /// Update MDF mean with amnesic average
        /// </summary>
        /// <param name="vector">Vector, which should be add to mean</param>
        protected void UpdateMeanMDF(ILArray<double> vector)
        {
            double t = (double)this.items.Count;

            //double multiplier1 = (t - 1) / t;
            //double multiplier2 = 1 / t;
            double multiplier1 = (t - 1 - this.GetAmnesicParameter(t)) / t;
            double multiplier2 = (1 + this.GetAmnesicParameter(t)) / t;

            Vector incrementPart = new Vector(vector.ToArray());

            this.meanMDF = (this.meanMDF * multiplier1) + (vector * multiplier2);
        }

        public void AddItemWithoutUpdatingStats(Vector vector)
        {
            this.items.Add(vector);
            this.itemsCount++;
        }

        public List<Vector> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        public string SavePath { get; set; }

        public Vector Mean
        {
            get
            {
                return this.mean;
            }
            set
            {
                this.mean = value;
            }
        }

        public Node Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        public ILArray<double> MeanMDF
        {
            get
            {
                return this.meanMDF;
            }
            set
            {
                this.meanMDF = value;
            }
        }

        public ClusterPair ClusterPair
        {
            get
            {
                return this.clusterPair;
            }
            set
            {
                this.clusterPair = value;
            }
        }
    }
}
