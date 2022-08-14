using System;
using dgtk.OpenGL;
using System.Runtime.InteropServices;

namespace dge.GLSL
{
    public class ShaderGLES : I_Shader
    {
        internal uint ui_id;
        private uint ui_vs;
        private uint ui_fs;
        private uint ui_gs;

        public ShaderGLES(string vs, string fs, string gs)
        {
            CompileShader(vs, fs, gs);
        }

        private unsafe void CompileShader(string vs, string fs, string gs)
        {
            ui_vs = GLES.glCreateShader(ShaderType.GL_VERTEX_SHADER);
            ui_fs = GLES.glCreateShader(ShaderType.GL_FRAGMENT_SHADER);
            ui_gs = (gs == null) ? 0: GLES.glCreateShader(ShaderType.GL_GEOMETRY_SHADER);

            int glResult = 0;

            GLES.glShaderSource(ui_vs, vs);
            GLES.glCompileShader(ui_vs);

            #if DEBUG
            glResult = GLES.glGetShaderiv(ui_vs, ShaderParameterName.GL_COMPILE_STATUS);            
            if (glResult<=0)
            {          
                string s_log = GLES.glGetShaderInfoLog(ui_vs);
                Console.WriteLine(((ErrorCode)glResult).ToString()+" (vs): "+s_log);
            }
            #endif

            GLES.glShaderSource(ui_fs, fs);
            GLES.glCompileShader(ui_fs);

            #if DEBUG
            glResult = GLES.glGetShaderiv(ui_fs, ShaderParameterName.GL_COMPILE_STATUS);            
            if (glResult<=0)
            {          
                string s_log = GLES.glGetShaderInfoLog(ui_fs);
                Console.WriteLine(((ErrorCode)glResult).ToString()+" (fs): "+s_log);
            }
            #endif

            this.ui_id = GLES.glCreateProgram();
            GLES.glAttachShader(this.ui_id, ui_vs);
            GLES.glAttachShader(this.ui_id, ui_fs);

            if (gs != null) 
            {
                GLES.glShaderSource(ui_gs, gs);
                GLES.glCompileShader(ui_gs);

                #if DEBUG
                glResult = GLES.glGetShaderiv(ui_gs, ShaderParameterName.GL_COMPILE_STATUS);  
                if (glResult<=0)
                {          
                    string s_log = GLES.glGetShaderInfoLog(ui_gs);
                    Console.WriteLine(((ErrorCode)glResult).ToString()+" (gs): "+s_log);
                }
                #endif


                GLES.glAttachShader(this.ui_id, ui_gs);
            }

            GLES.glLinkProgram(this.ui_id);

            GLES.glDeleteShader(ui_vs);
            GLES.glDeleteShader(ui_fs);
            if (gs != null) { GLES.glDeleteShader(ui_gs); }

        }

        public string GetLog()
        {
            this.Use();
            string vs_log = "";
            string fs_log = "";
            string gs_log = "";
            
            vs_log = GLES.glGetShaderInfoLog(this.ui_vs);
            Console.WriteLine("Shader Log (vs): "+vs_log);

            fs_log = GLES.glGetShaderInfoLog(this.ui_fs);
            Console.WriteLine("Shader Log (fs): "+fs_log);
            
            if (this.ui_gs > 0)
            {
                gs_log = GLES.glGetShaderInfoLog(this.ui_gs);
                Console.WriteLine("Shader Log (gs): "+gs_log);
            }            

            return ("Shader Log (vs): "+vs_log+System.Environment.NewLine) + ("Shader Log (fs): "+fs_log+System.Environment.NewLine) + ((this.ui_gs > 0) ? ("Shader Log (gs): "+gs_log+System.Environment.NewLine) : "");
        }

        public void Use()
        {
            GLES.glUseProgram(this.ui_id);
        }

        public uint ID
        {
            get { return this.ui_id;}
        }
    }
}