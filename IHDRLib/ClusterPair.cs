using ILNumerics;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class ClusterPair
    {
        private ClusterX clusterX;
        private ClusterY clusterY;

        public ClusterPair()
        {
            this.clusterX = new ClusterX();
            this.clusterX.SetClusterPair(this);
            this.clusterY = new ClusterY();
            this.clusterY.SetClusterPair(this);
        }

        public ClusterPair(ClusterX cX, ClusterY cY)
        {
            clusterX = cX;
            clusterY = cY;

            this.PreviousCenter = 0;
            this.CurrentCenter = 0;
        }

        public ClusterPair GetClone()
        {
            ClusterPair cp = new ClusterPair();

            foreach (var x in this.X.Items)
            {
                cp.X.AddItemWithoutUpdatingStats(x);
                cp.X.ClusterPair = cp;
            }
            foreach (var y in this.Y.Items)
            {
                cp.Y.AddItemWithoutUpdatingStats(y);
            }

            try
            {
                cp.X.CovMatrix = ILMath.array(this.X.CovMatrix.ToArray(), Params.inputDataDimension, Params.inputDataDimension);
                cp.X.Mean = new Vector(this.X.Mean.ToArray());

                cp.Y.Mean = new Vector(this.Y.Mean.ToArray());
            }
            catch (Exception ee)
            {
                int i = 0;
            }

            if (cp.X.CovMatrix == null ||  cp.X.Mean == null || cp.Y.Mean == null)
            {
                throw new InvalidCastException("Bad clone");
            }
            
            return cp;
        }

        public ClusterX X
        {
            get
            {
                return clusterX;
            }
        }

        public ClusterY Y
        {
            get
            {
                return clusterY;
            }
        }

        public void CurrentToPrev()
        {
            this.PreviousCenter = this.CurrentCenter;
            this.CurrentCenter = -1;
        }

        public int Id { get; set; }
        public int PreviousCenter { get; set; }
        public int CurrentCenter { get; set; }

    }
}
