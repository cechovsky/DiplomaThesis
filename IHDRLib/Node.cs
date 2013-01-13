using ILNumerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Node
    {
        private int id;
        private Node parent;
        private Sample lastAddedSample;
        private Samples samples;
        private List<ClusterX> clustersX;
        private List<ClusterY> clustersY;
        private List<ClusterPair> clusterPairs;
        private bool isLeafNode;
        private bool isPlastic;
        private bool isActive;
        private int count;
        private string path;
        private ILArray<double> gSOManifold;
        private int differentReturnedCluster = 0;
        protected ILArray<double> covarianceMatrixMeanMDF;
        protected ILArray<double> covarianceMatrixMean;
        protected ILArray<double> c;

        private List<Node> children;

        public Node()
        {
            this.clustersX = new List<ClusterX>();
            this.clustersY = new List<ClusterY>();
            this.samples = new Samples();
            this.clusterPairs = new List<ClusterPair>();
            this.isLeafNode = true;
            this.parent = null;
            this.count = 0;

            this.children = new List<Node>(Params.q);
            this.path = Params.savePath + "root\\";
        }

        public Node(Node parent, string path)
        {
            this.parent = parent;

            clustersX = new List<ClusterX>();
            clustersY = new List<ClusterY>();
            samples = new Samples();
            clusterPairs = new List<ClusterPair>();
            isLeafNode = true;
            count = 0;

            this.children = new List<Node>(Params.q);
            this.path = path;
        }

        #region properties

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
                this.isPlastic = true;
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
            count++;
            //Console.WriteLine("Add sample " + count.ToString());

            // add sample (because of counting of output)
            samples.Add(sample);

            // count y of sample, if it is null
            if (sample.Y == null)
            {
                sample.SetY(samples.GetMeanOfDataWithLabel(sample.Label));
            }


            if (this.isLeafNode)
            {
                // do leaf node staff
                if (this.samples.Count == 1)
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
                this.UpdateClusters(sample);
                
                // TODO update subspace of most discrimanting subspace

                #warning not euclidean
                // TODO compute probabilities described in Subsection IV-C. ( it will be implemented latest )
                // closest X will active node to be searched deaper
                Node next = this.GetNextAByEuclidean(sample);
                Node next2 = this.GetNextByPropability(sample);

                if (this.children[0].IsLeafNode)
                {
                    if (next.Id == next2.Id)
                    {
                        Console.WriteLine("Similar next node");
                    }
                    else
                    {
                        Console.WriteLine("Different next node");
                    }
                }    
                next.UpdateNode(sample);                
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

            ClusterPair clusterPair = new ClusterPair(newClusterX, newClusterY);
            newClusterX.SetClusterPair(clusterPair);
            newClusterY.SetClusterPair(clusterPair);
            clusterPair.Id = clusterPairs.Count;

            this.clusterPairs.Add(clusterPair);
        }

        #region Update cluster pairs

        private ClusterPair GetNearestClusterPairX(Sample sample, out double distance, out int index)
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

            //Console.WriteLine("Normal" + sample.Id.ToString() + " : " + index.ToString());

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

        /// <summary>
        /// get nearest cluster pair by most dicrimnating features vector
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private ClusterPair GetNearestClusterPairXMDF(Sample sample, out double distance, out int index)
        {
            ILArray<double> thisVector = sample.X.ToArray();
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

            //Console.WriteLine("Distance in MDF : " + distance.ToString());


            //Console.WriteLine("MDF" + sample.Id.ToString() + " : " + index.ToString());

            return closestPair;
        }

        /// <summary>
        /// get nearest cluster pair by most dicrimnating features vector
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private ClusterPair GetNearestClusterPairXMDFProbability(Sample sample, out double distance, out int index)
        {
            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            int i = 0;
            index = -1;
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.X.GetSDNLL_MDF(sample.X.ToArray(), this.gSOManifold, this.c);
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

        private ClusterPair GetNearestClusterPairXProbability(Sample sample, out double distance, out int index)
        {
            ILArray<double> vector = sample.X.ToArray();

            distance = double.MaxValue;
            ClusterPair closestPair = clusterPairs[0];
            int i = 0;
            index = -1;
            foreach (ClusterPair item in clusterPairs)
            {
                double newDistance = item.X.GetSDNLL(vector);
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
            if (clusterPairs.Count < Params.blx && distance > Params.deltaX)
            {
                this.CreateNewClusters(sample, this);
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

                this.EvaluateSwap();
                this.Swap();

                // count most discriminating features space etc.
                this.CountC();
                this.CountGSOManifold();
                this.CountMDFOfVectors();
                this.CountMDFMeans();
                this.CountCovarianceMatricesMDF();
                
                //dispose matrices
                //this.DisposeCovarianceMatrices();

                //count cov matrix mean
                this.CountCovarianceMatrixMeanMDF();
            }
        }

        #endregion

        #region Update Node

        private void UpdateClusters(Sample sample)
        {
            // parameters bly bound of number of y clusters in node, dy resolution
            // find nearest xj cluster using euclidean distance 

            ClusterPair nearestCluster = null;
            ClusterPair nearestCluster2 = null;
            ClusterPair nearestCluster3 = null;
            ClusterPair nearestCluster4 = null;
            double distance = double.MaxValue;
            double distance2 = double.MaxValue;
            double distance3 = double.MinValue;
            int index1 = -1;
            int index2 = -1;
            int index3 = -1;
            int index4 = -1;
            
            
            nearestCluster = this.GetNearestClusterPairY(sample, out distance, out index1);
            
            //nearestCluster2 = this.GetNearestClusterPairXMDF(sample, out distance2, out index2);
            //nearestCluster3 = this.GetNearestClusterPairXMDFProbability(sample, out distance3, out index3);
            //nearestCluster4 = this.GetNearestClusterPairXGaussian(sample, out distance, out index4);

            //Console.WriteLine("Distance1 : " + distance.ToString());
            //Console.WriteLine("Distance2 : " + distance3.ToString());

            //Console.WriteLine("Index1 : " + index2.ToString());
            //Console.WriteLine("Index2 : " + index3.ToString());

            //if (index2 != index3)
            //{
            //    differentReturnedCluster++;
            //    Console.WriteLine("Increment diff clusters" + differentReturnedCluster.ToString());
            //}
            //else
            //{
            //    Console.WriteLine("The same clusters.");
            //}

            //Console.WriteLine(distance.ToString());
            // if is count < like bly and distance > deltay create new cluster
            // add new cluster pair (x,y), increment n
            if (samples.Count < Params.bly && distance > Params.deltaY)
            {
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
                //Update a certain portion p (e.g., p = 0:2, i.e., pulling top 20%) of nearest clusters using the amnesic average
                //explained in Section III-F and return the index j
                // add sample to clusters, update statistics of clusters
                nearestCluster.X.AddItemNonLeaf(sample.X, this);
                nearestCluster.Y.AddItem(sample.Y);
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
            return 2 * (this.samples.Count - Params.q) / Math.Pow(Params.q, 2);
        }

        #region Swapping

        private void EvaluateSwap()
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
                //Console.WriteLine("Center like cluster: " + r.ToString());
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

                //Console.WriteLine("Assign " + clPair.Id.ToString() + " to " + clPair.CurrentCenter.ToString());
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

            for (int i = 0; i < Params.q; i++)
            {
                // create node + set save path
                Node node = new Node(this, this.path + "node" + (children.Count + 1).ToString() + @"\");
                List<ClusterPair> clPairs = this.clusterPairs.Where(cp => cp.CurrentCenter == i).ToList();

                // set id of node
                node.Id = this.children.Count + 1;

                foreach (var item in clPairs)
                {
                    ClusterPair clusterPair = item.GetClone();
                    node.AddClusterPair(clusterPair);
                }

                // set parent to clusters X
                node.SetParentToXClusters();

                // count covariance patrix mean for probability counting
                node.CountCovarianceMatrixMean();

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
            foreach (var item in clustersX)
            {
                item.CountMDFMean();
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

        /// <summary>
        /// count MDF means in all clusters
        /// </summary>
        public void DisposeCovarianceMatrices()
        {
            foreach (var item in clustersX)
            {
                item.DisposeCovMatrix();
            }
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
                    double MDF = vector.GetMDFDistance(startPoint.MostDiscrimatingFeatures);
                    //Console.WriteLine("Normal " + i.ToString() + " : "+ normal.ToString());
                    //Console.WriteLine("MDF " + i.ToString() + " : " + MDF.ToString());
                }
            }
        }

        private void UpdatePlasticityOfParents(Node node)
        {
            Node n = node;
            for (int i = 0; i <= Params.l; i++)
            {
                if (n.parent != null)
                {
                    n = n.parent;
                    if (i == Params.l)
                    {
                        n.IsPlastic = false;
                    }
                }
                else
                {
                    return;
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

        public Node GetNextByPropability(Sample sample)
        {
            if (children.Count > 0)
            {
                double distance = double.MaxValue;
                Node result = children[0];

                foreach (var node in children)
                {
                    if (node.IsLeafNode == true)
                    {
                        int clusterIndex = 1;
                        foreach (var cluster in node.ClustersX)
                        {
                            double tmpDistance = cluster.GetSDNLL(sample.X.ToArray());

                            //Console.WriteLine(String.Format("P : N : {0}, C:{1}, D:{2}", node.Id, clusterIndex, tmpDistance.ToString()));
                            clusterIndex++;

                            if (tmpDistance < distance)
                            {
                                distance = tmpDistance;
                                result = node;
                            }
                        }
                    }
                    else
                    {
                        foreach (var cluster in node.ClustersX)
                        {
                            double tmpDistance = cluster.GetSDNLL_MDF(sample.X.ToArray(), this.gSOManifold, this.c);
                            if (tmpDistance < distance)
                            {
                                distance = tmpDistance;
                                result = node;
                            }
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

        #region Grand Schmidt Ortogonalisation Process

        public void CountC()
        {
            this.c = this.GetCFromClustersX().ToArray();
        }

        public void CountGSOManifold()
        {
            // get scatter vecrtors, count = q-1, we ignore cluster with less samples
            List<Vector> scatterVectors = this.GetScatterVectors();
            gSOManifold = this.GetManifold(scatterVectors);
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
                    ILArray<double> newVector = scatterVectors[i].ToArray();
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
                        ILArray<double> si = scatterVectors[i].ToArray();
                        si = si.T;

                        ILArray<double> siaj = ILMath.multiply(si, ortBasisVectors[j]);
                        incrementalPart = siaj[0] * ortBasisVectors[j];

                        // count divider
                        projectionW = projectionW + incrementalPart;
                    }

                    ILArray<double> scatterVector = ILMath.array<double>(scatterVectors[i].ToArray());
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
            return Vector.GetMeanOfVectors(clustersX.Select(cl => cl.Mean).ToList<Vector>());
        }

        public List<Vector> GetScatterVectors()
        {
            List<Vector> scatterVectors = new List<Vector>(clustersX.Count);
            
            Vector C = this.GetCFromClustersX();

            foreach (var item in clustersX)
            {
                Vector newScatter = new Vector(item.Mean.ToArray());
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
            scatterVectors.RemoveAt(index);
            return scatterVectors;
        }

        #endregion

        #region Covariance Matrix Mean 

        public void CountCovarianceMatrixMean()
        {
            ILArray<double> mean = ILMath.zeros(Params.inputDataDimension, Params.inputDataDimension);

            foreach (var item in clustersX)
            {
                mean = mean + item.CovMatrix;
            }

            mean = mean / this.clustersX.Count;
            this.covarianceMatrixMean = mean;
        }

        public void CountCovarianceMatrixMeanMDF()
        {
            ILArray<double> mean = ILMath.zeros(this.clustersX.Count - 1, this.ClustersX.Count - 1);

            foreach (var item in clustersX)
            {
                mean = mean + item.CovMatrixMDF;
            }

            mean = mean / this.clustersX.Count;
            this.covarianceMatrixMeanMDF = mean;
        }

        #endregion

    }
}
