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
    public class CovarianceMatrixTest
    {
        [TestMethod]
        public void UpdateMatrix_CovarianceMatrixIsCorrect()
        {
            Params.inputDataDimension = 3;
            Params.outputDataDimension = 3;

            ClusterX clusterX = new ClusterX(new Sample(new double[] { 4.0, 2.0, 0.6 }, 1.0));
            clusterX.AddItem(new Vector(new double[] { 4.2, 2.1, 0.59 }));
            clusterX.AddItem(new Vector(new double[] { 3.9, 2.0, 0.58 }));
            clusterX.AddItem(new Vector(new double[] { 4.3, 2.1, 0.62 }));
            clusterX.AddItem(new Vector(new double[] { 4.1, 2.2, 0.63 }));
        }
    }
}
