using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHDRLib;
using ILNumerics;

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

            Assert.AreEqual(vector.Values[0], 3.0);
            Assert.AreEqual(vector.Values[1], 5.0);
            Assert.AreEqual(vector.Values[2], 7.0);
        }

        [TestMethod]
        public void Substract_RestulVectorIsCorrect()
        {
            Vector vector = new Vector(new double[] { 1.0, 2.0, 3.0 });
            Vector vector2 = new Vector(new double[] { 2.0, 4.0, 6.0 });

            vector.Subtract(vector2);

            Assert.AreEqual(vector.Values[0], -1.0);
            Assert.AreEqual(vector.Values[1], -2.0);
            Assert.AreEqual(vector.Values[2], -3.0);
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
        public void GetMDFDistance_GetCorrectDistance()
        {
            Vector v = new Vector(new double[] { 1, 3, 4 });
            v.ValuesMDF = new double[] { 1, 3, 4 };
            ILArray<double> a2 = new double[] { 7, 2, 3 };

            double distance = Math.Round(v.GetMDFDistance(a2), 3);

            Assert.AreEqual(distance, 6.164);
        }

        [TestMethod]
        public void GetMeanOfVectors_GetCorrectMean()
        {
            List<Vector> vectors = new List<Vector>();
            vectors.Add(new Vector(new double[]{ 1.0, 2.0, 3.0}, 1.0, 0));
            vectors.Add(new Vector(new double[] { 2.0, 3.0, 4.0 }, 1.0, 0));

            Vector result = Vector.GetMeanOfVectors(vectors);

            Assert.AreEqual(result.Values[0], 1.5);
            Assert.AreEqual(result.Values[1], 2.5);
            Assert.AreEqual(result.Values[2], 3.5);

        }

        [TestMethod]
        public void GetIdOfClosestVector_GetCorrectId()
        {
            List<Vector> vectors = new List<Vector>();
            vectors.Add(new Vector(new double[] { 1.0, 2.0, 3.0 }, 1.0, 0));
            vectors.Add(new Vector(new double[] { 2.0, 3.0, 4.0 }, 1.0, 1));
            vectors.Add(new Vector(new double[] { 2.0, 2.0, 2.0 }, 1.0, 2));
            vectors.Add(new Vector(new double[] { 7.0, 8.0, 6.0 }, 1.0, 3));

            Vector vector = new Vector(new double[] { 2.0, 3.0, 5.0 });

            int result = vector.GetIdOfClosestVector(vectors);

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void GetNormalisationNum_GetCorrectNum()
        {
            ILArray<double> vector = new double[] { 1, 1, 1, 1 };
            double normNum = Vector.GetNormalisationNum(vector);

            Assert.AreEqual(normNum, 2.0);
        }

        [TestMethod]
        public void GetVarianceOfVectors()
        {
            Node node = new Node(0.0, 0.0, string.Empty);            
            
            List<Vector> vectors = new List<Vector>();
            vectors.Add(new Vector(new double[] { 9, 2, 7 }, new double[] { 9, 2, 7 }));
            vectors.Add(new Vector(new double[] { 3, 3, 10 }, new double[] { 3, 3, 10 }));
            vectors.Add(new Vector(new double[] { 6, 4, 25 }, new double[] { 6, 4, 25 }));

            node.CountMeanMDF(vectors);
            node.CountVarianceMDF(vectors);

            Assert.AreEqual(node.VarianceMDF.ToArray()[0], 9);
            Assert.AreEqual(node.VarianceMDF.ToArray()[1], 1);
            Assert.AreEqual(node.VarianceMDF.ToArray()[2], 93);
        }
    }
}
