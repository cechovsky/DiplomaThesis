using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHDRLib
{
    public class Tree
    {
        private Node root;
        private bool isEmpty;
    
        public Tree()
        {
            root = new Node();
            isEmpty = true;
        }

        public Node Root
        {
            get
            {
                return root;
            }            
        }

        public bool IsActive
        {
            get
            {
                return this.IsActive;
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
    }
}
