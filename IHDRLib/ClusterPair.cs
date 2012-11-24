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


    }
}
