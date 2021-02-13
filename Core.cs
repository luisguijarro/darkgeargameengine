using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace dge
{    
    
    public static class Core
    {
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

    }

    internal static class Core2D
    {
        internal static Dictionary<uint, uint> IDS;
        internal static uint GetID() // Otorga un ID a objeto que lo solicita
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

        internal static bool ReleaseID(uint id) // Liberia ID de objeto y devuelve si ha tenido exito.
        {
            if (IDS == null) { return false; }
            if (IDS.ContainsKey(id))
            {
                IDS.Remove(id);
                return true;
            }
            return false;
        }

        internal static byte[] DeUIntAByte4(uint num) //
		{
			byte[] ret=new byte[4];
			
			for(uint t=1;t<3;t++)
			{
				ret[t]=(byte)(num%256);
				num=(num-num%256)/256;
			}
			ret[3] = 255;
			return ret;
		}

		internal static int DeByte4AInt(byte[] num)
		{
			return num[1]+num[2]*256; //+num[2]*256*256; //+num[3]*256*256*256
		}
    }
}