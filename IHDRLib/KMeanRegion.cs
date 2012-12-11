using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHDRLib
{
    class KMeanRegion
    {
        Vector center;
        List<Vector> assignedVectors;


        public KMeanRegion(Vector center)
        {
            this.center = new Vector(center.ToArray());
            this.assignedVectors = new List<Vector>();
        }

        public void UpdateCenter()
        {
            // TODO
        }




    }
}
