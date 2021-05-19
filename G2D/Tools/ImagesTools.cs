using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

using dge.Tools;

using dgtk.OpenGL;

namespace dge.G2D
{
    public static partial class Tools
    {
        internal static Dictionary<string, TextureBufferObject> d_imageshash = new Dictionary<string, TextureBufferObject>();

        public static TextureBufferObject LoadImage(string filepath)
        {
            string s_hash = FileTools.MD5FromFile(filepath);

            if (d_imageshash.ContainsKey(s_hash))
            {
                return d_imageshash[s_hash];
            }
            d_imageshash.Add(s_hash, p_LoadImage(filepath, s_hash));
            return d_imageshash[s_hash];
        }

        public static TextureBufferObject LoadImage(Stream stream, string name)
        {
            string s_hash = name.GetHashCode().ToString();

            if (d_imageshash.ContainsKey(s_hash))
            {
                return d_imageshash[s_hash];
            }
            d_imageshash.Add(s_hash, p_LoadImage(stream, s_hash, name));
            return d_imageshash[s_hash];
        }

        public static unsafe TextureBufferObject LoadImageFromIntPTr(string name, string hash, int Width, int Height, IntPtr Scan0)
		{
            if (d_imageshash.ContainsKey(hash))
            {
                return d_imageshash[hash];
            }
            d_imageshash.Add(hash, p_LoadImageFromIntPTr(name, Width, Height, Scan0));
            return d_imageshash[hash];
        }

        private static TextureBufferObject p_LoadImage(string filepath, string s_hash)
        {
            TextureBufferObject tbo_ret = new TextureBufferObject();
            if (File.Exists(filepath))
			{
                Bitmap bp = new Bitmap(filepath);
                BitmapData bd = bp.LockBits(new Rectangle(0, 0, bp.Size.Width, bp.Size.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
							
				tbo_ret = p_LoadImageFromIntPTr(filepath, bd.Width, bd.Height, bd.Scan0);
							
				bp.UnlockBits(bd);
				bp.Dispose();
				
				return tbo_ret;
            }
            #if DEBUG
            Console.WriteLine("Error: File "+filepath+"not exist.");
            #endif
            return tbo_ret;
        }

        private static TextureBufferObject p_LoadImage(Stream stream, string s_hash, string name)
        {
            TextureBufferObject tbo_ret = new TextureBufferObject();
            if (stream != null)
			{
                Bitmap bp = (Bitmap)(Image.FromStream(stream, true, false));
                BitmapData bd = bp.LockBits(new Rectangle(0, 0, bp.Size.Width, bp.Size.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
							
				tbo_ret = p_LoadImageFromIntPTr(s_hash, bd.Width, bd.Height, bd.Scan0);
							
				bp.UnlockBits(bd);
				bp.Dispose();
				
				return tbo_ret;
            }
            #if DEBUG
            Console.WriteLine("Error: " + name + " Stream is Null.");
            #endif
            return tbo_ret;
        }

        private static unsafe TextureBufferObject p_LoadImageFromIntPTr(string name, int Width, int Height, IntPtr Scan0)
		{
            bool current = dgtk.OpenGL.OGL_SharedContext.MakeCurrent();

            UInt32 idret = 0; //stackalloc uint[1];
            GL.glEnable(EnableCap.GL_TEXTURE_2D);

            idret = GL.glGenTexture();
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, idret);
            GL.glPixelStoref(PixelStoreParameter.GL_UNPACK_ALIGNMENT, 1);

            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MIN_FILTER, (int)TextureMinFilter.GL_LINEAR);
            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MAG_FILTER, (int)TextureMinFilter.GL_NEAREST);
            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_WRAP_S, (int)TextureWrapMode.GL_REPEAT);
            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_WRAP_T, (int)TextureWrapMode.GL_REPEAT);

            GL.glTexEnvi(TextureEnvTarget.GL_TEXTURE_ENV, TextureEnvParameter.GL_TEXTURE_ENV_MODE, (int)TextureEnvMode.GL_REPLACE_EXT);

            GL.glTexImage2D(TextureTarget.GL_TEXTURE_2D, 0, InternalFormat.GL_RGBA, Width, Height, 0, dgtk.OpenGL.PixelFormat.GL_BGRA, PixelType.GL_UNSIGNED_BYTE, Scan0);

            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, 0);

            GL.glFlush();

            return new TextureBufferObject(name, (uint)Width, (uint)Height, idret, name);
        }
        
		public static bool SaveImage(TextureBufferObject tbo, string filepath)
		{
			
			return false;
			
		}
		
		public static bool SaveScreenShot( string filepath)
		{
			
			return false;
			
		}
    }
}