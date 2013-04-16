using ILNumerics;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IHDRLib
{
    [Serializable]
    public class ClusterX : Cluster
    {
        private Node child;
        protected ILArray<double> covarianceMatrixMDF;
        private ILArray<double> varianceMDF;

        private double label;

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("child", child, typeof(Node));
            info.AddValue("covarianceMatrixMDF", covarianceMatrixMDF, typeof(ILArray<double>));
            info.AddValue("label", label, typeof(double));
        }

        public ClusterX(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            child = (Node)info.GetValue("child", typeof(Node));
            covarianceMatrixMDF = (ILArray<double>)info.GetValue("covarianceMatrixMDF", typeof(ILArray<double>));
            label = (double)info.GetValue("label", typeof(double));
        }

        public ClusterX(Node parent)
            : base(parent)
        {
            this.child = null;
        }

        public ClusterX(Sample sample, Node parent)
            : base(sample, parent)
        {
            this.child = null;
            this.dimension = Params.inputDataDimension;

            items.Add(new Vector(sample.X.Values.ToArray(), sample.Label, 1));

            this.mean = new Vector(sample.X.Values.ToArray());

            //#warning TODO count covariance matrix 
            //this.covarianceMatrix = ILMath.zeros(Params.inputDataDimension, Params.inputDataDimension);
        }

        public void SetClusterPair(ClusterPair clusterPair)
        {
            this.clusterPair = clusterPair;
        }

        public void SaveSamples()
        {
            if (!Params.StoreItems)
            {
                throw new InvalidOperationException("Unable to save cluster, because items were disposed.");
            }

            foreach (var item in items)
            {
                item.SaveToBitmap(this.SavePath, false, Params.inputBmpWidth, Params.inputBmpHeight);
            }

            //if (Params.SaveMeansMDF)
            //{
            //    if (this.meanMDF != null)
            //    {
            //        this.SaveILArrayToCsvFile(this.meanMDF, 1, this.meanMDF.Length - 1, this.SavePath + @"\MeanMdf.txt");
            //    }
            //}
            //if (Params.SaveCovMatricesMDF)
            //{
            //    if (this.covarianceMatrixMDF != null && this.meanMDF != null)
            //    {
            //       this.SaveILArrayToCsvFile(this.covarianceMatrixMDF, this.meanMDF.Length - 1, this.meanMDF.Length - 1, this.SavePath + @"\CovMatrixMdf.txt");
            //    }
            //}

            //this.SaveClusterInfo(this.SavePath + @"\ClusterInfo.txt");

            this.mean.SaveToBitmap(this.SavePath, true, Params.inputBmpWidth, Params.inputBmpHeight);
        }

        private void SaveClusterInfo(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Label of cluster: " + this.Label);
            File.WriteAllText(path, sb.ToString());
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
            foreach (var vector in this.items)
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

        public void UpdateCovarianceMatrixMDF_NonParametric(ILArray<double> vector)
        {

#warning this must be remake according to F. Amnesic average with parameters t1, t2

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

        public void UpdateCovarianceMatrixMDF(ILArray<double> vector)
        {

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

            double t = (double)this.items.Count;
            //double fragment1 = (t - 2) / (t - 1);
            //double fragment2 = t / (Math.Pow((t - 1), 2));

            double fragment1 = (t - 1 - this.GetAmnesicParameter(t)) / t;
            double fragment2 = (1 + this.GetAmnesicParameter(t)) / t;

            try
            {
                this.covarianceMatrixMDF = (this.covarianceMatrixMDF * fragment1) + (fragment2 * ILMath.multiply(vector1, vector2));
            }
            catch (Exception ee)
            {
                throw new InvalidCastException();
            }

        }

        //public void CountCovariacneMatrix()
        //{
        //    this.mean = Vector.GetMeanOfVectors(this.items);
        //    this.covarianceMatrix = ILMath.zeros(Params.inputDataDimension, Params.inputDataDimension);

        //    if (items.Count > 1)
        //    {
        //        for (int i = 0; i < dimension; i++)
        //        {
        //            for (int j = 0; j < dimension; j++)
        //            {
        //                double sum = 0.0;
        //                foreach (Vector item in items)
        //                {
        //                    sum += (item[i] - this.mean[i]) * (item[j] - this.mean[j]);
        //                }
                    
        //                this.covarianceMatrix[i, j] = sum / (items.Count - 1);
        //            }
        //        }
        //   }
        //}

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
                                sum += ((double)item.ValuesMDF[i] - (double)this.meanMDF[i]) * ((double)item.ValuesMDF[j] - (double)this.meanMDF[j]);
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

        //public void DisposeCovMatrix()
        //{
        //    this.covarianceMatrix.Dispose();
        //}

        public void AddItem(Vector vector, double label)
        {
            Vector newItem = new Vector(vector.Values.ToArray());
            newItem.Label = label;
            newItem.Id = this.items.Count + 1;
            this.items.Add(newItem);
            this.itemsCount++;

            // update mean
            this.UpdateMean(newItem);
            // update covariance matrix
            //this.UpdateCovarianceMatrix(vector);
        }

        public void AddItemNonLeaf(Vector newItem)
        {
            newItem.Id = this.items.Count + 1;

            this.items.Add(newItem);
            this.itemsCount++;
            // update mean
            this.UpdateMean(newItem);

            //node.CountGSOManifold();

            this.UpdateMeanAndCovMatrixMDF(newItem.ValuesMDF.ToArray());   
        }

        public void UpdateMeanAndCovMatrixMDF(ILArray<double> vector)
        {
            // update mean
            this.UpdateMeanMDF(vector);
            // update covariance matrix
            this.UpdateCovarianceMatrixMDF(vector);
        }

        //public ILArray<double> CovMatrix
        //{
        //    get
        //    {
        //        return this.covarianceMatrix;
        //    }
        //    set
        //    {
        //        this.covarianceMatrix = value;
        //    }
        //}

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

        public double Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        #region Probability Based Metrics

        //public double GetSDNLL(ILArray<double> vector)
        //{
        //    double firstPart = 0;
        //    double secondPart = 0;
        //    double thirdPart = 0;

        //    if (this.items.Count == 1)
        //    {
        //        this.covarianceMatrix = this.GetVarianceMatrix();
        //    }

        //    ILArray<double> meanIL = this.mean.ToArray();

        //    ILArray<double> W = this.GetMatrixW();

        //    ILArray<double> WInverse = 1;
        //    if (Params.ContainsSingularCovarianceMatrixes)
        //    {
        //        //WInverse = this.GetPseudoInverseMatrixOfMatrix(W);
        //        WInverse = this.GetInverseMatrixOfMatrix(W, Params.inputDataDimension);
        //    }
        //    else
        //    {
        //        WInverse = this.GetInverseMatrixOfMatrix(W, Params.inputDataDimension);
        //    }

        //    ILArray<double> vector1 = meanIL - vector;
        //    ILArray<double> tmpArray = ILMath.multiply(WInverse, vector1);

        //    firstPart = 0.5 * ILMath.multiply(vector1.T, tmpArray).ToArray()[0];
        //    secondPart = Params.inputDataDimension / 2 * ILMath.log(2 * Math.PI).ToArray()[0];
        //    thirdPart = 0.5 * ILMath.log(ILMath.det(W)).ToArray()[0];

        //    return firstPart + secondPart + thirdPart;
        //}

        public double GetSDNLL_MDF(ILArray<double> mdfVector)
        {
            double firstPart = 0;
            double secondPart = 0;
            double thirdPart = 0;

            if (this.itemsCount == 1)
            {
                this.covarianceMatrixMDF = this.GetVarianceMatrix_MDF();
            }

            int q = this.meanMDF.Length;

            // get matrix W
            ILArray<double> W = this.GetMatrixW_MDF();
            ILArray<double> WInverse = this.GetInverseMatrixOfMatrix(W, q);

            if (Double.IsNaN(WInverse.ToArray()[0])) throw new InvalidDataException("inverse of covariance matrix MDF is NaN");

            ILArray<double> vector1 = mdfVector - this.meanMDF;
            ILArray<double> tmpArray = ILMath.multiply(WInverse, vector1);

            firstPart = 0.5 * ILMath.multiply(vector1.T, tmpArray).ToArray()[0];
            secondPart = q / 2 * ILMath.log(2 * Math.PI).ToArray()[0];
            thirdPart = 0.5 * ILMath.log(ILMath.det(W)).ToArray()[0];

            return firstPart + secondPart + thirdPart;
        }

        public ILArray<double> GetMatrixW_MDF()
        {
            double be = this.Getbe(); 
            //double be = 0; 
            double bm = this.Getbm();
            //double bm = 0;
            double bg = this.Getbg();
            //double bg = 0; 
            double b = be + bm + bg;

            double we = be / b;
            double wm = bm / b;
            double wg = bg / b;

            //Console.WriteLine(string.Format("we: {0}", we));
            //Console.WriteLine(string.Format("wm: {0}", wm));
            //Console.WriteLine(string.Format("wg: {0}", wg));

            ILArray<double> gaussianPart = this.covarianceMatrixMDF;
            ILArray<double> mahalonobisPart = this.parent.CovarianceMatrixMeanMDF;
            ILArray<double> euclideanPart = this.GetVarianceMatrix_MDF();

            return (wg * gaussianPart) + (wm * mahalonobisPart) + (we * euclideanPart);
        }

        public ILArray<double> GetInverseMatrixOfMatrix(ILArray<double> inputMatrix, int dimension)
        {
            ILArray<double> identityMatrix = ILMath.eye(dimension, dimension);
            ILArray<double> inverseCovMatrix = ILMath.linsolve(inputMatrix, identityMatrix);
            return inverseCovMatrix;
        }
                
        public ILArray<double> GetVarianceMatrix()
        {
            ILArray<double> result = ILMath.eye(Params.inputDataDimension, Params.inputDataDimension);
            result = Params.digitizationNoise * result;

            return result;
        }

        public ILArray<double> GetVarianceMatrix_MDF()
        {
            double sum = this.parent.VarianceMDF.Sum() / this.parent.VarianceMDF.Length;
            ILArray<double> diagonale = ILMath.eye(this.parent.VarianceMDF.Length, this.parent.VarianceMDF.Length);
            ILArray<double> result = diagonale * sum;
            //ILArray<double> result = ILMath.diag<double>(this.parent.VarianceMDF.ToArray());

            return result;
        }

        public double Getbe()
        {
            double nspp = (this.parent.CountOfSamples - 1) * (parent.ClustersX.Count - 1);
            double ns = 1 / Params.confidenceValue + 1;
            return Math.Min( nspp, ns);
        }

        public double Getbm()
        {
            double q = parent.ClustersX.Count;
            double nspp = 2 * (this.parent.CountOfSamples - q) / q;
            double ns = 1 / Params.confidenceValue + 1;
            return Math.Min(Math.Max(nspp, 0), ns);
        }

        public double Getbg()
        {
            double q = parent.ClustersX.Count;

            double result = 2 * (this.parent.CountOfSamples - q) / Math.Pow(q, 2);
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

        public void CountLabelOfCluter()
        {
            if (Params.StoreItems)
            {
                throw new InvalidOperationException("Unable to count label because items are not stored.");
            }

            var labels = this.items.GroupBy(i => i.Label).Select(i => i.First().Label);

            int count = int.MinValue;
            double newLabel = -1;

            foreach (var item in labels)
            {
                int labelCount = this.items.Where(i => i.Label == item).Count();
                if (labelCount > count)
                {
                    count = labelCount;
                    newLabel = item;
                }
            }
            this.label = newLabel;
        }
    }
}
