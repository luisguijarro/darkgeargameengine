using System;
using System.Collections.Generic;

namespace dge
{
    public class Scenenode
    {
        Dictionary<int,Scenenode> Nodes;
        Dictionary<int,Ligth> Ligths;
        public Scenenode()
        {
            this.Nodes = new Dictionary<int, Scenenode>();
        }
    }

}