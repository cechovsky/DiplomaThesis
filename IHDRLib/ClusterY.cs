using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class ClusterY : Cluster
    {
        public ClusterY() : base()
        {
            
        }

        public ClusterY(Sample sample) : base(sample)
        {
            this.items.Add(new Vector(sample.Y.ToArray(), sample.Label));
            this.mean = new Vector(sample.Y.ToArray());

            #warning TODO count covariance matrix 
        }

        



    }
}
