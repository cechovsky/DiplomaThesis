using ILNumerics;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class ClusterX : Cluster
    {
        private Node child;
        protected ILArray<double> covarianceMatrix;
        protected ILArray<double> covarianceMatrixMDF;

        public ClusterX(Node parent)
            : base(parent)
        {
            this.child = null;
            this.dimension = Params.inputDataDimension;
            this.covarianceMatrix = null;
        }

        public ClusterX(Sample sample, Node parent)
            : base(sample, parent)
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
                item.SaveToBitmap(this.SavePath, false);
            }

            if (Params.SaveMeansMDF)
            {
                if (this.meanMDF != null)
                {
                    this.SaveILArrayToCsvFile(this.meanMDF, 1, this.meanMDF.Length - 1, this.SavePath + @"\MeanMdf.txt");
                }
            }
            if (Params.SaveCovMatricesMDF)
            {
                if (this.covarianceMatrixMDF != null && this.meanMDF != null)
                {
                   this.SaveILArrayToCsvFile(this.covarianceMatrixMDF, this.meanMDF.Length - 1, this.meanMDF.Length - 1, this.SavePath + @"\CovMatrixMdf.txt");
                }
            }
            this.mean.SaveToBitmap(this.SavePath, true);
        }

        private void SaveMeanMdfToFile()
        {
            if (this.meanMDF != null)
            {
                StringBuilder sb = new StringBuilder();

                double[] array = this.meanMDF.ToArray();

                for (int i = 1; i < this.meanMDF.Length - 1; i++)
                {
                    sb.Append(array[i]);
                }
                sb.Append(array[this.meanMDF.Length - 1].ToString());

                // create directory
            DirectoryInfo dir = new DirectoryInfo(this.SavePath);
            if (!dir.Exists)
            {
                dir.Create();
            }

                File.WriteAllText(this.SavePath + @"\MeanMdf.txt", sb.ToString());
            }
        }

        public void CountMDFOfItems(ILArray<double> gSOManifold, ILArray<double> C)
        {
            foreach (var vector in items)
            {
                vector.CountMDF(gSOManifold, C);
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

            if (items.Count > 1)
            {
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
        }

        public void CountCovarianceMatrixMDF()
        {
            if (this.meanMDF == null)
            {
                this.CountMDFMean();
            }

            // TODO reimplement
            if (this.items.Count == 1)
            {
                //double variance = this.GetVarianceOfVector(this.meanMDF);
                //ILArray<double> identityM = ILMath.eye(this.meanMDF.Length, this.meanMDF.Length);

                this.covarianceMatrixMDF = ILMath.zeros(this.meanMDF.Length, this.meanMDF.Length);
            }
            else
            {
                if (this.meanMDF == null)
                {
                    this.CountMDFMean();
                }

                int mdfDimension = this.meanMDF.Length;
                this.covarianceMatrixMDF = ILMath.zeros(mdfDimension, mdfDimension);

                if (items.Count > 1)
                {
                    for (int i = 0; i < mdfDimension; i++)
                    {
                        for (int j = 0; j < mdfDimension; j++)
                        {
                            double sum = 0.0;
                            foreach (Vector item in items)
                            {
                                double d = (double)this.meanMDF[i];
                                sum += ((double)item.MostDiscrimatingFeatures[i] - (double)this.meanMDF[i]) * ((double)item.MostDiscrimatingFeatures[j] - (double)this.meanMDF[j]);
                            }
                            this.covarianceMatrixMDF[i, j] = Math.Round(sum / (items.Count - 1));
                        }
                    }
                }
            }
        }

        public double GetVarianceOfVector(ILArray<double> vector)
        {
            // count mean
            double sum = 0.0;
            foreach (var item in vector)
            {
                sum += item;
            }
            
            double mean = sum / (double)vector.Length;

            double sum2 = 0.0;

            foreach (var item in vector)
            {
                sum2 += item - mean;
            }
            return sum2 / (double)vector.Length;
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

            vector.CountMDF(node.GSOManifold, node.C);
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

        public ILArray<double> CovMatrixMDF
        {
            get
            {
                return this.covarianceMatrixMDF;
            }
            set
            {
                this.covarianceMatrixMDF = value;
            }
        }

        #region Probability Based Metrics

        public double GetSDNLL(ILArray<double> vector)
        {
            double firstPart = 0;
            double secondPart = 0;
            double thirdPart = 0;

            if (this.items.Count == 1)
            {
                this.covarianceMatrix = this.GetVarianceMatrix();
            }

            ILArray<double> meanIL = this.mean.ToArray();

            ILArray<double> W = this.GetMatrixW();

            ILArray<double> WInverse = 1;
            if (Params.ContainsSingularCovarianceMatrixes)
            {
                //WInverse = this.GetPseudoInverseMatrixOfMatrix(W);
                WInverse = this.GetInverseMatrixOfMatrix(W, Params.inputDataDimension);
            }
            else
            {
                WInverse = this.GetInverseMatrixOfMatrix(W, Params.inputDataDimension);
            }

            ILArray<double> vector1 = meanIL - vector;
            ILArray<double> tmpArray = ILMath.multiply(WInverse, vector1);

            firstPart = 0.5 * ILMath.multiply(vector1.T, tmpArray).ToArray()[0];
            secondPart = Params.inputDataDimension / 2 * ILMath.log(2 * Math.PI).ToArray()[0];
            thirdPart = 0.5 * ILMath.log(ILMath.det(W)).ToArray()[0];

            return firstPart + secondPart + thirdPart;
        }

        public double GetSDNLL_MDF(ILArray<double> vector, ILArray<double> manifold, ILArray<double> c)
        {
            // convert vector to mdf vector
            ILArray<double> scaterPart = vector - c;
            ILArray<double> mdfVector = ILMath.multiply(manifold.T, scaterPart);

            double firstPart = 0;
            double secondPart = 0;
            double thirdPart = 0;

            if (this.items.Count == 1)
            {
                this.covarianceMatrixMDF = this.GetVarianceMatrix_MDF();
            }

#warning implement dimension of MDF to property
            int q = this.meanMDF.Length;

            // get matrix W
            ILArray<double> W = this.GetMatrixW_MDF();
            ILArray<double> WInverse = this.GetInverseMatrixOfMatrix(W, q);

            if (Double.IsNaN(WInverse.ToArray()[0])) throw new InvalidDataException("inverse of covariance matrix MDF is NaN");

            ILArray<double> vector1 = this.meanMDF - mdfVector;
            ILArray<double> tmpArray = ILMath.multiply(WInverse, vector1);

            firstPart = 0.5 * ILMath.multiply(vector1.T, tmpArray).ToArray()[0];
            secondPart = q / 2 * ILMath.log(2 * Math.PI).ToArray()[0];
            thirdPart = 0.5 * ILMath.log(ILMath.det(W)).ToArray()[0];

            return firstPart + secondPart + thirdPart;
        }

        public ILArray<double> GetMatrixW_MDF()
        {
            double be = this.Getbe();
            double bm = this.Getbm();
            double bg = this.Getbg();

            double b = be + bm + bg;

            double we = be / b;
            double wm = we / b;
            double wg = bg / b;

            ILArray<double> gaussianPart = this.covarianceMatrixMDF;
            ILArray<double> mahalonobisPart = this.parent.CovarianceMatrixMeanMDF;
            ILArray<double> euclideanPart = this.GetVarianceMatrix_MDF();

            return (wg * gaussianPart) + (wm * mahalonobisPart) + (we * euclideanPart);
        }

        public ILArray<double> GetMatrixW()
        {
            double be = this.Getbe();
            double bm = this.Getbm();
            double bg = this.Getbg();

            double b = be + bm + bg;

            double we = be / b;
            double wm = we / b;
            double wg = bg / b;

            ILArray<double> result = null;
            
            ILArray<double> gaussianPart = null;
            ILArray<double> mahalonobisPart = null;
            ILArray<double> euclideanPart = null;

            gaussianPart = this.covarianceMatrix;
            mahalonobisPart = this.parent.CovarianceMatrixMean;
            euclideanPart = this.GetVarianceMatrix();

            return result = (wg * gaussianPart) + (wm * mahalonobisPart) + (we * euclideanPart);
        }

        public ILArray<double> GetPseudoInverseMatrixOfMatrix(ILArray<double> inputMatrix)
        {
            ILArray<double> svdValues = ILMath.svd(inputMatrix);
            ILArray<double> svdMatrix = ILMath.diag(svdValues);

            ILArray<double> eigenVectors = 1;
            ILArray<double> eigValues = ILMath.diag(ILMath.eigSymm(this.covarianceMatrix, eigenVectors));

            // count pseudoinverse matrix
            ILArray<double> pseudoInverse = ILMath.multiply(eigenVectors, ILMath.multiply(svdMatrix, eigenVectors.T));

            return pseudoInverse;
        }

        public ILArray<double> GetInverseMatrixOfMatrix(ILArray<double> inputMatrix, int dimension)
        {
            ILArray<double> identityMatrix = ILMath.eye(dimension, dimension);
            ILArray<double> inverseCovMatrix = ILMath.linsolve(inputMatrix, identityMatrix);

            // ILArray<double> testingMatrix = ILMath.multiply(this.covarianceMatrixMDF, inverseCovMatrix);

            return identityMatrix;
        }
                
        public ILArray<double> GetVarianceMatrix()
        {
            ILArray<double> result = ILMath.eye(Params.inputDataDimension, Params.inputDataDimension);
            result = Params.digitizationNoise * result;

            return result;
        }

        public ILArray<double> GetVarianceMatrix_MDF()
        {
            ILArray<double> result = ILMath.eye(this.meanMDF.Length, this.meanMDF.Length);
            result = Params.digitizationNoise * result;

            return result;
        }

        public double Getbe()
        {
            double nspp = (this.items.Count - 1) * (parent.ClustersX.Count - 1);
            double ns = 1 / Params.confidenceValue + 1;
            return Math.Min( nspp, ns);
        }

        public double Getbm()
        {
            double q = parent.ClustersX.Count;
            double nspp = 2 * (this.items.Count - q) / q;
            double ns = 1 / Params.confidenceValue + 1;
            return Math.Min(Math.Max(nspp, 0), ns);
        }

        public double Getbg()
        {
            double q = parent.ClustersX.Count;
            double totalCount = parent.ClustersX.Select(cl => cl.items.Count).Sum();

            double result = 2 * (totalCount - q) / Math.Pow(q, 2);
            return result;
        }

        public void ILArrayToFile(ILArray<double> ilarray, int dimensionX, int dimensionY)
        {
            double[,] array = this.GetTwoDimensionalArray(ilarray, dimensionX, dimensionY);

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            for (int i = 0; i < dimensionX; i++)
            {
                sb.Append("{");

                for (int j = 0; j < dimensionY; j++)
                {
                    sb.Append(array[i, j].ToString());

                    if (j < dimensionY - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.Append("}");

                if (i < dimensionX - 1)
                {
                    sb.Append(",");
                }

                sb.Append("\r\n");
            }

            sb.Append("}");

            File.WriteAllText(@"D:/TestMatrix.txt", sb.ToString());
        }

        public void SaveILArrayToCsvFile(ILArray<double> ilarray, int dimensionX, int dimensionY, string path)
        {
            double[,] array = this.GetTwoDimensionalArray(ilarray, dimensionX, dimensionY);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dimensionX; i++)
            {
                for (int j = 0; j < dimensionY; j++)
                {
                    sb.Append(array[i, j].ToString());

                    if (j < dimensionY - 1)
                    {
                        sb.Append(",");
                    }
                }

                if (i < dimensionX - 1)
                {
                    sb.Append(",");
                }

                sb.Append("\r\n");
            }

            File.WriteAllText(path, sb.ToString());
        }

        private double[,] GetTwoDimensionalArray(ILArray<double> array, int dimensionX, int dimensionY)
        {
            double[,] result = new double[dimensionX, dimensionY];
            double[] source = array.ToArray();

            for (int i = 0; i < dimensionX; i++)
            {
                for (int j = 0; j < dimensionY; j++)
                {
                    result[i, j] = source[i * dimensionX + j];
                }
            }

            return result;
        }

        #endregion
    }
}
