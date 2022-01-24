using System;
using System.Collections.Generic;

using dgtk.Graphics;
using dgtk.Math;

namespace dge
{
    public class Light2D 
    {
        bool b_IsDirectional;
        float f_Light_range;
        float f_Source_Sine;
        Color4 c4_LightColor;
        Vector3 v3_Position;
        float f_Rotation; //in Z Axis
        float f_OpeningAngle;
        internal uint ui_id;

        public Light2D(float x, float y, float lightRange, float sourceShine, Color4 lightColor) : this(x, y, 0, lightRange, sourceShine, lightColor) {}

        public Light2D(float x, float y, float z, float lightRange, float sourceShine, dgtk.Graphics.Color4 lightColor)
        {
            this.b_IsDirectional = false;
            this.f_Light_range = lightRange;
            this.f_Source_Sine = sourceShine;
            this.c4_LightColor = lightColor;
            this.v3_Position = new Vector3(x, y, z);
            this.f_Rotation = 0f; // degrees
            this.f_OpeningAngle = 30; // degrees
            this.ui_id = Core2D.GetLight2DID();
        }
        ~Light2D()
        {
            Core2D.ReleaseLight2DID(this.ui_id);
        }

        public bool IsDirectional
        {
            set { this.b_IsDirectional = value; }
            get { return this.b_IsDirectional; }
        }
        public float LightRange
        {
            set { this.f_Light_range = value; }
            get { return this.f_Light_range; }
        }
        public float SourceShine
        {
            set { this.f_Source_Sine = value; }
            get { return this.f_Source_Sine; }
        }
        public Color4 LightColor
        {
            set { this.c4_LightColor = value; }
            get { return this.c4_LightColor; }
        }
        public Vector3 Position
        {
            set { this.v3_Position = value; }
            get { return this.v3_Position; }
        }
        public float Rotation
        {
            set { this.f_Rotation = value; }
            get { return this.f_Rotation; }
        }
        public float OpeningAngle 
        {
            set { this.f_OpeningAngle = value; }
            get { return this.f_OpeningAngle; }
        }

        public uint ID 
        {
            get { return this.ui_id; }
        }
    }
}