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
        protected ILArray<double> covarianceMatrix;
        protected ILArray<double> covarianceMatrixMDF;

        public ClusterX() : base()
        {
            this.child = null;
            this.dimension = Params.inputDataDimension;
            this.covarianceMatrix = null;
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

        public void CountMDFOfItems(ILArray<double> gSOManifold)
        {
            foreach (var vector in items)
            {
                vector.CountMostDiscrimatingFeatures(gSOManifold);
            }
        }

        public double GetMDFDistanceFromMDFMean(ILArray<double> vector)
        {
            ILArray<double> delta = this.meanMDF - vector;
            double result = ILMath.multiply(delta.T, delta).ToArray()[0];
            if (result == 0) return 0;
            return Math.Sqrt(result);
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
            double t = (double)this.items.Count;
            double fragment1 = (t - 2) / (t - 1);
            double fragment2 = t / (Math.Pow((t - 1), 2));

            //DenseMatrix oldPart = fragment1 * this.covarianceMatrix;
            //DenseMatrix newCovPart = vector1 * vector2;

            try
            {
                //DenseMatrix incrementalPart = newCovPart * fragment2;
                this.covarianceMatrix = (this.covarianceMatrix * fragment1) + (ILMath.multiply(vector1, vector2) * fragment2);
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
            //Vector v1 = new Vector(vector.ToArray());
            //v1.Subtract(this.meanMD);

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
                    this.covarianceMatrix[i, j] = sum / (items.Count - 1);
                }
            }
        }

        public void CountCovarianceMatrixMDF()
        {
            if (this.meanMDF == null)
            {
                this.CountMDFMean();
            }

            int mdfDimension = this.meanMDF.Length;
            this.covarianceMatrixMDF = ILMath.zeros(mdfDimension, mdfDimension);

            for (int i = 0; i < mdfDimension; i++)
            {
                for (int j = 0; j < mdfDimension; j++)
                {
                    double sum = 0.0;
                    foreach (Vector item in items)
                    {
                        double d = (double)this.meanMDF[i];
                        sum += ((double)item.MostDiscrimatingFeatures[i] - (double)this.meanMDF[i]) * ((double)item.MostDiscrimatingFeatures[j] - -(double)this.meanMDF[j]);
                    }
                    this.covarianceMatrixMDF[i, j] = sum / (items.Count - 1);
                }
            }
        }

        public void DisposeCovMatrix()
        {
            this.covarianceMatrix.Dispose();
        }

        public void AddItem(Vector vector)
        {
            vector.Id = this.items.Count + 1;
            this.items.Add(vector);

            // update mean
            this.UpdateMean(vector);
            // update covariance matrix
            this.UpdateCovarianceMatrix(vector);
        }

        public void AddItemNonLeaf(Vector vector, Node node)
        {
            vector.Id = this.items.Count + 1;
            this.items.Add(vector);

            // update mean
            this.UpdateMean(vector);

            node.CountGSOManifold();

            vector.CountMostDiscrimatingFeatures(node.GSOManifold);

            this.UpdateMeanAndCovMatrixMDF(vector.MostDiscrimatingFeatures.ToArray());   
        }

        public void UpdateMeanAndCovMatrixMDF(ILArray<double> vector)
        {
            // update mean
            this.UpdateMeanMDF(vector);
            // update covariance matrix
            this.UpdateCovarianceMatrixMDF(vector);
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

    }
}
