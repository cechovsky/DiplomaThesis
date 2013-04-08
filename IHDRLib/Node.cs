using ILNumerics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IHDRLib
{
    [Serializable]
    public class Node : ISerializable
    {
        private int id;
        private Node parent;
        //private Samples samples;
        private List<ClusterX> clustersX;
        private List<ClusterY> clustersY;
        private List<ClusterPair> clusterPairs;
        private bool isLeafNode;
        private bool isPlastic;
        private int countOfSamples;
        private string path;
        private ILArray<double> gSOManifold;
        protected ILArray<double> covarianceMatrixMeanMDF;
        protected ILArray<double> covarianceMatrixMean;
        private ILArray<double> meanMDF;
        private ILArray<double> varianceMDF;
        protected ILArray<double> c;
        private List<Node> children;
        private double deltaX;
        private double deltaY;
        private List<Sample> samples;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", id, typeof(int));
            info.AddValue("parent", parent, typeof(Node));
            //info.AddValue("samples", samples, typeof(Samples));
            info.AddValue("clustersX", clustersX, typeof(List<ClusterX>));
            info.AddValue("clustersY", clustersY, typeof(List<ClusterY>));
            info.AddValue("clusterPairs", clusterPairs, typeof(List<ClusterPair>));
            info.AddValue("isLeafNode", isLeafNode, typeof(bool));
            info.AddValue("isPlastic", isPlastic, typeof(bool));
            info.AddValue("countOfSamples", countOfSamples, typeof(int));
            info.AddValue("path", path, typeof(string));
            info.AddValue("gSOManifold", gSOManifold, typeof(ILArray<double>));
            info.AddValue("covarianceMatrixMeanMDF", covarianceMatrixMeanMDF, typeof(ILArray<double>));
            info.AddValue("covarianceMatrixMean", covarianceMatrixMean, typeof(ILArray<double>));
            info.AddValue("c", c, typeof(ILArray<double>));
            info.AddValue("children", children, typeof(List<Node>));
            
        }

        // The special constructor is used to deserialize values. 
        public Node(SerializationInfo info, StreamingContext context)
        {
            id = (int)info.GetValue("id", typeof(int));
            parent = (Node)info.GetValue("parent", typeof(Node));
            //samples = (Samples)info.GetValue("samples", typeof(Samples));
            clustersX = (List<ClusterX>)info.GetValue("clustersX", typeof(List<ClusterX>));
            clustersY = (List<ClusterY>)info.GetValue("clustersY", typeof(List<ClusterY>));
            clusterPairs = (List<ClusterPair>)info.GetValue("clusterPairs", typeof(List<ClusterPair>));
            isLeafNode = (bool)info.GetValue("isLeafNode", typeof(bool));
            isPlastic = (bool)info.GetValue("isPlastic", typeof(bool));
            countOfSamples = (int)info.GetValue("countOfSamples", typeof(int));
            path = (string)info.GetValue("path", typeof(string));
            gSOManifold = (ILArray<double>)info.GetValue("gSOManifold", typeof(ILArray<double>));
            covarianceMatrixMeanMDF = (ILArray<double>)info.GetValue("covarianceMatrixMeanMDF", typeof(ILArray<double>));
            covarianceMatrixMean = (ILArray<double>)info.GetValue("covarianceMatrixMean", typeof(ILArray<double>));
            c = (ILArray<double>)info.GetValue("c", typeof(ILArray<double>));
            children = (List<Node>)info.GetValue("children", typeof(List<Node>));
        }

        public Node(double deltaX, double deltaY)
        {
            this.clustersX = new List<ClusterX>();
            this.clustersY = new List<ClusterY>();
            //this.samples = new Samples();
            this.clusterPairs = new List<ClusterPair>();
            this.isLeafNode = true;
            this.parent = null;
            this.countOfSamples = 0;
            this.samples = new List<Sample>();

            this.children = new List<Node>(Params.q);
            this.path = Params.savePath + "root\\";

            this.deltaX = deltaX;
            this.deltaY = deltaY;

            this.IsPlastic = true;
        }

        public Node(Node parent, string path, double deltaX, double deltaY)
        {
            this.parent = parent;

            clustersX = new List<ClusterX>();
            clustersY = new List<ClusterY>();
            this.samples = new List<Sample>();
            clusterPairs = new List<ClusterPair>();
            isLeafNode = true;
            countOfSamples = 0;

            this.children = new List<Node>(Params.q);
            this.path = path;

            if (deltaX < Params.deltaXMin)
            {
                this.deltaX = deltaX;
            }
            if (deltaY < Params.deltaYMin)
            {
                this.deltaY = deltaY;
            }

            this.IsPlastic = true;
            
        }

        #region properties

        public int CountOfSamples
        {
            get
            {
                return this.countOfSamples;
            }
            set
            {
                this.countOfSamples = value;
            }
        }

        public Node Parent
        {
            get
            {
                return this.parent;
            }
        }

        public ILArray<double> C
        {
            get
            {
                return this.c;
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
            set
            {
                this.isLeafNode = value;
            }
        }

        public bool IsPlastic
        {
            get
            {
                return this.isPlastic;
            }
            set
            {
                this.isPlastic = value;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public ILArray<double> GSOManifold
        {
            get
            {
                return this.gSOManifold;
            }
            set
            {
                this.gSOManifold = value;
            }
        }

        public ILArray<double> CovarianceMatrixMeanMDF
        {
            get
            {
                return this.covarianceMatrixMeanMDF;
            }
            set
            {
                this.covarianceMatrixMeanMDF = value;
            }
        }

        public ILArray<double> VarianceMDF
        {
            get
            {
                return this.varianceMDF;
            }
        }


        public ILArray<double> CovarianceMatrixMean
        {
            get
            {
                return this.covarianceMatrixMean;
            }
            set
            {
                this.covarianceMatrixMean = value;
            }
        }

        #endregion

        #region UpdateNode

        public void UpdateNode(Sample sample)
        {
            //Console.WriteLine("Add sample " + count.ToString());

            // add sample (because of counting of output)
            if (Params.StoreSamples)
            {
                this.samples.Add(sample);
            }

            // count y of sample, if it is null
            if (sample.Y == null)
            {
                throw new InvalidOperationException("Output Y of sample is null");
            }

            this.countOfSamples++;
            if (this.isLeafNode)
            {
                // do leaf node staff
                if (this.countOfSamples == 1)
                {
                    // create new clusters and cluster pair
                    this.CreateNewClusters(sample, this);
                }
                else
                {
                    // update cluster pairs
                    this.UpdateClusterPairsX(sample);
                }
            }
            else
            {
                // update y clusters 
                // q count of cluster, dy resolution
                // 1. find nearest y cluster, euclidean distance
                // 2. if n < q and dy > distance, increment n, add new cluster y
                //    else update p ( e.g p = 0,2 -> 20% ) nearest cluster using amnesic average
                // return nearest cluster

                // update x cluster associated with returned y, mean with amnesic average

                if (this.isPlastic)
                {
                    this.UpdateClusters(sample);
                    
                }

                double distance = 0;
                int index = 0;
                ClusterPair nearestClPair = this.GetNearestClusterPairXBySDNLL_MDF(sample, out distance, out index);

                Node next = nearestClPair.CorrespondChild;
                  
                next.UpdateNode(sample);                
            }
        }

        public void UpdateNode_ForSwapping(Sample sample)
        {
            this.countOfSamples++;
            if (this.isLeafNode)
            {
                // do leaf node staff
                if (this.countOfSamples == 1)
                {
                    // create new clusters and cluster pair
                    this.CreateNewClusters(sample, this);
                }
                else
                {
                    // update cluster pairs
                    this.UpdateClusterPairsX_ForSwapping(sample);
                }
            }
        }

        #endregion

        /// <summary>
        /// Create new clusers X and Y and their cluster pair
        /// </summary>
        /// <param name="sample">new sample</param>
        private void CreateNewClusters(Sample sample, Node parent)
        {
            ClusterX newClusterX = new ClusterX(sample, parent);
            this.clustersX.Add(newClusterX);
            ClusterY newClusterY = new ClusterY(sample, parent);
            this.clustersY.Add(newClusterY);

            ClusterPair clusterPair = new ClusterPair(newClusterX, newClusterY, sample);
            newClusterX.SetClusterPair(clusterPair);
            newClusterY.SetClusterPair(clusterPair);

            clusterPair.Id = clusterPairs.Count;
            clusterPair.Samples.Add(sample);

            this.clusterPairs.Add(clusterPair);
        }

        #region Update cluster pairs

        public ClusterPair GetNearestClusterPairX(Sample sample, out double distance, out int index)
        {
            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            int i = 0;
            index = -1;
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.X.Mean.GetDistance(sample.X);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closestPair = item;
                    index = i;
                }
                i++;
            }

            return closestPair;
        }

        private ClusterPair GetNearestClusterPairY(Sample sample, out double distance, out int index)
        {
            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            int i = 0;
            index = -1;
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.Y.Mean.GetDistance(sample.Y);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closestPair = item;
                    index = i;
                }
                i++;
            }

            return closestPair;
        }

        private List<Tuple<double, ClusterPair>>  GetDistancesAndClusterPairsY(Sample sample)
        {
            List<Tuple<double, ClusterPair>> result = new List<Tuple<double, ClusterPair>>();
            foreach (ClusterPair item in clusterPairs)
            {
                if (sample.Y.Values.Count() < 2)
                {
                    throw new InvalidOperationException("Bad operation");
                }
                double newDistance = item.Y.Mean.GetDistance(sample.Y);
                result.Add(new Tuple<double,ClusterPair>(newDistance, item));
            }

            result = result.OrderBy(i => i.Item1).ToList();
            return result;
        }

        /// <summary>
        /// get nearest cluster pair by most dicrimnating features vector
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private ClusterPair GetNearestClusterPairXMDF(Sample sample, out double distance, out int index)
        {
            ILArray<double> thisVector = sample.X.Values.ToArray();
            ILArray<double> scaterPart = thisVector - C;
            ILArray<double> vector = ILMath.multiply(this.gSOManifold.T, scaterPart.ToArray());
                
            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            int i = 0;
            index = -1;
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.X.GetMDFDistanceFromMDFMean(vector);
                //Console.WriteLine("Distance in MDF : " + newDistance.ToString());
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closestPair = item;
                    index = i;
                }
                i++;
            }

            return closestPair;
        }

        /// <summary>
        /// get nearest cluster pair by most dicrimnating features vector
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private ClusterPair  GetNearestClusterPairXBySDNLL_MDF(Sample sample, out double distance, out int index)
        {
            // convert vector to mdf vector
            ILArray<double> x = sample.X.Values.ToArray();
            ILArray<double> scaterPart = x - this.c;
            ILArray<double> mdfVector = ILMath.multiply(this.gSOManifold.T, scaterPart);

            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            int i = 0;
            index = -1;
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.X.GetSDNLL_MDF(mdfVector);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closestPair = item;
                    index = i;
                }
                i++;
            }

            //Console.WriteLine("MDF" + sample.Id.ToString() + " : " + index.ToString());

            return closestPair;
        }

        private List<ClusterPair> GetClosestClusterPairsAndSDNLLDistances_MDF(Sample sample)
        {
            // convert vector to mdf vector
            ILArray<double> x = sample.X.Values.ToArray();
            ILArray<double> scaterPart = x - this.c;
            ILArray<double> mdfVector = ILMath.multiply(this.gSOManifold.T, scaterPart);

            List<Tuple<double, ClusterPair>> results = new List<Tuple<double, ClusterPair>>();

            foreach(ClusterPair item in clusterPairs)
            {
                double newDistance = item.X.GetSDNLL_MDF(mdfVector);
                results.Add(new Tuple<double,ClusterPair>(newDistance, item));
            }

            return results.OrderBy(item => item.Item1).Select(item => item.Item2).Take(Params.WidthOfTesting).ToList();
        }

        public List<Node> GetNodesToSearch(Sample sample)
        {
            List<ClusterPair> clusterPairs = this.GetClosestClusterPairsAndSDNLLDistances_MDF(sample);

            return clusterPairs.GroupBy(item => item.CorrespondChild.Id).Select(item => item.First().CorrespondChild).ToList();
        }

        private void UpdateClusterPairsX(Sample sample)
        {
            // update cluster pairs

            // parameters bl bound of number of microclusters in node, dx resolution
            // find nearest xj cluster using euclidean distance 

            double distance = 0.0;
            int index = 0;
            ClusterPair nearestCluster = this.GetNearestClusterPairX(sample, out distance, out index);
            
            //Console.WriteLine(distance.ToString());
            // if is count < like bl and distance > delta create new cluster
            // add new cluster pair (x,y), increment n
            if (clusterPairs.Count < Params.blx && distance > this.deltaX)
            {
                this.CreateNewClusters(sample, this);
            }
            // else update xj cluster and yj cluster using amnesic average
            else
            {
                // add sample to clusters, update statistics of clusters
                nearestCluster.AddItem(sample);
            }

            // spawn if necessary 
            // if 2(n - q)/q2 > bs spawn to  q children
            // use k-means alg
            if (this.GetNSPP() > Params.bs)
            {
                #region Swapping evaluation log

                //for (int i = 0; i < 10; i++)
                //{
                //    this.EvaluateSwap();

                //    Console.WriteLine("Round" + i.ToString());
                //    for (int j = 0; j < Params.q; j++)
                //    {
                //        Node node = new Node(this);
                //        List<ClusterPair> clPairs = this.clusterPairs.Where(cp => cp.CurrentCenter == j).ToList();
                //        Console.WriteLine("Region" + j.ToString() + ": " + clPairs.Count.ToString());
                //    }
                //}

                #endregion

                if (Params.SwapType == 1)
                {
                    this.EvaluateSwap();
                    this.Swap();
                }
                if (Params.SwapType == 2)
                {
                    this.EvaluateSwap();
                    this.Swap_Modified();
                }
                if (Params.SwapType == 3)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (this.KMeansClustering())
                        {
                            break;
                        }
                        if (i == 9)
                        {
                            throw new InvalidOperationException("Not successfull keans clustering.");
                        }
                    }
                    this.EvaluateSwap();
                    this.Swap();
                }
                if (Params.SwapType == 4)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (this.KMeansClusteringY())
                        {
                            break;
                        }
                        if (i == 9)
                        {
                            throw new InvalidOperationException("Not successfull keans clustering.");
                        }
                    }
                    this.EvaluateSwap();
                    this.Swap();
                }

                // count most discriminating features space etc.
                this.CountC();
                this.CountGSOManifold();
                this.CountMDFOfVectors();
                this.CountMDFMeans();
                this.CountCovarianceMatricesMDF();
                
                //count cov matrix mean
                this.CountCovarianceMatrixMeanMDF();
                this.CountMeanAndVarianceMDF();

                // dispose cluster items
                if (!Params.StoreItems)
                {
                    this.DisposeClustersItems();
                }
            }
        }

        private bool KMeansClustering()
        {
            if (!Params.StoreSamples)
            {
                throw new InvalidOperationException("Unable to evaluate KMeans without stored samples");
            }

            int countOfKmeansTry = 0;

            Random random = new Random();
            //List<Sample> centersCandidates = this.samples.OrderBy(item => random.Next()).Take((int)Params.blx).ToList();
            List<Sample> centersCandidates = this.samples.Take((int)Params.blx).ToList();

            List<Sample> centers = new List<Sample>();
            foreach (var item in centersCandidates)
            {
                centers.Add(new Sample(item.X.Values.ToArray(), item.Label, item.Id));
            }

            // set centers id 
            int i = 1;
            foreach (var item in centers)
            {
                item.CenterId = i;
                i++;
            }

            //set assigment to 0
            foreach (var item in this.samples)
	        {
                item.ClusterAssignemntNew = 0;
                item.ClusterAssignemntOld = 0;
	        }

            // do k-means
            bool allAssigmentsEquals = false;
            while (!allAssigmentsEquals)
            {
                foreach (var item in this.samples)
                {
                    item.ClusterAssignemntOld = item.ClusterAssignemntNew;
                    item.ClusterAssignemntNew = this.GetCenterIdOfClosestCenter(centers, item);
                }

                allAssigmentsEquals = this.AllAssigmentsEquals(this.samples);

                foreach (var item in centers)
                {
                    var centerSamples =
                        this.samples.Where(sample => sample.ClusterAssignemntNew == item.CenterId).ToList();
                    
                    if (centerSamples.Count == 0)
                    {
                        Console.WriteLine("Not successful kmeans clustering.");
                        if (countOfKmeansTry > 10)
                        {
                            throw new InvalidOperationException("Unable to execute kmeans on this data.");
                        }

                        return false;
                    }
                    this.UpdateCenter(item, centerSamples);
                }
            }

            // checking bad clusters created by kmeans, if some are bad, call this method next time
            foreach (var item in centers)
            {
                List<Sample> samplesOfCenter = this.samples.Where(sample => sample.ClusterAssignemntNew == item.CenterId).ToList();

                if (samplesOfCenter.Count == 0)
                {
                    Console.WriteLine("Not successful kmeans clustering.");

                    return false;
                }
            }

            // create new clusters
            this.clusterPairs = new List<ClusterPair>();
            this.clustersX = new List<ClusterX>();
            this.clustersY = new List<ClusterY>();

            foreach (var item in centers)
            {
                List<Sample> samplesOfCenter = this.samples.Where(sample => sample.ClusterAssignemntNew == item.CenterId).ToList();

                this.CreateNewClusters(samplesOfCenter[0], this);
                ClusterPair newClPair = this.ClusterPairs[this.ClusterPairs.Count - 1];

                for (int j = 1; j < samplesOfCenter.Count; j++)
                {
                    newClPair.AddItem(samplesOfCenter[j]);
                }                        
            }

            return true;
        }

        private bool KMeansClusteringY()
        {
            if (!Params.StoreSamples)
            {
                throw new InvalidOperationException("Unable to evaluate KMeans without stored samples");
            }

            Random random = new Random();
            List<Sample> centersCandidates = this.samples.OrderBy(item => random.Next()).Take((int)Params.bly).ToList();

            List<Sample> centers = new List<Sample>();
            foreach (var item in centersCandidates)
            {
                centers.Add(new Sample(item.X.Values.ToArray(), item.Y.Values.ToArray(), item.Label, item.Id));
            }

            // set centers id 
            int i = 1;
            foreach (var item in centers)
            {
                item.CenterId = i;
                i++;
            }

            //set assigment to 0
            foreach (var item in this.samples)
            {
                item.ClusterAssignemntNew = 0;
                item.ClusterAssignemntOld = 0;
            }

            // do k-means
            bool allAssigmentsEquals = false;
            while (!allAssigmentsEquals)
            {
                foreach (var item in this.samples)
                {
                    item.ClusterAssignemntOld = item.ClusterAssignemntNew;
                    item.ClusterAssignemntNew = this.GetCenterIdOfClosestCenterY(centers, item);
                }

                allAssigmentsEquals = this.AllAssigmentsEquals(this.samples);

                if (!allAssigmentsEquals)
                {
                    foreach (var item in centers)
                    {
                        var samplesToUpdateCenter =
                            this.samples.Where(sample => sample.ClusterAssignemntNew == item.CenterId).ToList();
                       
                        if (samplesToUpdateCenter.Count == 0)
                        {
                            Console.WriteLine("Not successful kmeans clustering.");
                            return false;
                        }
                        
                    }
                }
            }

            // checking bad clusters created by kmeans, if some are bad, call this method next time
            foreach (var item in centers)
            {
                List<Sample> samplesOfCenter = this.samples.Where(sample => sample.ClusterAssignemntNew == item.CenterId).ToList();

                if (samplesOfCenter.Count == 0)
                {
                    Console.WriteLine("Not successful kmeans clustering.");
                    return false;
                }
            }

            // create new clusters
            this.clusterPairs = new List<ClusterPair>();
            this.clustersX = new List<ClusterX>();
            this.clustersY = new List<ClusterY>();

            foreach (var item in centers)
            {
                List<Sample> samplesOfCenter = this.samples.Where(sample => sample.ClusterAssignemntNew == item.CenterId).ToList();

                this.CreateNewClusters(samplesOfCenter[0], this);
                ClusterPair newClPair = this.ClusterPairs[this.ClusterPairs.Count - 1];

                for (int j = 1; j < samplesOfCenter.Count; j++)
                {
                    newClPair.AddItem(samplesOfCenter[j]);
                }
            }

            return true;
        }

        // for k-means
        private void UpdateCenter(Sample center, List<Sample> listOfSamples)
        {
            center.X = Vector.GetMeanOfVectors(listOfSamples.Select(sample => sample.X).ToList());
        }

        // for k-means
        private void UpdateCenterY(Sample center, List<Sample> listOfSamples)
        {
            center.Y = Vector.GetMeanOfVectors(listOfSamples.Select(sample => sample.Y).ToList());
        }

        private int GetCenterIdOfClosestCenter(List<Sample> centers, Sample sample)
        {
            int returnId = 0;
            double minDistance = Double.MaxValue;

            foreach (var item in centers)
            {
                double distance = item.X.GetDistance(sample.X);
                if (distance < minDistance)
                {
                    returnId = item.CenterId;
                    minDistance = distance;
                }
            }

            return returnId;
        }

        private int GetCenterIdOfClosestCenterY(List<Sample> centers, Sample sample)
        {
            int returnId = 0;
            double minDistance = Double.MaxValue;

            foreach (var item in centers)
            {
                double distance = item.Y.GetDistance(sample.Y);
                if (distance < minDistance)
                {
                    returnId = item.CenterId;
                    minDistance = distance;
                }
            }

            return returnId;
        }

        private bool AllAssigmentsEquals(List<Sample> sampleslist)
        {
            foreach (var item in sampleslist)
            {
                if (item.ClusterAssignemntNew != item.ClusterAssignemntOld)
                    return false;
            }
            return true;
        }

        private void UpdateClusterPairsX_ForSwapping(Sample sample)
        {
            double distance = 0.0;
            int index = 0;
            ClusterPair nearestCluster = this.GetNearestClusterPairX(sample, out distance, out index);

            //Console.WriteLine(distance.ToString());
            // if is count < like bl and distance > delta create new cluster
            // add new cluster pair (x,y), increment n
            if (clusterPairs.Count < Params.blx && distance > this.deltaX)
            {
                this.CreateNewClusters(sample, this);
            }
            // else update xj cluster and yj cluster using amnesic average
            else
            {
                // add sample to clusters, update statistics of clusters
                nearestCluster.AddItem(sample);
            }
        }

        #endregion

        #region Update Node

        private void UpdateClusters(Sample sample)
        {
            // parameters bly bound of number of y clusters in node, dy resolution
            // find nearest xj cluster using euclidean distance 

            ClusterPair nearestCluster = null;
            double distance = double.MaxValue;
            int index = -1;
            
            //nearestCluster = this.GetNearestClusterPairY(sample, out distance, out index);

            List<Tuple<double, ClusterPair>> orderedClusterPairs = this.GetDistancesAndClusterPairsY(sample);

            // if is count < like bly and distance > deltay create new cluster
            // add new cluster pair (x,y), increment n
            if (this.clusterPairs.Count < Params.bly && orderedClusterPairs[0].Item1 > this.deltaY)
            {
                if (!Params.StoreItems)
                {
                    throw new InvalidOperationException("Not possible create new cluster. No items available for MDF counting.");
                }

                this.CreateNewClusters(sample, this);

                // update MDF space
                this.CountC();
                this.CountGSOManifold();
                this.CountMDFOfVectors();
                this.CountMDFMeans();
                this.CountCovarianceMatricesMDF();

                //count cov matrix mean
                this.CountCovarianceMatrixMeanMDF();
            }
            // else update p percents of xj cluster and yj cluster using amnesic average
            else
            {
                #warning TODO update ceratin portion

                int countOfClusters = orderedClusterPairs.Count;
                int countOfClustersToUpdate = (int)((orderedClusterPairs.Count - 1 ) * Params.p) + 1;


                //Update a certain portion p (e.g., p = 0:2, i.e., pulling top 20%) of nearest clusters using the amnesic average
                //explained in Section III-F and return the index j
                for (int i = 0; i < countOfClustersToUpdate; i++)
                {
                    //Console.WriteLine("Y");
                    orderedClusterPairs[i].Item2.Y.AddItem(sample.Y, sample.Label);
                }

                Vector newItem = new Vector(sample.X.Values.ToArray());
                newItem.Label = sample.Label;
                newItem.CountMDF(this.gSOManifold, this.c);

                // add sample to clusters, update statistics of clusters
                orderedClusterPairs[0].Item2.X.AddItemNonLeaf(newItem);
                
                if (Params.StoreSamples)
                {
                    orderedClusterPairs[0].Item2.Samples.Add(sample);
                }
               
                // update meanMDF and varianceMDF
                this.UpdateMeanAndVarianceMdf(newItem);

                //count cov matrix mean
                this.CountCovarianceMatrixMeanMDF();
            }
        }

        private ClusterPair GetNearestClusterPairY(Sample sample, out double distance)
        {
            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.Y.Mean.GetDistance(sample.Y);
                if (item.Y.Mean.GetDistance(sample.Y) < distance)
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
            double nspp = 2 * (this.countOfSamples - this.ClusterPairs.Count) / Math.Pow(this.ClusterPairs.Count, 2);
            //Console.WriteLine("Nspp :" + nspp.ToString());
            return nspp;
        }

        #region Swapping

        private void EvaluateSwap()
        {
            if (this.clusterPairs.Count < Params.q)
            {
                throw new InvalidOperationException("Small count of microclusters, impossible to swap");
            }

            // random selection is deleted, because of unification of results
            // select q random samples 
            //List<Vector> centres = new List<Vector>();
            //Random random = new Random();
            //List<int> randoms = new List<int>();
            //for (int i = 0; i < Params.q; i++)
            //{
            //    bool randomIsNotUnique = true;
            //    int r = 0;
                
            //    while (randomIsNotUnique)
            //    {
            //        r = random.Next(this.clusterPairs.Count);
            //        randomIsNotUnique = randoms.Contains(r);
            //    }
            //    //Console.WriteLine("Center like cluster: " + r.ToString());
            //    randoms.Add(r);
                
            //    Vector vector = new Vector(this.clusterPairs[r].X.Mean.Values.ToArray());
            //    vector.Id = i;
            //    centres.Add(vector);
            //}

            #region instead of random

            List<Vector> centres = new List<Vector>();
            for (int i = 0; i < Params.q; i++)
            {
                Vector vector = new Vector(this.clusterPairs[i].X.Mean.Values.ToArray());
                vector.Id = i;
                centres.Add(vector);
            }


            #endregion 

            // evaluate to which center vector belongs
            foreach (ClusterPair clPair in clusterPairs)
            {
                clPair.CurrentToPrev();
                clPair.CurrentCenter = clPair.X.Mean.GetIdOfClosestVector(centres);
            }
            

            // while assignment is not changed, do
            bool next = true;
            int whil = 1;
            while (next)
            {
                //Console.WriteLine("While cycle " + whil.ToString());
                for (int i = 0; i < centres.Count; i++)
                {
                    //select all vectors assignet to center
                    List<Vector> assignedVectors = clusterPairs.Where(cp => cp.CurrentCenter == centres[i].Id).Select(cp => cp.X.Mean).ToList<Vector>();
                    // update center
                    if (assignedVectors.Count > 0)
                    {
                        Vector v = new Vector(Params.inputDataDimension, 0.0);
                        double dis1 = centres[i].GetDistance(v);
                        centres[i] = Vector.GetMeanOfVectors(assignedVectors);
                        double dis2 = centres[i].GetDistance(v);

                        //Console.WriteLine("Center " + i.ToString() + " Distance 1: " + dis1.ToString() + " Distance 2: " + dis2.ToString());
                    }
                    centres[i].Id = i;
                }
                
                // update assignments 
                // evaluate to which center vector belongs
                foreach (ClusterPair clPair in clusterPairs)
                {
                    clPair.CurrentToPrev();
                    clPair.CurrentCenter = clPair.X.Mean.GetIdOfClosestVector(centres);

                    //Console.WriteLine("Change cl. " + clPair.Id.ToString() + " from " + clPair.PreviousCenter.ToString() + " to " + clPair.CurrentCenter.ToString());
                }

                next = this.AssigmentIsChanged();

                whil++;
            }
        }


        private void Swap()
        {
            this.IsLeafNode = false;

            //foreach (var item in this.clusterPairs)
            //{
            //    Console.WriteLine("Center " + item.CurrentCenter.ToString());
            //} 

            for (int i = 0; i < Params.q; i++)
            {
                // create node + set save path
                Node node = new Node(this, this.path + "node" + (children.Count + 1).ToString() + @"\", this.deltaX * Params.deltaMultiplyReduction, this.deltaY * Params.deltaMultiplyReduction);
                List<ClusterPair> clPairs = this.clusterPairs.Where(cp => cp.CurrentCenter == i).ToList();

                // set id of node
                node.Id = this.children.Count + 1;

                foreach (var item in clPairs)
                {
                    ClusterPair clusterPair = item.GetClone();
                    node.AddClusterPair(clusterPair);

                    // set reference to ClusterPair, which node correspond to this ClusterPair
                    item.CorrespondChild = node;

                    if (node == null)
                    {
                        throw new InvalidOperationException("Node cant be null");
                    }
                }

                // set parent to clusters X
                node.SetParentToXClusters();

                // count covariance patrix mean for probability counting
                // node.CountCovarianceMatrixMean();

                // set plastic/non plastic 
                this.UpdatePlasticityOfParents(node);

                // add children
                this.children.Add(node);
            }
        }

        private void Swap_Modified()
        {
            if (!Params.StoreSamples)
            {
                throw new InvalidOperationException("Unable to swap modified because samples are not stored in memory.");
            }

            this.IsLeafNode = false;

            for (int i = 0; i < Params.q; i++)
            {
                // create node + set save path
                Node node = new Node(this, this.path + "node" + (children.Count + 1).ToString() + @"\", this.deltaX - Params.deltaXReduction, this.deltaY - Params.deltaYReduction);
                List<ClusterPair> clPairs = this.clusterPairs.Where(cp => cp.CurrentCenter == i).ToList();

                // set id of node
                node.Id = this.children.Count + 1;

                List<Sample> sampleForNewNode = new List<Sample>();
                foreach (var item in clPairs)
                {
                    // set reference to ClusterPair, which node correspond to this ClusterPair
                    item.CorrespondChild = node;
                    sampleForNewNode.AddRange(item.Samples);
                }

                foreach (var item in sampleForNewNode)
                {
                    node.UpdateNode_ForSwapping(item);
                }

                // set parent to clusters X
                node.SetParentToXClusters();

                // count covariance patrix mean for probability counting
                // node.CountCovarianceMatrixMean();

                // set plastic/non plastic 
                this.UpdatePlasticityOfParents(node);

                // add children
                this.children.Add(node);
            }
        }

        public void SetParentToXClusters()
        {
            foreach (var item in ClustersX)
            {
                item.Parent = this;
            }
        }

        /// <summary>
        /// count MDF means in all clusters
        /// </summary>
        public void CountMDFMeans()
        {
            foreach (var item in this.clustersX)
            {
                item.CountMDFMean();
            }
        }

        /// <summary>
        /// The dispose clusters items.
        /// </summary>
        public void DisposeClustersItems()
        {
            foreach (var item in this.clusterPairs)
            {
                item.X.DisposeItems();
                item.Y.DisposeItems();
            }
        }

        /// <summary>
        /// count MDF means in all clusters
        /// </summary>
        public void CountCovarianceMatricesMDF()
        {
            foreach (var item in clustersX)
            {
                item.CountCovarianceMatrixMDF();
            }
        }

        /// <summary>
        /// count MDF covariance mean
        /// </summary>
        public void CountCovarianceMatricesMeanMDF()
        {
            this.covarianceMatrixMeanMDF = ILMath.zeros(this.clustersX.Count - 1, this.clustersX.Count - 1);
            foreach (var item in this.clustersX)
            {
                this.covarianceMatrixMeanMDF = this.covarianceMatrixMeanMDF + item.CovMatrixMDF;
            }
            this.covarianceMatrixMeanMDF = this.covarianceMatrixMeanMDF / this.clustersX.Count;
        }

        public void CountMeanAndVarianceMDF()
        {
            List<Vector> allSamples = this.ClustersX.SelectMany(cl => cl.Items).ToList();

            this.CountMeanMDF(allSamples);
            this.CountVarianceMDF(allSamples);    
        }

        public void CountMeanMDF(List<Vector> allsamples)
        {
            this.meanMDF = ILMath.array<double>(0.0, allsamples[0].ValuesMDF.Length);

            foreach (var item in allsamples)
            {
                this.meanMDF = this.meanMDF + item.ValuesMDF;
            }

            this.meanMDF = this.meanMDF / allsamples.Count;
        }

        public void CountVarianceMDF(List<Vector> allSamples)
        {
            this.varianceMDF = ILMath.array<double>(0.0, allSamples[0].ValuesMDF.Length);

            foreach (var item in allSamples)
            {
                ILArray<double> diff = ILMath.array<double>(item.ValuesMDF.ToArray());
                diff = diff - this.meanMDF;
                diff = diff * diff;
                this.varianceMDF = this.varianceMDF + diff;
            }

            this.varianceMDF = this.varianceMDF / (allSamples.Count - 1);
        }

        public void UpdateMeanAndVarianceMdf(Vector newItem)
        {
            if (this.countOfSamples < 2)
            {
                throw new InvalidOperationException("Small count of samples");
            }

            ILArray<double> oldsn = (this.countOfSamples - 2) * this.VarianceMDF;
            ILArray<double> oldmean = this.meanMDF.ToArray();
            ILArray<double> x = newItem.ValuesMDF.ToArray();

            this.UpdateMeanMdf(newItem);

            ILArray<double> newMean = this.meanMDF.ToArray();

            ILArray<double> newsn = oldsn + ((x - oldmean) * (x - newMean));

            this.varianceMDF = newsn / (this.countOfSamples - 1);

        }

        public void UpdateMeanMdf(Vector newItem)
        {
            ILArray<double> newsample = newItem.ValuesMDF.ToArray();
            this.meanMDF = (this.meanMDF * ((double)(this.countOfSamples - 1) / (double)this.countOfSamples)) + ((1 / (double)this.countOfSamples) * newsample);
        }

        /// <summary>
        /// count most discrimating vectors for all x 
        /// </summary>
        private void CountMDFOfVectors()
        {
            foreach (var item in clustersX)
            {
                item.CountMDFOfItems(this.gSOManifold, this.c);
            }
        }

        private void TestDistancesFromMDF()
        {
            Vector startPoint = clustersX[0].Items[0];
            foreach (var item in clustersX)
            {
                int i = 0;
                foreach (var vector in item.Items)
                {
                    double normal = vector.GetDistance(startPoint);
                    double MDF = vector.GetMDFDistance(startPoint.ValuesMDF);
                    //Console.WriteLine("Normal " + i.ToString() + " : "+ normal.ToString());
                    //Console.WriteLine("MDF " + i.ToString() + " : " + MDF.ToString());
                }
            }
        }

        private void UpdatePlasticityOfParents(Node node)
        {
            //Node n = node;
            //for (int i = 0; i <= Params.l; i++)
            //{
            //    if (n.parent != null)
            //    {
            //        n = n.parent;
            //        if (i == Params.l)
            //        {
            //            n.IsPlastic = false;
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            Node n = node;
            int distanceFromLeaf = 0;
            while (node != null)
            {
                if (node.Parent != null)
                {
                    node = node.Parent;
                }
                else
                {
                    node = null;
                }
                distanceFromLeaf++;
                if (node != null && distanceFromLeaf > Params.l)
                {
                    node.IsPlastic = false;
                }
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

        public void AddClusterPair(ClusterPair clusterPair)
        {
            this.clusterPairs.Add(clusterPair);
            this.clustersX.Add(clusterPair.X);
            this.clustersY.Add(clusterPair.Y);
        }

        public void AddClusterPairs(List<ClusterPair> clusterPairs)
        {
            foreach (var item in ClusterPairs)
            {
                this.AddClusterPair(item);
            }
        }

        public Node GetNextAByEuclidean(Sample sample)
        {
            if (children.Count > 0)
            {
                double distance = double.MaxValue;
                Node result = children[0];

                foreach (var node in children)
                {
                    int clusterIndex = 1;
                    foreach (var cluster in node.ClustersX)
                    {
                        double tmpDistance = cluster.Mean.GetDistance(sample.X);

                        //Console.WriteLine(String.Format("E : N : {0}, C:{1}, D:{2}", node.Id, clusterIndex, tmpDistance.ToString()));
                        clusterIndex++;

                        if (tmpDistance < distance)
                        {
                            distance = tmpDistance;
                            result = node;
                        }
                    }
                }
                
                return result;
            }

            return null;             
        }

        #region SavingToFileHierarchy

        public void SaveToFileHierarchy()
        {
            this.SetPathsToClusters();
            this.SaveClusters();

            // save children
            foreach (var child in children)
            {
                child.SaveToFileHierarchy();
            }
        }

        private void SetPathsToClusters()
        {
            int i = 1;
            string ii = "";
            foreach (var item in ClustersX)
            {
                if (i < 10)
                {
                    ii = "0" + i.ToString();
                }
                else
                {
                    ii = i.ToString();
                }

                item.SavePath = this.path + "cluster" + ii + "\\";
                i++;
            }
        }

        private void SaveClusters()
        {
            foreach (var item in clustersX)
            {
                item.SaveSamples();
            }
        }

        #endregion

        public void EvaluateClustersLabels()
        {
            if (this.IsLeafNode)
            {
                foreach (var item in clustersX)
                {
                    item.CountLabelOfCluter();
                }
            }
            else
            {
                foreach (var item in this.children)
                {
                    item.EvaluateClustersLabels();
                }
            }
        }

        public void EvaluateAllClustersLabels()
        {
            
            foreach (var item in clustersX)
            {
                item.CountLabelOfCluter();
            }
            
            foreach (var item in this.children)
            {
                item.EvaluateAllClustersLabels();
            }
        }

        #region Grand Schmidt Ortogonalisation Process

        public void CountC()
        {
            this.c = this.GetCFromClustersX().Values.ToArray();
        }

        public void CountGSOManifold()
        {
            // get scatter vecrtors, count = q-1, we ignore cluster with less samples
            List<Vector> scatterVectors = this.GetScatterVectors();
            this.gSOManifold = this.GetManifold(scatterVectors);
        }

        public ILArray<double> GetManifold(List<Vector> scatterVectors)
        {
            ILArray<double> newArray = ILMath.zeros(Params.inputDataDimension, clustersX.Count - 1);

            List<ILArray<double>> ortBasisVectors = new List<ILArray<double>>(clustersX.Count);
            // count manifold
            for (int i = 0; i < scatterVectors.Count; i++)
            {
                if (i == 0)
                {
                    ILArray<double> newVector = scatterVectors[i].Values.ToArray();
                    double normNum = Vector.GetNormalisationNum(newVector);

                    // normalize
                    newVector = newVector / normNum;

                    this.SetColumnToILArray(newArray, newVector, i);
                    ortBasisVectors.Add(newVector);
                }
                else
                {
                    // initialise ortogonal projection
                    ILArray<double> projectionW = ILMath.zeros(Params.inputDataDimension);
                    for (int j = 0; j < i; j++)
                    {
                        ILArray<double> incrementalPart = ILMath.zeros(Params.inputDataDimension);
                        ILArray<double> si = scatterVectors[i].Values.ToArray();
                        si = si.T;

                        ILArray<double> siaj = ILMath.multiply(si, ortBasisVectors[j]);
                        incrementalPart = siaj[0] * ortBasisVectors[j];

                        // count divider
                        projectionW = projectionW + incrementalPart;
                    }

                    ILArray<double> scatterVector = ILMath.array<double>(scatterVectors[i].Values.ToArray());
                    ILArray<double> newColumn = scatterVector - projectionW;

                    double normNum = Vector.GetNormalisationNum(newColumn);

                    // normalize
                    newColumn = newColumn / normNum;

                    this.SetColumnToILArray(newArray, newColumn, i);
                    ortBasisVectors.Add(newColumn);
                }
            }

            return newArray;
        }

        public void SetColumnToILArray(ILArray<double> array, ILArray<double> column, int index)
        {
            int i = 0;
            foreach (double item in column)
            {
                array[i, index] = item;
                i++;
            }
        }

        public Vector GetCFromClustersX()
        {
            //old version
            //var means = clustersX.Select(cl => cl.Mean).ToList<Vector>();
            //return Vector.GetMeanOfVectors(means);

            List<Tuple<Vector, int>> means = clustersX.Select(cl => new Tuple<Vector, int>(cl.Mean, cl.Items.Count)).ToList();

            ILArray<double> result =ILMath.zeros(Params.inputDataDimension);

            int count = 0;
            foreach (var item in means)
            {
                result = result + (item.Item1.Values * item.Item2);
                count += item.Item2;
            }

            result = result / count;

            return new Vector(result.ToArray());
        }

        public List<Vector> GetScatterVectors()
        {
            List<Vector> scatterVectors = new List<Vector>(clustersX.Count - 1);
            
            Vector C = this.GetCFromClustersX();

            foreach (var item in clustersX)
            {
                Vector newScatter = new Vector(item.Mean.Values.ToArray());
                newScatter.Subtract(C);
                scatterVectors.Add(newScatter);
            }

            // delete one vectore, we need q-1
            int i = 0;
            int index = 0;
            int minCount = int.MaxValue;
            foreach (var item in clustersX)
            {
                if (item.Items.Count < minCount)
                {
                    index = i;
                    minCount = item.Items.Count;
                }
                i++;
            }
            //removing vector
            scatterVectors.RemoveAt(index);
            return scatterVectors;
        }

        #endregion

        #region Covariance Matrix Mean 

        //public void CountCovarianceMatrixMean()
        //{
        //    ILArray<double> mean = ILMath.zeros(Params.inputDataDimension, Params.inputDataDimension);

        //    foreach (var item in clustersX)
        //    {
        //        mean = mean + item.CovMatrix;
        //    }

        //    mean = mean / this.clustersX.Count;
        //    this.covarianceMatrixMean = mean;
        //}

        public void CountCovarianceMatrixMeanMDF()
        {
            ILArray<double> mean = ILMath.zeros(this.clustersX.Count - 1, this.ClustersX.Count - 1);

            foreach (var item in clustersX)
            {
                mean = mean + (item.CovMatrixMDF * item.Items.Count);
            }

            mean = mean / this.countOfSamples;
            this.covarianceMatrixMeanMDF = mean;
        }

        #endregion

        public TestResult GetLabelOfCategory(Sample item)
        {
            if (this.isLeafNode)
            {
                double distance = double.MinValue;
                int index = int.MinValue;
                ClusterPair clPair = this.GetNearestClusterPairX(item, out distance, out index);

                TestResult tr = new TestResult()
                {
                    ClusterMeanX = clPair.X.Mean,
                    ClusterMeanY = clPair.Y.Mean,
                    Label = clPair.X.Label,
                    Samples = clPair.Samples
                };
                return tr;
            }
            
            double distance2 = double.MinValue;
            int index2 = int.MinValue;
            ClusterPair nearestClPair = this.GetNearestClusterPairXBySDNLL_MDF(item, out distance2, out index2);
            Node next = nearestClPair.CorrespondChild;
            return next.GetLabelOfCategory(item);
        }

        public ClusterPair GetTestResultByWidthSearch(Sample item)
        {
            ClusterPairTestResult result = new ClusterPairTestResult() { Distance = double.MaxValue, ClusterPair = new ClusterPair()};
            this.CountClosestClusterPairByWidthSearch(item, result);

            return result.ClusterPair;
        }

        //public void CountClosestClusterPairByWidthSearch(Sample item, ClusterPairTestResult result)
        //{
        //    if (this.children != null && this.children.Count != 0 && this.children[0].IsLeafNode)
        //    {
        //        double distance = double.MaxValue;
        //        int index = 0;
        //        ClusterPair clPair = this.GetNearestClusterPairXBySDNLL_MDF(item, out distance, out index);
        //        if (distance < result.Distance)
        //        {
        //            result.ClusterPair = clPair;
        //            result.Distance = distance;
        //        }
        //    }
        //    else
        //    {
        //        var nodesToSearch = this.GetNodesToSearch(item);

        //        foreach (var node in nodesToSearch)
        //        {
        //            node.CountClosestClusterPairByWidthSearch(item, result);
        //        }
        //    }
        //}

        public void CountClosestClusterPairByWidthSearch(Sample item, ClusterPairTestResult result)
        {
            if (this.IsLeafNode)
            {
                if (this.Parent != null)
                {
                    double distance = double.MaxValue;
                    int index = 0;
                    ClusterPair clPair = this.Parent.GetNearestClusterPairXBySDNLL_MDF(item, out distance, out index);
                    Console.WriteLine(distance.ToString());
                    if (distance < result.Distance)
                    {
                        //Console.WriteLine("Store");
                        result.ClusterPair = clPair;
                        result.Distance = distance;
                    }
                }
            }
            else
            {
                var nodesToSearch = this.GetNodesToSearch(item);

                foreach (var node in nodesToSearch)
                {
                    node.CountClosestClusterPairByWidthSearch(item, result);
                }
            }
        }

        public List<ClusterPair> GetAllLeafClusterPairs()
        {
            if (this.isLeafNode)
            {
                return this.ClusterPairs;
            }
            else
            {
                List<ClusterPair> clPairs = new List<ClusterPair>();
                foreach (var item in children)
                {
                    clPairs.AddRange(item.GetAllLeafClusterPairs());
                }
                return clPairs;
            }
        }

        public void EvaluateDepth(int depth)
        {
            foreach (var item in clusterPairs)
            {
                item.Depth = depth;
            }
            if (!this.isLeafNode)
            {
                foreach (var item in children)
                {
                    item.EvaluateDepth(depth + 1);
                }
            }
        }

        public List<ClusterPair> GetAllClusterPairs()
        {
            List<ClusterPair> clPairs = new List<ClusterPair>();

            clPairs.AddRange(this.ClusterPairs);

            foreach (var item in children)
            {
                clPairs.AddRange(item.GetAllClusterPairs());
            }

            return clPairs;
        }
    }
}
