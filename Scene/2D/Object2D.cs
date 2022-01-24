using System;
using System.Collections.Generic;

using dgtk.Graphics;
using dgtk.Math;
using dge.G2D;

namespace dge
{
    public class Object2D
    {
        Vector3 v3_position;
        Size s2_size;
        float f_rotation;
        
        protected TextureBufferObject tbo;
        protected Color4 c4_color;
        private uint ui_id;
        private Color4 c4_idColor;

        public Object2D(TextureBufferObject tbo) : this(0, 0, 0, tbo) {}
        public Object2D(float x, float y, float depth) : this(x, y, depth, TextureBufferObject.Null) {}

        public Object2D(float x, float y, float depth, TextureBufferObject tbo)
        {
            this.ui_id = dge.Core2D.GetID();
            this.v3_position = new Vector3(x, y, depth);
            this.s2_size = new Size(tbo.Width, tbo.Height);
            this.tbo = tbo;
        }

        internal void Draw(Drawer drw, Scene2DNode node)
        {
            if (node.Lights.Count>0)
            {
                Light2D[] lights = new Light2D[node.Lights.Count];
                node.Lights.Values.CopyTo(lights, 0);
                if (this.tbo.ID != TextureBufferObject.Null.ID)
                {
                    drw.Draw(this.tbo.ID, (int)v3_position.X, (int)v3_position.Y, (int)v3_position.Z, s2_size.Width, s2_size.Height, f_rotation, lights);
                }
                else
                {
                    drw.Draw(this.c4_color, (int)v3_position.X, (int)v3_position.Y, (int)v3_position.Z, s2_size.Width, s2_size.Height, f_rotation, lights);
                }
            }
            else
            {
                if (this.tbo.ID != TextureBufferObject.Null.ID)
                {
                    drw.Draw(this.tbo.ID, (int)v3_position.X, (int)v3_position.Y, (int)v3_position.Z, s2_size.Width, s2_size.Height, f_rotation);
                }
                else
                {
                    drw.Draw(this.c4_color, (int)v3_position.X, (int)v3_position.Y, (int)v3_position.Z, s2_size.Width, s2_size.Height, f_rotation);
                }
            }
        }

        internal void DrawID()
        {
            //dge.G2D.IDsDrawer.DrawGL2D(c4_idColor, )
        }

        public uint ID
        {
            get { return this.ui_id; }
        }

        public Vector3 Position
        {
            set { this.v3_position = value; }
            get { return this.v3_position; }
        }

        public TextureBufferObject TBO
        {
            set { this.tbo = value; }
            get { return this.tbo; }
        }

        public Color4 Color
        {
            set { this.c4_color = value; }
            get { return this.c4_color; }
        }
    } 
}