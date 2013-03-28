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
            Node node = new Node(0.0,0.0);
            Sample s1 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s2 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s3 = new Sample(new double[] { 1, 2, 3 }, 1, 0);

            node.ClustersX.Add(new ClusterX(s1, null));
        }

        [TestMethod]
        public void GetCFromClustersX_GetCorrectC()
        {
            Node node = new Node(0.0, 0.0);
            Sample s1 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s2 = new Sample(new double[] { 2, 3, 4 }, 1, 0);
            Sample s3 = new Sample(new double[] { 3, 4, 5 }, 1, 0);

            node.ClustersX.Add(new ClusterX(s1, null));
            node.ClustersX.Add(new ClusterX(s2, null));
            node.ClustersX.Add(new ClusterX(s3, null));

            Vector C = node.GetCFromClustersX();

            Assert.AreEqual(C.Values[0], 2.0);
            Assert.AreEqual(C.Values[1], 3.0);
            Assert.AreEqual(C.Values[2], 4.0);
        }

        [TestMethod]
        public void GetScatterVectors_GetCorrectVectors()
        {
            Node node = new Node(0.0, 0.0);
            Sample s1 = new Sample(new double[] { 1, 2, 3 }, 1, 0);
            Sample s2 = new Sample(new double[] { 2, 3, 4 }, 1, 0);
            Sample s3 = new Sample(new double[] { 3, 4, 5 }, 1, 0);

            node.ClustersX.Add(new ClusterX(s1, null));
            node.ClustersX.Add(new ClusterX(s2, null));
            node.ClustersX.Add(new ClusterX(s3, null));

            List<Vector> scatterVectors = node.GetScatterVectors();

            //Assert.AreEqual(scatterVectors[0].Values[0], -1);
            //Assert.AreEqual(scatterVectors[0].Values[1], -1);
            //Assert.AreEqual(scatterVectors[0].Values[2], -1);

            Assert.AreEqual(scatterVectors[0].Values[0], 0);
            Assert.AreEqual(scatterVectors[0].Values[1], 0);
            Assert.AreEqual(scatterVectors[0].Values[2], 0);

            Assert.AreEqual(scatterVectors[1].Values[0], 1);
            Assert.AreEqual(scatterVectors[1].Values[1], 1);
            Assert.AreEqual(scatterVectors[1].Values[2], 1);

        }

        [TestMethod]
        public void CountGSOManifold_ProvideCorrectCounting()
        {
            Params.inputDataDimension = 4;

            Node node = new Node(0.0, 0.0);
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

        [TestMethod]
        public void CountC_CountCCorrect()
        {
            Node node = new Node(0, 0);
            Params.inputDataDimension = 3;
            node.CountOfSamples = 6;

            // cluster 1
            ClusterX newClusterX1 = new ClusterX(node);
            newClusterX1.Items.Add(new Vector(0, 0));
            newClusterX1.Items.Add(new Vector(0, 0));
            newClusterX1.Items.Add(new Vector(0, 0));
            newClusterX1.Mean = new Vector(new double[] { 1, 2, 3 });
            node.ClustersX.Add(newClusterX1);

            ClusterPair clusterPair1 = new ClusterPair();
            clusterPair1.X = newClusterX1;

            newClusterX1.SetClusterPair(clusterPair1);

            node.ClusterPairs.Add(clusterPair1);

            // cluster 2
            ClusterX newClusterX2 = new ClusterX(node);
            newClusterX2.Items.Add(new Vector(0, 0));
            newClusterX2.Items.Add(new Vector(0, 0));
            newClusterX2.Mean = new Vector(new double[] { 3, 3, 4 });
            node.ClustersX.Add(newClusterX2);

            ClusterPair clusterPair2 = new ClusterPair();
            clusterPair2.X = newClusterX2;

            newClusterX2.SetClusterPair(clusterPair2);

            node.ClusterPairs.Add(clusterPair2);

            // cluster 3
            ClusterX newClusterX3 = new ClusterX(node);
            newClusterX3.Items.Add(new Vector(0, 0));
            newClusterX3.Mean = new Vector(new double[] { 9, 6, 7 });
            node.ClustersX.Add(newClusterX3);

            ClusterPair clusterPair3 = new ClusterPair();
            clusterPair3.X = newClusterX3;

            newClusterX3.SetClusterPair(clusterPair3);

            node.ClusterPairs.Add(clusterPair3);

            Vector mean = node.GetCFromClustersX();

            Assert.AreEqual(mean.Values[0], 3);
            Assert.AreEqual(mean.Values[1], 3);
            Assert.AreEqual(mean.Values[2], 4);
            
        }
    }
}
