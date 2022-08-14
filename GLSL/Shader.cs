using System;
using dgtk.OpenGL;
using System.Runtime.InteropServices;

namespace dge.GLSL
{
    public class Shader
    {
        private I_Shader Instance;
        public Shader(string vs, string fs, bool IsGLES) : this(vs,fs,null, IsGLES)
        {
            
        }

        public Shader(string vs, string fs, string gs, bool IsGLES)
        {
            if (IsGLES)
            {
                this.Instance = new ShaderGLES(vs, fs, gs);
            }
            else
            {
                this.Instance = new ShaderGL(vs, fs, gs);
            }
        }

        public string GetLog()
        {
            return Instance.GetLog();
        }

        public void Use()
        {
            Instance.Use();
        }

        public uint ID
        {
            get { return this.Instance.ID; }
        }
    }
}