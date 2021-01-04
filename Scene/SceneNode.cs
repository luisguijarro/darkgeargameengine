using System;
using System.Collections.Generic;

namespace dge
{
    public class Scenenode
    {
        Dictionary<int,Scenenode> Nodes;
        public Scenenode()
        {
            this.Nodes = new Dictionary<int, Scenenode>();
        }
    }

}