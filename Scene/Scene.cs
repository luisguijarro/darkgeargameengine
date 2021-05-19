using System;
using System.Collections.Generic;

using dgtk.OpenGL;

namespace dge
{

    public class Scene : Scenenode
    {
        protected string s_name;
        protected dgWindow parentWin;
        protected dgtk.Graphics.Color4 c4_BackGroundColor;
        public Scene(string name) : base()
        {
            this.s_name = name;
        }
        internal void InternalDraw()
        {
            GL.glClearColor(this.c4_BackGroundColor);
            GL.glClear(ClearBufferMask.GL_ALL);
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