using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHDRLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IHDRLibTest
{
    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        public void GetXDistanceFromSample_GiveCorrectDistance()
        {
            Sample sample1 = new Sample(new double[] { 0, 3, 4, 5}, 1, 0);
            Sample sample2 = new Sample(new double[] { 7, 6, 3, -1}, 1, 0);

            double distance = Math.Round(sample1.GetXDistanceFromSample(sample2), 3);
            Assert.AreEqual(distance, 9.747);
        }

        
    }
}
