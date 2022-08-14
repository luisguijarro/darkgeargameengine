using System;
using dgtk.OpenGL;
using System.Runtime.InteropServices;

namespace dge.GLSL
{
    interface I_Shader
    {
        //unsafe void CompileShader(string vs, string fs, string gs);

        string GetLog();

        void Use();

        uint ID { get; }
    }
}