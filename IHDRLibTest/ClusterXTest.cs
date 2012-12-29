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
            
            ClusterX clusterX = new ClusterX(sample);

            Assert.IsTrue(clusterX.Mean.EqualsToVector(new Vector(new double[] { 1.0, 2.0, 3.0 }))); 
        }

        [TestMethod]
        public void UpdateMean_UpdatingIsCorrect()
        {
            Params.inputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 1.0, 2.0, 3.0 }, 1.0, 0));

            clusterX.AddItem(new Vector(new double[] { 2.0, 3.0, 4.0 }));

            Assert.AreEqual(clusterX.Mean[0], 1.5);
            Assert.AreEqual(clusterX.Mean[1], 2.5);
            Assert.AreEqual(clusterX.Mean[2], 3.5);

            clusterX.AddItem(new Vector(new double[] { 3.0, 4.0, 5.0 }));

            Assert.AreEqual(clusterX.Mean[0], 2.0);
            Assert.AreEqual(clusterX.Mean[1], 3.0);
            Assert.AreEqual(clusterX.Mean[2], 4.0);
        }

        [TestMethod]
        public void CountCovarianceMatrix_CovarianceMatrixIsCorrect()
        {
#warning not complet test, update of covariance matrix must be reimplemented
            Params.inputDataDimension = 3;
            Params.outputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 4.0, 2.0, 0.6 }, 1.0, 0));
            clusterX.AddItem(new Vector(new double[] { 4.2, 2.1, 0.59 }));

            //clusterX.CountCovariacneMatrix();

            clusterX.AddItem(new Vector(new double[] { 3.9, 2.0, 0.58 }));
            clusterX.AddItem(new Vector(new double[] { 4.3, 2.1, 0.62 }));

            clusterX.CountCovariacneMatrix();

            clusterX.AddItem(new Vector(new double[] { 4.1, 2.2, 0.63 }));

            clusterX.CountCovariacneMatrix();

        }
    }
}
