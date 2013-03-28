using IHDRLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHDRLibTest
{
    [TestClass]
    public class ClusterXTest
    {
        [TestMethod]
        public void ClusterXCreate_CreateCorrectClusterX()
        {
            // set params 
            Params.inputDataDimension = 3;
            Sample sample = new Sample(new double[] { 1.0, 2.0, 3.0 }, 1.0, 0);
            ClusterX clusterX = new ClusterX(sample, null);
            Assert.IsTrue(clusterX.Mean.EqualsToVector(new Vector(new double[] { 1.0, 2.0, 3.0 }))); 
        }

        [TestMethod]
        public void UpdateMean_UpdatingIsCorrect()
        {
            Params.inputDataDimension = 3;
            Params.t1 = 10;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 1.0, 2.0, 3.0 }, 1.0, 0), null);

            clusterX.AddItem(new Vector(new double[] { 2.0, 3.0, 4.0 }), 0);

            Assert.AreEqual(clusterX.Mean.Values[0], 1.5);
            Assert.AreEqual(clusterX.Mean.Values[1], 2.5);
            Assert.AreEqual(clusterX.Mean.Values[2], 3.5);

            clusterX.AddItem(new Vector(new double[] { 3.0, 4.0, 5.0 }), 0);

            Assert.AreEqual(clusterX.Mean.Values[0], 2.0);
            Assert.AreEqual(clusterX.Mean.Values[1], 3.0);
        }

        [TestMethod]
        public void CountCovarianceMatrix_CovarianceMatrixIsCorrect()
        {
#warning not complet test, update of covariance matrix must be reimplemented

            Params.inputDataDimension = 3;
            Params.outputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 4.0, 2.0, 0.6 }, 1.0, 0), null);
            clusterX.AddItem(new Vector(new double[] { 4.2, 2.1, 0.59 }), 0);

            //clusterX.CountCovariacneMatrix();

            clusterX.AddItem(new Vector(new double[] { 3.9, 2.0, 0.58 }), 0);
            clusterX.AddItem(new Vector(new double[] { 4.3, 2.1, 0.62 }), 0);

            //clusterX.CountCovariacneMatrix();

            clusterX.AddItem(new Vector(new double[] { 4.1, 2.2, 0.63 }), 0);

            //clusterX.CountCovariacneMatrix();
        }

        [TestMethod]
        public void CountMDFMean_CountCorrectMean()
        {
            Params.inputDataDimension = 3;
            Params.outputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 1, 2, 3 }, 1.0, 0), null);
            clusterX.AddItem(new Vector(new double[] { 2, 3, 4 }), 0);
            clusterX.AddItem(new Vector(new double[] { 3, 4, 5 }), 0);

            clusterX.Items[0].ValuesMDF = new double[] { 1, 2, 3 };
            clusterX.Items[1].ValuesMDF = new double[] { 2, 3, 4 };
            clusterX.Items[2].ValuesMDF = new double[] { 3, 4, 5 };

            clusterX.CountMDFMean();

            Assert.AreEqual(clusterX.MeanMDF[0], 2.0);
            Assert.AreEqual(clusterX.MeanMDF[1], 3.0);
            Assert.AreEqual(clusterX.MeanMDF[2], 4.0);
        }

        [TestMethod]
        public void CountCovarianceMatrixMDF_CountCorrectCM()
        {
            Params.inputDataDimension = 3;
            Params.outputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 4.0, 2.0, 0.6 }, 1.0, 0), null);
            clusterX.AddItem(new Vector(new double[] { 4.2, 2.1, 0.59 }), 0);
            clusterX.AddItem(new Vector(new double[] { 3.9, 2.0, 0.58 }), 0);
            clusterX.AddItem(new Vector(new double[] { 4.3, 2.1, 0.62 }), 0);
            clusterX.AddItem(new Vector(new double[] { 4.1, 2.2, 0.63 }), 0);
            

            clusterX.Items[0].ValuesMDF = new double[] { 4.0, 2.0, 0.6 };
            clusterX.Items[1].ValuesMDF = new double[] { 4.2, 2.1, 0.59 };
            clusterX.Items[2].ValuesMDF = new double[] { 3.9, 2.0, 0.58 };
            clusterX.Items[3].ValuesMDF = new double[] { 4.3, 2.1, 0.62 };
            clusterX.Items[4].ValuesMDF = new double[] { 4.1, 2.2, 0.63 };

            clusterX.CountCovarianceMatrixMDF();
        }

        [TestMethod]
        public void GetGaussianNLL_GetCorrectGausianNLL()
        {
            Params.inputDataDimension = 3;
            Params.outputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 1, 2, 3 }, 1.0, 0), null);
            clusterX.AddItem(new Vector(new double[] { 2, 3, 4 }), 0);
            clusterX.AddItem(new Vector(new double[] { 3, 4, 5 }), 0);

            clusterX.Items[0].ValuesMDF = new double[] { 1, 2, 3 };
            clusterX.Items[1].ValuesMDF = new double[] { 2, 3, 4 };
            clusterX.Items[2].ValuesMDF = new double[] { 3, 4, 5 };

            clusterX.CountMDFMean();

            clusterX.CovMatrixMDF = new double[3, 3] { { 1, 2, 3 }, { 2, 1, 2 }, { 3, 3, 1 } };

            // clusterX.GetGaussianNLL(new double[] { 1, 2, 3 });
        }

        [TestMethod]
        public void CountLabelOfCluster_LabelIsCorrect()
        {
            Params.inputDataDimension = 3;
            Params.outputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 1, 2, 3 }, 1.0, 0), null);
            clusterX.AddItem(new Vector(new double[] { 2, 3, 4 }), 2.0);
            clusterX.AddItem(new Vector(new double[] { 2, 3, 4 }), 3.0);
            clusterX.AddItem(new Vector(new double[] { 2, 3, 4 }), 2.0);
            clusterX.AddItem(new Vector(new double[] { 2, 3, 4 }), 1.0);
            clusterX.AddItem(new Vector(new double[] { 2, 3, 4 }), 2.0);

            clusterX.CountLabelOfCluter();

            Assert.AreEqual(clusterX.Label, 2.0);     
        }
    }
}
