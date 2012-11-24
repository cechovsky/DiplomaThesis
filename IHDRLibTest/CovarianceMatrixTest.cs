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
            CovarianceMatrix matrix = new CovarianceMatrix(new Vector(new double[] { 4.0, 2.0, 0.6}), 3);
            matrix.UpdateMatrix(new Vector(new double[] { 4.2, 2.1, 0.59 }), 2);
            matrix.UpdateMatrix(new Vector(new double[] { 3.9, 2.0, 0.58 }), 3);
            matrix.UpdateMatrix(new Vector(new double[] { 4.3, 2.1, 0.62 }), 4);
            matrix.UpdateMatrix(new Vector(new double[] { 4.1, 2.2, 0.63 }), 5);


        }
    }
}
