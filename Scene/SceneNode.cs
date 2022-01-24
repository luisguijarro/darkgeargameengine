using System;
using System.Collections.Generic;

namespace dge
{
    public class SceneNode : I_SceneNode
    {
        protected string s_name;
        protected Dictionary<int,I_SceneNode> Nodes;
        public SceneNode(string name)
        {
            this.s_name = name;
            this.Nodes = new Dictionary<int, I_SceneNode>();
        }
        public string Name
        {
            set { this.s_name = value; }
            get { return this.s_name; }
        }
    }

}