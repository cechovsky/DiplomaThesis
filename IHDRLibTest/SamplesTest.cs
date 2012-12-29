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
    public class SamplesTest
    {
        [TestMethod]
        public void GetLabels_FunctionReturnCorrectLabels()
        {
            Samples samples = new Samples();
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 1, 0));
            samples.Add(new Sample(new double[] { 2, 3, 4 }, 1, 0));
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 2, 0));
            samples.Add(new Sample(new double[] { 2, 3, 4 }, 2, 0));
            samples.Add(new Sample(new double[] { 5, 6, 7 }, 2, 0));
            samples.Add(new Sample(new double[] { 5, 6, 7 }, 3, 0));

            List<double> result = samples.GetLabels();

            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0], 1);
            Assert.AreEqual(result[1], 2);
            Assert.AreEqual(result[2], 3);
        }

        [TestMethod]
        public void GetSamplesOfLabel_ReturnSamplesWithSpecificLabel()
        {
            Samples samples = new Samples();
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 1, 0));
            samples.Add(new Sample(new double[] { 2, 3, 4 }, 1, 0));
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 2, 0));
            samples.Add(new Sample(new double[] { 2, 3, 4 }, 2, 0));
            samples.Add(new Sample(new double[] { 5, 6, 7 }, 2, 0));
            samples.Add(new Sample(new double[] { 5, 6, 7 }, 3, 0));

            List<Sample> result = samples.GetSamplesOfLabel(1.0);

            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].Label, 1);
            Assert.AreEqual(result[1].Label, 1);

        }

        [TestMethod]
        public void GetMeanOfDataWithLabel_ReturnCorrectMean()
        {
            Params.inputDataDimension = 3;

            Samples samples = new Samples();
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 1, 0));
            samples.Add(new Sample(new double[] { 2, 3, 4 }, 1, 0));
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 2, 0));
            samples.Add(new Sample(new double[] { 2, 3, 4 }, 2, 0));
            samples.Add(new Sample(new double[] { 5, 6, 7 }, 2, 0));
            samples.Add(new Sample(new double[] { 5, 6, 7 }, 3, 0));

            Vector result1 = samples.GetMeanOfDataWithLabel(1.0);

            Vector result2 = new Vector(new double[] { 1.5, 2.5, 3.5 });

            Assert.AreEqual(result1[0], result1[0]);
            Assert.AreEqual(result1[1], result1[1]);
            Assert.AreEqual(result1[2], result1[2]);
        }

        [TestMethod]
        public void CountOutputsFromClassLabels_CountCorrectOutputs()
        {
            Samples samples = new Samples();
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 1, 0));
            samples.Add(new Sample(new double[] { 2, 3, 4 }, 1, 0));
            samples.Add(new Sample(new double[] { 1, 2, 3 }, 2, 0));
            samples.Add(new Sample(new double[] { 3, 4, 5 }, 2, 0));
            samples.Add(new Sample(new double[] { 5, 6, 7 }, 2, 0));
            samples.Add(new Sample(new double[] { 6, 3, 7 }, 3, 0));

            samples.CountOutputsFromClassLabels();
            Assert.IsTrue(samples[0].Y.EqualsToVector(new Vector(new double[] { 1.5, 2.5, 3.5 })));
            Assert.IsTrue(samples[1].Y.EqualsToVector(new Vector(new double[] { 1.5, 2.5, 3.5 })));
            Assert.IsTrue(samples[2].Y.EqualsToVector(new Vector(new double[] { 3.0, 4.0, 5.0 })));
            Assert.IsTrue(samples[3].Y.EqualsToVector(new Vector(new double[] { 3.0, 4.0, 5.0 })));
            Assert.IsTrue(samples[4].Y.EqualsToVector(new Vector(new double[] { 3.0, 4.0, 5.0 })));
            Assert.IsTrue(samples[5].Y.EqualsToVector(new Vector(new double[] { 6.0, 3.0, 7.0 })));
        }
    }
}
