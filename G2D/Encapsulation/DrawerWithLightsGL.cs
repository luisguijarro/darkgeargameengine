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
        private partial class DrawerGL : I_Drawer
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
                BasicShader_L = new Shader(ShadersSourcesGL.Basic2DIlluminatedvs, ShadersSourcesGL.Basic2DIlluminatedfs, false);

                idUniform_texcoords_L = GL.glGetUniformLocation(BasicShader_L.ID, "utexcoords");
                idUniform_v_size_L = GL.glGetUniformLocation(BasicShader_L.ID, "v_size");
                idUniformTColor_L = GL.glGetUniformLocation(BasicShader_L.ID, "tColor");
                idUniformColor_L = GL.glGetUniformLocation(BasicShader_L.ID, "Color");
                idUniformMat_View_L= GL.glGetUniformLocation(BasicShader_L.ID, "view");
                idUniformMat_Per_L = GL.glGetUniformLocation(BasicShader_L.ID, "perspective");
                idUniformMat_Tra_L = GL.glGetUniformLocation(BasicShader_L.ID, "trasform");
                idUniformSilhouette_L = GL.glGetUniformLocation(BasicShader_L.ID, "Silhouette");
                idUniformTexturePassed_L = GL.glGetUniformLocation(BasicShader_L.ID, "TexturePassed");
                idUniformGlobalLightColor_L = GL.glGetUniformLocation(BasicShader_L.ID, "GlobalLightColor");
                idUniformlightsPosRange = GL.glGetUniformLocation(BasicShader_L.ID, "lightsPosRange");
                idUniformlightsColor = GL.glGetUniformLocation(BasicShader_L.ID, "lightsColor");
                idUniformlightRotationAngle = GL.glGetUniformLocation(BasicShader_L.ID, "lightRotationAngle");
                idUniformN_luces = GL.glGetUniformLocation(BasicShader_L.ID, "n_luces");
            }

            public void DrawGL(Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, Light2D[] lights)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width / 2f, height / 2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.

                BasicShader_L.Use();
                //GL.glUniform4fv(idUniform_texcoords_L, 1, new float[]{0f, 0f, 1f, 1f});
                GL.glUniform2f(idUniform_v_size_L, width, height);
                GL.glUniform1i(idUniformSilhouette_L, 0);
                GL.glUniform1i(idUniformTexturePassed_L, 0);
                GL.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R); // Transmitimos al Shader la trasformación.
                GL.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
                this.SendUniformLights(lights);
                GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);
                GL.glBindVertexArray(VAO);
                GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GL.glBindVertexArray(0);
            }
            
            public void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(width/2f, height/2f));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
                
                GL.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader_L.Use();
                GL.glUniform4fv(idUniform_texcoords_L, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
                GL.glUniform2f(idUniform_v_size_L, width, height);
                GL.glUniform1i(idUniformSilhouette_L, Silhouette);
                GL.glUniform1i(idUniformTexturePassed_L, 1);
                GL.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GL.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
                this.SendUniformLights(lights);
                GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
                GL.glBindVertexArray(VAO);
                GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GL.glBindVertexArray(0);
            }

            public void DrawGL(uint tboID, Color4 color, int x, int y, int depth, int width, int height, int rotateX, int rotateY, float RotInDegrees, float Texcoord0x, float Texcoord0y, float Texcoord1x, float Texcoord1y, int Silhouette, Light2D[] lights)
            {
                dgtk.Math.Mat4 m4R = dgtk.Math.MatrixTools.TwistAroundPoint2D((b_invert_y ? -RotInDegrees : RotInDegrees), new Vector2(rotateX, rotateY));
                dgtk.Math.Mat4 m4T = dgtk.Math.MatrixTools.MakeTraslationMatrix(new Vector3(x, y, depth)); // Creamos la Matriz de traslación.
                
                GL.glEnable(EnableCap.GL_TEXTURE_2D);
                
                BasicShader_L.Use();
                GL.glUniform4fv(idUniform_texcoords_L, 1, new float[]{Texcoord0x, Texcoord0y, Texcoord1x, Texcoord1y});
                GL.glUniform2f(idUniform_v_size_L, width, height);
                GL.glUniform1i(idUniformSilhouette_L, Silhouette);
                GL.glUniform1i(idUniformTexturePassed_L, 1);
                GL.glUniformMatrix(idUniformMat_Tra_L, dgtk.OpenGL.Boolean.GL_FALSE, m4T * m4R ); // Transmitimos al Shader la trasformación.
                GL.glUniform4f(idUniformColor_L, color.R, color.G, color.B, color.A);
                this.SendUniformLights(lights);
                GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, tboID);
                GL.glBindVertexArray(VAO);
                GL.glDrawElements(PrimitiveType.GL_TRIANGLES, 6, DrawElementsType.GL_UNSIGNED_INT, new IntPtr(0));
                GL.glBindVertexArray(0);
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
                GL.glUniform1i(idUniformN_luces, lightsPosRange.Count);
                GL.glUniform4fv(idUniformlightsPosRange, lights.Length, lightsPosRange.ToArray());
                GL.glUniform4fv(idUniformlightsColor, lights.Length, lightsColor.ToArray());
                GL.glUniform4fv(idUniformlightRotationAngle, lights.Length, lightRotationAngle.ToArray());
            }
        }
    }
}