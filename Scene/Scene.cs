using System;
using System.Collections.Generic;

using dgtk.OpenGL;

namespace dge
{
    public class Scene : SceneNode
    {
        protected dgWindow parentWin;
        protected dgtk.Graphics.Color4 c4_BackGroundColor;
        public event EventHandler<ParentWindowSettedEventArgs> ParentWindowSetted;
        public Scene(string name) : base(name)
        {
            this.ParentWindowSetted += delegate{};
            this.c4_BackGroundColor = dgtk.Graphics.Color4.Black;
        }
        internal virtual void InternalDraw(dge.G2D.Drawer drawer)
        {
            GL.glClearColor(this.c4_BackGroundColor);
            GL.glClear(ClearBufferMask.GL_ALL);
            this.Draw(drawer);
        }
        protected virtual void Draw(dge.G2D.Drawer drawer) // Dibuja la escena.
        {

        }

        internal void SetParentWindow(dgWindow win)
        {
            this.parentWin = win;
            this.ParentWindow_Setted(win);
            this.ParentWindowSetted(this, new ParentWindowSettedEventArgs(this.parentWin));
        }

        protected virtual void ParentWindow_Setted(dgWindow window)
        {

        }

        internal virtual void RemParentWindow()
        {
            this.parentWin = null;
        }
    }

}