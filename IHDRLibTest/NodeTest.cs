using IHDRLib;
using ILNumerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHDRLibTest
{
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void GetNearestClusterPairX_GetCorrectClusterPair()
        {
            Node node = new Node();
            Sample s1 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s2 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s3 = new Sample(new double[] { 1, 2, 3 }, 1, 0);

            node.ClustersX.Add(new ClusterX(s1, null));
        }

        [TestMethod]
        public void GetCFromClustersX_GetCorrectC()
        {
            Node node = new Node();
            Sample s1 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s2 = new Sample(new double[] { 2, 3, 4 }, 1, 0);
            Sample s3 = new Sample(new double[] { 3, 4, 5 }, 1, 0);

            node.ClustersX.Add(new ClusterX(s1, null));
            node.ClustersX.Add(new ClusterX(s2, null));
            node.ClustersX.Add(new ClusterX(s3, null));

            Vector C = node.GetCFromClustersX();

            Assert.AreEqual(C[0], 2.0);
            Assert.AreEqual(C[1], 3.0);
            Assert.AreEqual(C[2], 4.0);
        }

        [TestMethod]
        public void GetScatterVectors_GetCorrectVectors()
        {
            Node node = new Node();
            Sample s1 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s2 = new Sample(new double[] { 2, 3, 4 }, 1, 0);
            Sample s3 = new Sample(new double[] { 3, 4, 5 }, 1, 0);

            node.ClustersX.Add(new ClusterX(s1, null));
            node.ClustersX.Add(new ClusterX(s2, null));
            node.ClustersX.Add(new ClusterX(s3, null));

            List<Vector> scatterVectors = node.GetScatterVectors();

            Assert.AreEqual(scatterVectors[0][0], -1);
            Assert.AreEqual(scatterVectors[0][1], -1);
            Assert.AreEqual(scatterVectors[0][2], -1);

            Assert.AreEqual(scatterVectors[1][0], 0);
            Assert.AreEqual(scatterVectors[1][1], 0);
            Assert.AreEqual(scatterVectors[1][2], 0);

            Assert.AreEqual(scatterVectors[2][0], 1);
            Assert.AreEqual(scatterVectors[2][1], 1);
            Assert.AreEqual(scatterVectors[2][2], 1);

        }

        [TestMethod]
        public void CountGSOManifold_ProvideCorrectCounting()
        {
            Params.inputDataDimension = 4;

            Node node = new Node();
            Sample s1 = new Sample(new double[] { 1, 0, 2, 1 }, 1, 0);
            Sample s2 = new Sample(new double[] { -1, 1, 0, -1 }, 1, 0);
            Sample s3 = new Sample(new double[] { 2, 1, 1, 1 }, 1, 0);

            node.ClustersX.Add(new ClusterX(s1, null));
            node.ClustersX.Add(new ClusterX(s2, null));
            node.ClustersX.Add(new ClusterX(s3, null));

            List<Vector> scatterVectors = new List<Vector>();
            scatterVectors.Add(new Vector(new double[] { 1, 0, 2, 1 }));
            scatterVectors.Add(new Vector(new double[] { -1, 1, 0, -1 }));
            scatterVectors.Add(new Vector(new double[] { 2, 1, 1, 1 }));

            ILArray<double> array = node.GetManifold(scatterVectors);
        }
    }
}
