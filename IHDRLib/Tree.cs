using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IHDRLib
{
    [Serializable()]
    public class Tree : ISerializable
    {
        private Node root;
        private bool isEmpty;
    
        public Tree()
        {
            root = new Node(Params.deltaX, Params.deltaY);
            isEmpty = true;
        }

        public Node Root
        {
            get
            {
                return root;
            }            
        }

        /// <summary>
        /// Update tree with sample
        /// </summary>
        /// <param name="sample">added sample to tree</param>
        public void UpdateTree(Sample sample)
        {
            this.root.UpdateNode(sample);
        }

        public void SaveToFileHierarchy()
        {
            if (this.root != null)
            {
                this.root.SaveToFileHierarchy();
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("root", root, typeof(Node));
            info.AddValue("isEmpty", isEmpty, typeof(bool));
        }

        // The special constructor is used to deserialize values. 
        public Tree(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            root = (Node)info.GetValue("root", typeof(Node));
            isEmpty = (bool)info.GetValue("isEmpty", typeof(bool));
        }

        public void EvaluateClustersLabels()
        {
            if (this.root != null)
            {
                this.root.EvaluateClustersLabels();
            }
        }


        public TestResult GetLabelOfCategory(Sample item)
        {
            return this.root.GetLabelOfCategory(item);
        }

        public TestResult GetTestResultByWidthSearch(Sample item)
        {
            ClusterPair resultClusterPair = root.GetTestResultByWidthSearch(item);

            return new TestResult()
            {
                ClusterMeanX = resultClusterPair.X.Mean,
                ClusterMeanY = resultClusterPair.Y.Mean,
                Label = resultClusterPair.X.Label
            };
        }
    }
}
