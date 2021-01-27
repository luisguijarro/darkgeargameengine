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
        internal static Dictionary<int, int> IDS;
        internal static int GetID() // Otorga un ID a objeto que lo solicita
        {
            if (IDS == null)
            {
                IDS = new Dictionary<int, int>();
            }
            for (int i=0;i<IDS.Count;i++)
            {
                if (!IDS.ContainsKey(i))
                {
                    IDS.Add(i,i);
                    return i;
                }
            }
            int e = IDS.Count;
            IDS.Add(e,e);
            return e;
        }

        internal static bool ReleaseID(int id) // Liberia ID de objeto y devuelve si ha tenido exito.
        {
            if (IDS == null) { return false; }
            if (IDS.ContainsKey(id))
            {
                IDS.Remove(id);
                return true;
            }
            return false;
        }

        internal static byte[] DeIntAByte4(int num) //
		{
			byte[] ret=new byte[4];
			
			for(int t=1;t<3;t++)
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