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

        public ClusterPair(ClusterX cX, ClusterY cY)
        {
            clusterX = cX;
            clusterY = cY;

            this.PreviousCenter = 0;
            this.CurrentCenter = 0;
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
