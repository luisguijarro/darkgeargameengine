using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dgtk.OpenGL;

namespace dge
{    
    
    public static class Core
    {
        internal static object LockObject;
        private static OperatingSystem OS;
        public static OperatingSystem GetOS()
        {
            if (OS == OperatingSystem.None)
            {
                OS = pGetOS();
            }
            return OS;
        }
        
        private static OperatingSystem pGetOS()
		{
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OperatingSystem.Linux;
            } 
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OperatingSystem.MacOS;
            } 
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OperatingSystem.Windows;
            }
            else
            {
                return OperatingSystem.NotSuported;
            }
        }

		public enum OperatingSystem
		{
			None = 0, Windows, Linux, MacOS, NotSuported
		}

        public static System.IO.Stream LoadEmbeddedResource(string resource)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
        }

        public static System.IO.Stream LoadEmbeddedResource(string resource, Assembly assembly)
        {
            return assembly.GetManifestResourceStream(resource);
        }

    }

    public static class Core2D
    {
        internal static uint PixelBufferObject_Select; 
        internal static uint ui_SelectedID;

        #region Surface IDs
        internal static Dictionary<uint, uint> IDS;
        public static uint GetID() // Otorga un ID a objeto que lo solicita
        {
            if (IDS == null)
            {
                IDS = new Dictionary<uint, uint>();
            }
            for (uint i=1;i<IDS.Count+1;i++) // El ID minimo es 1, 0 se deja para los objetos no interactivos.
            {
                if (!IDS.ContainsKey(i))
                {
                    IDS.Add(i,i);
                    return i;
                }
            }
            uint e = (uint)IDS.Count+1; // El ID minimo es 1, 0 se deja para los objetos no interactivos.
            IDS.Add(e,e);
            return e;
        }

        public static bool ReleaseID(uint id) // Liberia ID de objeto y devuelve si ha tenido exito.
        {
            if (IDS == null) { return false; }
            if (IDS.ContainsKey(id))
            {
                IDS.Remove(id);
                return true;
            }
            return false;
        }

        #endregion

        #region Light2D IDs

        internal static Dictionary<uint, uint> LightIDS;
        internal static uint GetLight2DID() // Otorga un ID a objeto que lo solicita
        {
            if (LightIDS == null)
            {
                LightIDS = new Dictionary<uint, uint>();
            }
            for (uint i=1;i<LightIDS.Count+1;i++) // El ID minimo es 1, 0 se deja para los objetos no interactivos.
            {
                if (!LightIDS.ContainsKey(i))
                {
                    LightIDS.Add(i,i);
                    return i;
                }
            }
            uint e = (uint)LightIDS.Count+1; // El ID minimo es 1, 0 se deja para los objetos no interactivos.
            LightIDS.Add(e,e);
            return e;
        }

        internal static bool ReleaseLight2DID(uint id) // Liberia ID de objeto y devuelve si ha tenido exito.
        {
            if (LightIDS == null) { return false; }
            if (LightIDS.ContainsKey(id))
            {
                LightIDS.Remove(id);
                return true;
            }
            return false;
        }

        #endregion

        public static byte[] DeUIntAByte4(uint num) // RGBA - A always must be 255
		{
			byte[] ret=new byte[4];
			
			for(uint t=0;t<3;t++)
			{
				ret[t]=(byte)(num%256);
				num=(num-num%256)/256;
			}
			ret[3] = 255;
			return ret;
		}

		public static uint DeByte4AUInt(byte[] num) // RGBA - A always must be 255
		{
			return (uint)(num[0]+num[1]*256+num[2]*256*256); //+num[3]*256*256*256
		}

        internal static void UpdateIdsMap(int width, int height, Action renderScene)
        {
            lock(Core.LockObject)
            {
                dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
                GL.glBindBuffer(BufferTargetARB.GL_PIXEL_PACK_BUFFER, PixelBufferObject_Select);
                GL.glBufferData(BufferTargetARB.GL_PIXEL_PACK_BUFFER, (int)(width*height*4), IntPtr.Zero, BufferUsageARB.GL_STREAM_READ);
                
                GL.glViewport(0,0,(int)width, (int)height);
                GL.glClearColor(0f,0f,0f,1f);
                GL.glClear(ClearBufferMask.GL_COLOR_BUFFER_BIT | ClearBufferMask.GL_DEPTH_BUFFER_BIT);
                
                renderScene();

                GL.glReadPixels(0, 0, (int)width, (int)height, PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, IntPtr.Zero);
                GL.glBindBuffer(BufferTargetARB.GL_PIXEL_PACK_BUFFER, 0);
                dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
            }
        }

        internal static unsafe uint SelectID(int mouseX, int mouseY, int width, int height)
        {
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            // Falta generar los bufferes.
            uint ret = 0;
            if ((mouseX < width) && (mouseY<height))
            {
                GL.glBindBuffer(BufferTargetARB.GL_PIXEL_PACK_BUFFER, PixelBufferObject_Select);
                IntPtr MapBuffer = GL.glMapBuffer(BufferTargetARB.GL_PIXEL_PACK_BUFFER, BufferAccessARB.GL_READ_ONLY);

                byte* MapPtr = (byte*)(MapBuffer.ToPointer());

                byte[] idcolor = new byte[4];
                idcolor[0] = MapPtr[((mouseX + ((height-mouseY) * width))*4)];
				idcolor[1] = MapPtr[((mouseX + ((height-mouseY) * width))*4)+1];
				idcolor[2] = MapPtr[((mouseX + ((height-mouseY) * width))*4)+2];
				idcolor[3] = MapPtr[((mouseX + ((height-mouseY) * width))*4)+3];

                ret = DeByte4AUInt(idcolor);

                GL.glUnmapBuffer(BufferTargetARB.GL_PIXEL_PACK_BUFFER);
                GL.glBindBuffer(BufferTargetARB.GL_PIXEL_PACK_BUFFER, 0);
            }
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
            ui_SelectedID = ret;
            #if DEBUG
                Console.WriteLine("ui_SelectedID: "+ui_SelectedID);
            #endif
            return ret;
        }
    
        public static uint SelectedID
        {
            get { return ui_SelectedID; }
        }
    }
}