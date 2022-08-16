using System;
using System.Collections.Generic;
//using System.Runtime.InteropServices;

using dgtk.Math;
using dgtk.Graphics;
using dgtk.OpenGL;
using dge.GLSL;

namespace dge.G2D
{   
    public partial class Drawer
    {
        private partial class DrawerGLES : I_Drawer
        {
            
            #region Uniforms Ids
            private int idUniformTColor_L; // ID de Uniform de Color transparente.
            private int idUniformColor_L; // ID de Uniform de color multiplicante.    
            private int idUniform_texcoords_L; // ID de Uniform de Coordenadas de Textura Dinamicas.
            private int idUniform_v_size_L; // ID de Uniform de Tamaño de superficie de dibujo.
            private int idUniformMat_View_L; // ID de Uniform que contiene la matriz de Projección.
            private int idUniformMat_Per_L; // ID de Uniform que contiene la matriz de Perspectiva.
            private int idUniformMat_Tra_L; // ID de Uniform que contiene la matriz de Perspectiva.
            private int idUniformSilhouette_L; // ID de Uniform que contiene la matriz de Transformación.
            private int idUniformTexturePassed_L; // Id de Uniform que indica si se está pasando textura o no.
            private int idUniformGlobalLightColor_L; // ID de Uniform que contiene el color de la iluminación global;
            private int idUniformlightsPosRange; // ID de Uniform que contiene Posicion y alcance de las Luces
            private int idUniformlightsColor; // ID de Uniform que contiene el Color de las Luces
            private int idUniformlightRotationAngle; // ID de Uniform que contiene si es omnidireccional o no, el angulo de rotación y el angulo de apertura.
            private int idUniformN_luces;

            #endregion

            private Shader BasicShader_L;

            internal void InitLightShader()
            {
                Console.WriteLine("DrawerWithLights Shader Compilation.");
                BasicShader_L = new Shader(ShadersSourcesGLES.Basic2DIlluminatedvs, ShadersSourcesGLES.Basic2DIlluminatedfs, true);

                idUniform_texcoords_L = GLES.glGetUniformLocation(BasicShader_L.ID, "utexcoords");
                idUniform_v_size_L = GLES.glGetUniformLocation(BasicShader_L.ID, "v_size");
                idUniformTColor_L = GLES.glGetUniformLocation(BasicShader_L.ID, "tColor");
                idUniformColor_L = GLES.glGetUniformLocation(BasicShader_L.ID, "Color");
                idUniformMat_View_L= GLES.glGetUniformLocation(BasicShader_L.ID, "view");
                idUniformMat_Per_L = GLES.glGetUniformLocation(BasicShader_L.ID, "perspective");
                idUniformMat_Tra_L = GLES.glGetUniformLocation(BasicShader_L.ID, "trasform");
                idUniformSilhouette_L = GLES.glGetUniformLocation(BasicShader_L.ID, "Silhouette");
                idUniformTexturePassed_L = GLES.glGetUniformLocation(BasicShader_L.ID, "TexturePassed");
                idUniformGlobalLightColor_L = GLES.glGetUniformLocation(BasicShader_L.ID, "GlobalLightColor");
                idUniformlightsPosRange = GLES.glGetUniformLocation(BasicShader_L.ID, "lightsPosRange");
                idUniformlightsColor = GLES.glGetUniformLocation(BasicShader_L.ID, "lightsColor");
                idUniformlightRotationAngle = GLES.glGetUniformLocation(BasicShader_L.ID, "lightRotationAngle");
                idUniformN_luces = GLES.glGetUniformLocation(BasicShader_L.ID, "n_luces");
            }

            public void DrawGL(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width / 2f, height / 2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.

                BasicShader_L.Use();
                //GL.glUniform4fv(idUniform_texcoords_L, 1, new float[]{0f, 0f, 1f, 1f});
                GLES.glUniform2f(idUniform_v_size_L, width, height);
                GLES.glUniform1i(idUniformSilhouette_L, 0);
                GLES.glUniform1i(idUniformTexturePassed_L, 0);
                GLES.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
                this.SendUniformLights(lights);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }
            
            public void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width/2f, height/2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
                
                GLES.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader_L.Use();
                GLES.glUniform4fv(idUniform_texcoords_L, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
                GLES.glUniform2f(idUniform_v_size_L, width, height);
                GLES.glUniform1i(idUniformSilhouette_L, Silhouette);
                GLES.glUniform1i(idUniformTexturePassed_L, 1);
                GLES.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
                this.SendUniformLights(lights);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }

            public void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(rotateX, rotateY));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
                
                GLES.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader_L.Use();
                GLES.glUniform4fv(idUniform_texcoords_L, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
                GLES.glUniform2f(idUniform_v_size_L, width, height);
                GLES.glUniform1i(idUniformSilhouette_L, Silhouette);
                GLES.glUniform1i(idUniformTexturePassed_L, 1);
                GLES.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GLES.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
                this.SendUniformLights(lights);
                GLES.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
                GLES.glBindVertexArray(VAO);
                GLES.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GLES.glBindVertexArray(0);
            }

            public void SendUniformLights(Light2D[] lights)
            {
                List<float> lightsPosRange = new List<float>();
                List<float> lightsColor = new List<float>();
                List<float> lightRotationAngle = new List<float>();
                for (int i=0;i<lights.Length;i++)
                {
                    lightsPosRange.AddRange(lights[i].Position.ToArray());
                    lightsPosRange.Add(lights[i].LightRange);

                    lightsColor.AddRange(lights[i].LightColor.ToRgbaFloatArray());

                    lightRotationAngle.Add(lights[i].IsDirectional ? 1f : 0f);
                    lightRotationAngle.Add(lights[i].Rotation);
                    lightRotationAngle.Add(lights[i].OpeningAngle);
                }
                GLES.glUniform1i(idUniformN_luces, lightsPosRange.Count);
                GLES.glUniform4fv(idUniformlightsPosRange, lights.Length, lightsPosRange.ToArray());
                GLES.glUniform4fv(idUniformlightsColor, lights.Length, lightsColor.ToArray());
                GLES.glUniform4fv(idUniformlightRotationAngle, lights.Length, lightRotationAngle.ToArray());
            }
        }
    }
}