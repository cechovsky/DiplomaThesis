using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Node
    {
        private Node parent;
        private Sample lastAddedSample;
        private Samples samples;
        private List<ClusterX> clustersX;
        private List<ClusterY> clustersY;
        private List<ClusterPair> clusterPairs;
        private bool isLeafNode;

        public Node()
        {
            this.clustersX = new List<ClusterX>();
            this.clustersY = new List<ClusterY>();
            this.samples = new Samples();
            this.clusterPairs = new List<ClusterPair>();
            this.isLeafNode = true;
            this.parent = null;
        }

        public Node(Node parent)
        {
            this.parent = parent;

            clustersX = new List<ClusterX>();
            clustersY = new List<ClusterY>();
            samples = new Samples();
            clusterPairs = new List<ClusterPair>();
            isLeafNode = true;
        }

        #region properties

        public Node Parent
        {
            get
            {
                return this.parent;
            }
        }

        public List<ClusterX> ClustersX
        {
            get
            {
                return this.clustersX;
            }
        }

        public List<ClusterY> ClustersY
        {
            get
            {
                return this.clustersY;
            }
        }

        public List<ClusterPair> ClusterPairs
        {
            get
            {
                return this.clusterPairs;
            }
        }

        public bool IsLeafNode
        {
            get
            {
                return this.isLeafNode;
            }
        }


        #endregion

        #region UpdateNode

        public void UpdateNode(Sample sample)
        {
            // add sample (because of counting of output)
            samples.Add(sample);

            if (sample.Y == null)
            {
                sample.SetY(samples.GetMeanOfDataWithLabel(sample.Label));
            }

            #endregion set output of sample

            if (this.isLeafNode)
            {
                // do leaf node staff
                if (this.samples.Count == 1)
                {
                    // create new clusters and cluster pair
                    this.CreateNewClusters(sample);
                }
                else
                {
                    // update cluster pairs

                    // parameters bl bound of number of microclusters in node, dx resolution
                    // find nearest xj cluster using euclidean distance 

                    double distance = 0.0;
                    ClusterPair nearestCluster = this.GetNearestClusterPairX(sample, out distance);

                    //Console.WriteLine(distance.ToString());
                    // if is count < like bl and distance > delta create new cluster
                    // add new cluster pair (x,y), increment n
                    if (samples.Count < Params.bl && distance > Params.deltaX)
                    {
                        this.CreateNewClusters(sample);
                    }
                    // else update xj cluster and yj cluster using amnesic average
                    else
                    {
                        // add sample to clusters, update statistics of clusters
                        nearestCluster.X.AddItem(sample.X);
                        nearestCluster.Y.AddItem(sample.Y);
                    }

                    // spawn if necessary 
                    // if 2(n - q)/q2 > bs spawn to  q children
                    // use k-means alg
                    if (this.GetNSPP() > Params.bs)
                    {
                        this.Swap();
                    }


                }
            }
            else
            {
                // compute probabilities described in Subsection IV-C. ( it will be implemented latest )
                // closest X will active node to be searched deaper

                // update y clusters 
                // q count of cluster, dy resolution
                // 1. find nearest y cluster, euclidean distance
                // 2. if n < q and dy > distance, increment n, add new cluster y
                //    else update p ( e.g p = 0,2 -> 20% ) nearest cluster using amnesic average
                // return nearest cluster

                // update x cluster associated with returned y, mean with amnesic average
                // update subspace of most discrimanting subspace

            }
        }

        

        /// <summary>
        /// Create new clusers X and Y and their cluster pair
        /// </summary>
        /// <param name="sample">new sample</param>
        private void CreateNewClusters(Sample sample)
        {
            ClusterX newClusterX = new ClusterX(sample);
            this.clustersX.Add(newClusterX);
            ClusterY newClusterY = new ClusterY(sample);
            this.clustersY.Add(newClusterY);


            ClusterPair clusterPair = new ClusterPair(newClusterX, newClusterY);
            newClusterX.SetClusterPair(clusterPair);
            newClusterY.SetClusterPair(clusterPair);

            this.clusterPairs.Add(clusterPair);
        }

        #region Update cluster pairs

        private ClusterPair GetNearestClusterPairX(Sample sample, out double distance)
        {
            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.X.Mean.GetDistance(sample.X);
                if (item.X.Mean.GetDistance(sample.X) < distance)
                {
                    distance = newDistance;
                    closestPair = item;
                }

            }
            return closestPair;
        }

        #endregion

        private double GetNSPP()
        {
            return 2 * (this.samples.Count - Params.q) / Math.Pow(Params.q, 2);
        }

        #region Swapping

        private void Swap()
        {
            if (this.clusterPairs.Count < Params.q)
            {
                // create new children
            }

            // select q random samples 
            List<Vector> centres = new List<Vector>();
            Random random = new Random();
            List<int> randoms = new List<int>();
            for (int i = 0; i < Params.q; i++)
            {
                bool randomIsNotUnique = true;
                int r = 0;
                while (randomIsNotUnique)
                {
                    r = random.Next(this.clusterPairs.Count);
                    randomIsNotUnique = randoms.Contains(r);
                }

                randoms.Add(r);
                
                Vector vector = new Vector(this.clusterPairs[r].X.Mean.ToArray());
                vector.Id = i;
                centres.Add(vector);
            }

            // evaluate to which center vector belongs
            foreach (ClusterPair clPair in clusterPairs)
            {
                clPair.CurrentToPrev();
                clPair.CurrentCenter = clPair.X.Mean.GetIdOfClosestVector(centres);
            }
            

            // while assignment is not changed, do
            bool next = true;
            while (next)
            {
                for (int i = 0; i < centres.Count; i++)
                {
                    //select all vectors assignet to center
                    List<Vector> assignedVectors = clusterPairs.Where(cp => cp.CurrentCenter == centres[i].Id).Select(cp => cp.X.Mean).ToList<Vector>();
                    // update center
                    if(assignedVectors.Count > 0) centres[i] = Vector.GetMeanOfVectors(assignedVectors);
                    centres[i].Id = i;
                }
                
                // update assignments 
                // evaluate to which center vector belongs
                foreach (ClusterPair clPair in clusterPairs)
                {
                    clPair.CurrentToPrev();
                    clPair.CurrentCenter = clPair.X.Mean.GetIdOfClosestVector(centres);
                }

                next = this.AssigmentIsChanged();
            }
        }

        /// <summary>
        /// return true if assignment to centres is changed
        /// </summary>
        /// <returns></returns>
        private bool AssigmentIsChanged()
        {
            foreach (var item in clusterPairs)
            {
                if (item.CurrentCenter != item.PreviousCenter) return true;
            }

            return false;
        }

        #endregion

    }

    
}
