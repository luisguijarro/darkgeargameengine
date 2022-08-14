using System;
using dgtk.OpenGL;
using System.Runtime.InteropServices;

namespace dge.GLSL
{
    public class ShaderGL : I_Shader
    {
        internal uint ui_id;
        private uint ui_vs;
        private uint ui_fs;
        private uint ui_gs;

        public ShaderGL(string vs, string fs, string gs)
        {
            CompileShader(vs, fs, gs);
        }

        private unsafe void CompileShader(string vs, string fs, string gs)
        {
            ui_vs = GL.glCreateShader(ShaderType.GL_VERTEX_SHADER);
            ui_fs = GL.glCreateShader(ShaderType.GL_FRAGMENT_SHADER);
            ui_gs = (gs == null) ? 0: GL.glCreateShader(ShaderType.GL_GEOMETRY_SHADER);

            int glResult = 0;

            GL.glShaderSource(ui_vs, vs);
            GL.glCompileShader(ui_vs);

            #if DEBUG
            glResult = GL.glGetShaderiv(ui_vs, ShaderParameterName.GL_COMPILE_STATUS);            
            if (glResult<=0)
            {          
                string s_log = GL.glGetShaderInfoLog(ui_vs);
                Console.WriteLine(((ErrorCode)glResult).ToString()+" (vs): "+s_log);
            }
            #endif

            GL.glShaderSource(ui_fs, fs);
            GL.glCompileShader(ui_fs);

            #if DEBUG
            glResult = GL.glGetShaderiv(ui_fs, ShaderParameterName.GL_COMPILE_STATUS);            
            if (glResult<=0)
            {          
                string s_log = GL.glGetShaderInfoLog(ui_fs);
                Console.WriteLine(((ErrorCode)glResult).ToString()+" (fs): "+s_log);
            }
            #endif

            this.ui_id = GL.glCreateProgram();
            GL.glAttachShader(this.ui_id, ui_vs);
            GL.glAttachShader(this.ui_id, ui_fs);

            if (gs != null) 
            {
                GL.glShaderSource(ui_gs, gs);
                GL.glCompileShader(ui_gs);

                #if DEBUG
                glResult = GL.glGetShaderiv(ui_gs, ShaderParameterName.GL_COMPILE_STATUS);  
                if (glResult<=0)
                {          
                    string s_log = GL.glGetShaderInfoLog(ui_gs);
                    Console.WriteLine(((ErrorCode)glResult).ToString()+" (gs): "+s_log);
                }
                #endif


                GL.glAttachShader(this.ui_id, ui_gs);
            }

            GL.glLinkProgram(this.ui_id);

            GL.glDeleteShader(ui_vs);
            GL.glDeleteShader(ui_fs);
            if (gs != null) { GL.glDeleteShader(ui_gs); }
            
        }

        public string GetLog()
        {
            this.Use();
            string vs_log = "";
            string fs_log = "";
            string gs_log = "";

            vs_log = GL.glGetShaderInfoLog(this.ui_vs);
            Console.WriteLine("Shader Log (vs): "+vs_log);

            fs_log = GL.glGetShaderInfoLog(this.ui_fs);
            Console.WriteLine("Shader Log (fs): "+fs_log);

            if (this.ui_gs > 0)
            {
                gs_log = GL.glGetShaderInfoLog(this.ui_gs);
                Console.WriteLine("Shader Log (gs): "+gs_log);
            }

            return ("Shader Log (vs): "+vs_log+System.Environment.NewLine) + ("Shader Log (fs): "+fs_log+System.Environment.NewLine) + ((this.ui_gs > 0) ? ("Shader Log (gs): "+gs_log+System.Environment.NewLine) : "");
        }

        public void Use()
        {
            GL.glUseProgram(this.ui_id);
        }

        public uint ID
        {
            get { return this.ui_id;}
        }
    }
}