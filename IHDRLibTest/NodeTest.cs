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
    public class NodeTest
    {
        [TestMethod]
        public void GetNearestClusterPairX_GetCorrectClusterPair()
        {
            Node node = new Node();
            Sample s1 = new Sample(new double[] { 1, 2, 3 }, 1);
            Sample s2 = new Sample(new double[] { 1, 2, 3 }, 1);
            Sample s3 = new Sample(new double[] { 1, 2, 3 }, 1);

            node.ClustersX.Add(new ClusterX(s1));

        }
    }
}
