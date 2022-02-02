using System;
using System.Collections.Generic;

namespace dge
{
    public class SceneNode : I_SceneNode
    {
        protected string s_name;
        protected Dictionary<int,I_SceneNode> Nodes;
        public event EventHandler<dgtk.dgtk_OnUpdateEventArgs> UpdateEvent;
        public SceneNode(string name)
        {
            this.s_name = name;
            this.Nodes = new Dictionary<int, I_SceneNode>();
            this.UpdateEvent += delegate {};
        }

        internal void InternalUpdate()
        {
            this.Update();
            this.UpdateEvent(this, new dgtk.dgtk_OnUpdateEventArgs());
        }

        protected virtual void Update()
        {

        }
        internal virtual void DrawIDs()
        {
            foreach (I_SceneNode node in this.Nodes.Values)
            {
                node.DrawIDs();
            }
        }



        public string Name
        {
            set { this.s_name = value; }
            get { return this.s_name; }
        }
    }

}