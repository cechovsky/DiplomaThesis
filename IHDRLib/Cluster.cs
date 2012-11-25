﻿using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Cluster
    {
        protected ClusterPair clusterPair;
        protected DenseMatrix covarianceMatrix;
        protected List<Vector> items;
        protected Vector mean;
        protected int dimension;

        public Cluster()
        {
            
            this.clusterPair = null;
            this.covarianceMatrix = null;
            this.items = new List<Vector>();
            this.mean = null;
        }

        public Cluster(Sample sample)
        {
            this.items = new List<Vector>();
        }

        public void SetClusterPair(ClusterPair clusterPair)
        {
            this.clusterPair = clusterPair;
        }

        

        /// <summary>
        /// Update mean with amnesic average
        /// </summary>
        /// <param name="vector">Vector, which should be add to mean</param>
        protected void UpdateMean(Vector vector)
        {
            #warning this must be remade according to F. Amnesic average with parameters t1, t2
            
            double t = this.items.Count;
            this.mean.Multiply((t-1)/t);

            Vector incrementPart = new Vector(vector.ToArray());
            incrementPart.Multiply(1 / t);

            this.mean.Add(incrementPart);
        }

        public void UpdateCovarianceMatrix(Vector vector)
        {

#warning this must be remage according to F. Amnesic average with parameters t1, t2

            // newCov = t-1/t * cov(t-1) + 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // oldPart = t-1/t * cov(t-1)
            // incrementalPart = 1/t * (newVector - mean(t)) * (newVector - mean(t))T
            // vector1 = (newVector - mean(t))
            // vector2 = (newVector - mean(t))T
            // newCovPart = vector1 * vector2

            //newVector - mean(t)
            Vector v1 = new Vector(vector.ToArray());
            v1.Substract(this.mean);

            DenseMatrix vector1 = new DenseMatrix(this.dimension, 1);
            vector1.SetColumn(0, v1.ToArray());
            DenseMatrix vector2 = new DenseMatrix(1, this.dimension);
            vector2.SetRow(0, v1.ToArray());

            double t = (double) this.items.Count;
            double fragment1 = (t - 2) / t - 1;
            double fragment2 = t / Math.Pow(t-1, 2);


            DenseMatrix oldPart = fragment1 * this.covarianceMatrix ;
            DenseMatrix newCovPart = vector1 * vector2;

            DenseMatrix incrementalPart = fragment2 * newCovPart;

            this.covarianceMatrix = oldPart + incrementalPart;
        }
    }
}
