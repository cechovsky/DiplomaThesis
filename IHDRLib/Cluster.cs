using ILNumerics;
using MathNet.Numerics.LinearAlgebra.Double;
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
        
        protected List<Vector> items;
        protected Vector mean;
        protected ILArray<double> meanMDF;
        protected int dimension;

        public Cluster()
        {
            this.clusterPair = null;
            
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

        public void CountMDFMean()
        {
            // initialize array with nulls
            ILArray<double> sum = ILMath.zeros(this.items[0].MostDiscrimatingFeatures.Length);

            foreach (var item in this.items)
            {
                sum = sum + item.MostDiscrimatingFeatures;
            }

            this.meanMDF = sum / items.Count;
        }

        /// <summary>
        /// Update mean with amnesic average
        /// </summary>
        /// <param name="vector">Vector, which should be add to mean</param>
        protected void UpdateMean(Vector vector)
        {
            #warning this must be remade according to F. Amnesic average with parameters t1, t2
            
            decimal t = (decimal)this.items.Count;

            decimal multiplier1 = (t - 1) / t;
            this.mean.Multiply(multiplier1);

            Vector incrementPart = new Vector(vector.ToArray());
            decimal multiplier2 = 1 / t;
            incrementPart.Multiply(multiplier2);

            this.mean.Add(incrementPart);
        }

        /// <summary>
        /// Update MDF mean with amnesic average
        /// </summary>
        /// <param name="vector">Vector, which should be add to mean</param>
        protected void UpdateMeanMDF(ILArray<double> vector)
        {
#warning this must be remade according to F. Amnesic average with parameters t1, t2

            double t = (double)this.items.Count;

            double multiplier1 = (t - 1) / t;
            

            Vector incrementPart = new Vector(vector.ToArray());
            double multiplier2 = 1 / t;

            this.meanMDF = (this.meanMDF * multiplier1) + (vector * multiplier2);
        }

        public void AddItemWithoutUpdatingStats(Vector vector)
        {
            this.items.Add(vector);
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
