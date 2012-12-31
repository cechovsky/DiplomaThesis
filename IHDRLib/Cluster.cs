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
        protected ILArray<double> covarianceMatrix;
        protected ILArray<double> covarianceMatrixMDF;
        protected List<Vector> items;
        protected Vector mean;
        protected ILArray<double> meanMDF;
        protected int dimension;

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

        public void UpdateCovarianceMatrix(Vector vector)
        {

#warning this must be remage according to F. Amnesic average with parameters t1, t2

            // newCov = t-1/t * cov(t-1) + 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // oldPart = t-1/t * cov(t-1)
            // incrementalPart = 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // vector1 = (newVector - mean(t))
            // vector2 = (newVector - mean(t))T
            // newCovPart = vector1 * vector2

            //newVector - mean(t)
            Vector v1 = new Vector(vector.ToArray());
            v1.Subtract(this.mean);

            ILArray<double> vector1 = v1.ToArray();
            ILArray<double> vector2 = v1.ToArray();
            // transpone
            vector2 = vector2.T;

#warning need to edit update covariance matrix
            double t = (double) this.items.Count;
            double fragment1 = (t - 2) / (t - 1);
            double fragment2 = t / (Math.Pow((t-1),2));

            //DenseMatrix oldPart = fragment1 * this.covarianceMatrix;
            //DenseMatrix newCovPart = vector1 * vector2;

            try
            {
                //DenseMatrix incrementalPart = newCovPart * fragment2;
                this.covarianceMatrix =  (this.covarianceMatrix * fragment1) + (ILMath.multiply(vector1, vector2) * fragment2);
            }
            catch (Exception ee)
            {
                throw new InvalidCastException();
            }
            
        }

        public void UpdateCovarianceMatrixMDF(ILArray<double> vector)
        {

#warning this must be remage according to F. Amnesic average with parameters t1, t2

            // newCov = t-1/t * cov(t-1) + 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // oldPart = t-1/t * cov(t-1)
            // incrementalPart = 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // vector1 = (newVector - mean(t))
            // vector2 = (newVector - mean(t))T
            // newCovPart = vector1 * vector2

            //newVector - mean(t)
            Vector v1 = new Vector(vector.ToArray());
            v1.Subtract(this.mean);

            ILArray<double> vector1 = (vector - this.meanMDF).ToArray();
            ILArray<double> vector2 = vector1.ToArray();
            // transpone
            vector2 = vector2.T;

#warning need to edit update covariance matrix
            double t = (double)this.items.Count;
            double fragment1 = (t - 2) / (t - 1);
            double fragment2 = t / (Math.Pow((t - 1), 2));

            //DenseMatrix oldPart = fragment1 * this.covarianceMatrix;
            //DenseMatrix newCovPart = vector1 * vector2;

            try
            {
                //DenseMatrix incrementalPart = newCovPart * fragment2;
                this.covarianceMatrixMDF = (this.covarianceMatrixMDF * fragment1) + (ILMath.multiply(vector1, vector2) * fragment2);
            }
            catch (Exception ee)
            {
                throw new InvalidCastException();
            }

        }

        public void CountCovariacneMatrix()
        {
            this.mean = Vector.GetMeanOfVectors(this.items);
            this.covarianceMatrix = ILMath.zeros(Params.inputDataDimension, Params.inputDataDimension);

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    double sum = 0.0;
                    foreach (Vector item in items)
                    {
                        sum += (item[i] - this.mean[i]) * (item[j] - this.mean[j]);
                    }
                    this.covarianceMatrix[i,j] = sum / (items.Count - 1);
                }
            }
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

        public void AddItem(Vector vector)
        {
            vector.Id = this.items.Count + 1;
            this.items.Add(vector);

            // update mean
            this.UpdateMean(vector);
            // update covariance matrix
            this.UpdateCovarianceMatrix(vector);
        }

        public void AddItemNonLeaf(Vector vector)
        {
            vector.Id = this.items.Count + 1;
            this.items.Add(vector);
        }

        public void UpdateMeanAndCovMatrixMDF()
        {
            // update mean
            this.UpdateMean(vector);
            // update covariance matrix
            this.UpdateCovarianceMatrix(vector);
        }

        public void AddItemWithoutUpdatingStats(Vector vector)
        {
            this.items.Add(vector);
        }

        public ILArray<double> CovMatrix
        {
            get
            {
                return this.covarianceMatrix;
            }
            set
            {
                this.covarianceMatrix = value;
            }
        }

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
