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
            // if sample hasn't output y assign mean of node samples 
            if (sample.Y == null)
            {
                // if its first sample
                if (this.samples.Count == 0)
                {
                    Vector newY = new Vector(sample.X.ToArray());
                    sample.SetY(newY);
                }
                // if contains some samples
                else
                {
                    sample.SetY(samples.GetMeanOfDataWithLabel(sample.Label));
                }
            }

            if (this.isLeafNode)
            {
                // do leaf node staff
                if (this.samples.Count == 0)
                {
                    // create new clusters and cluster pair
                    this.CreateNewClusters(sample);
                    this.samples.Add(sample);
                }
                else
                {
                    // update cluster pairs

                    // parameters bl bound of number of microclusters in node, dx resolution
                    // find nearest xj cluster using euclidean distance 

                    double distance = 0.0;
                    ClusterPair nearestCluster = this.GetNearestClusterPairX(sample, out distance);

                    // if is count < like bl and distance > delta create new cluster
                    // add new cluster pair (x,y), increment n
                    if (clusterPairs.Count < Params.bl && distance > Params.deltaX)
                    {
                        this.CreateNewClusters(sample);
                    }
                    // else update xj cluster and yj cluster using amnesic average
                    else
                    {
                        // update covariance matrix and mean of xcluster with amnesic average

                        // update covariance matrix and mean of ycluster with amnesic average
                    }

                    this.samples.Add(sample);
                    // spawn if necessary 
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

        #endregion

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

    }
}
