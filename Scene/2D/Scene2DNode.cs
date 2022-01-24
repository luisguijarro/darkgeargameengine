using System;
using System.Collections.Generic;

namespace dge
{
    public class Scene2DNode : I_SceneNode
    {        
        private string s_name;
        internal Dictionary<uint, Light2D> Lights;
        internal Dictionary<uint, Object2D> Objects;
        protected Dictionary<int,I_SceneNode> Nodes;
        public Scene2DNode(string name)
        {
            this.s_name = name;
            this.Lights = new Dictionary<uint, Light2D>();
            this.Objects = new Dictionary<uint, Object2D>();
            this.Nodes = new Dictionary<int, I_SceneNode>();
        }

        #region PUBLIC

        public void AddLight(Light2D light)
        {
            this.Lights.Add(light.ID, light);
        }

        public void RemoveLight(Light2D light)
        {
            this.RemoveLight(light.ID);
        }

        public void RemoveLight(uint id)
        {
            if (this.Lights.ContainsKey(id))
            {
                this.Lights.Remove(id);
            }
        }

        public void AddObject(Object2D obj)
        {
            if (!this.Objects.ContainsKey(obj.ID))
            {
                this.Objects.Add(obj.ID, obj);
            }
        }

        public void RemoveObject(Object2D obj)
        {
            this.RemoveObject(obj.ID);
        }

        public void RemoveObject(uint id)
        {
            if (this.Objects.ContainsKey(id))
            {
                this.Objects.Remove(id);
            }
        }

        #endregion

        #region INTERNALS
        internal virtual void InternalDraw(dge.G2D.Drawer drawer)
        {            
            foreach(Object2D obj in this.Objects.Values)
            {
                obj.Draw(drawer, this);
            }
            this.Draw(drawer);
        }
        protected virtual void Draw(dge.G2D.Drawer drawer) // Dibuja la escena.
        {
            
        }

        #endregion
        public string Name 
        {
            set { this.s_name = value; }
            get { return this.s_name; }
        }
    }

}