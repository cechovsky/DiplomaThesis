using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHDRLib;

namespace IHDRLibTest
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void Add_AddToVectorCorrectVector()
        {
            Vector vector = new Vector(new double[] { 1.0, 2.0, 3.0});
            Vector vector2 = new Vector(new double[] { 2.0, 3.0, 4.0 });

            vector.Add(vector2);

            Assert.AreEqual(vector[0], 3.0);
            Assert.AreEqual(vector[1], 5.0);
            Assert.AreEqual(vector[2], 7.0);
        }

        [TestMethod]
        public void Substract_RestulVectorIsCorrect()
        {
            Vector vector = new Vector(new double[] { 1.0, 2.0, 3.0 });
            Vector vector2 = new Vector(new double[] { 2.0, 4.0, 6.0 });

            vector.Subtract(vector2);

            Assert.AreEqual(vector[0], -1.0);
            Assert.AreEqual(vector[1], -2.0);
            Assert.AreEqual(vector[2], -3.0);
        }

        [TestMethod]
        public void Equals_VectorEqualsToVector()
        {
            Vector vector1 = new Vector(new double[] { 1.0, 2.0, 0, 3.0 });
            Vector vector2 = new Vector(new double[] { 1.0, 2.0, 0, 3.0 });

            bool equals = vector1.EqualsToVector(vector2);

            Assert.IsTrue(equals);
        }

        [TestMethod]
        public void Equals_VectorNotEqualsToVector()
        {
            Vector vector1 = new Vector(new double[] { 1.0, 2.0, 0, 3.0 });
            Vector vector2 = new Vector(new double[] { 1.0, 0.0, 0, 3.0 });

            bool equals = vector1.EqualsToVector(vector2);

            Assert.IsFalse(equals);
        }

        [TestMethod]
        public void GetDistance_GetCorrectDistance()
        {
            Vector v1 = new Vector(new double[] { 1, 3, 4 });
            Vector v2 = new Vector(new double[] { 7, 2, 3 });

            double distance = Math.Round(v1.GetDistance(v2), 3);

            Assert.AreEqual(distance, 6.164);

        }

        [TestMethod]
        public void GetMeanOfVectors_GetCorrectMean()
        {
            List<Vector> vectors = new List<Vector>();
            vectors.Add(new Vector(new double[]{ 1.0, 2.0, 3.0}, 1.0));
            vectors.Add(new Vector(new double[] { 2.0, 3.0, 4.0 }, 1.0));

            Vector result = Vector.GetMeanOfVectors(vectors);

            Assert.AreEqual(result[0], 1.5);
            Assert.AreEqual(result[1], 2.5);
            Assert.AreEqual(result[2], 3.5);

        }
    }
}
