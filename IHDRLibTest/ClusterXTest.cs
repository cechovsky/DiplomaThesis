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

            Sample sample = new Sample(new double[] { 1.0, 2.0, 3.0 }, 1.0);
            
            ClusterX clusterX = new ClusterX(sample);

            Assert.IsTrue(clusterX.Mean.EqualsToVector(new Vector(new double[] { 1.0, 2.0, 3.0 }))); 
        }
    }
}
