using System;
using System.Collections.Generic;

namespace dge
{

    public class Scene : Scenenode
    {
        protected string s_name;
        protected dgWindow parentWin;
        public Scene(string name) : base()
        {
            this.s_name = name;
        }
        internal void InternalDraw()
        {
            this.Draw();
        }
        protected virtual void Draw() // Dibuja la escena.
        {

        }
        internal virtual void SetParentWindow(dgWindow win)
        {
            parentWin = win;
        }
        public string Name
        {
            get { return this.s_name; }
        }
    }

}