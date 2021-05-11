using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dgtk.OpenAL;

namespace dge.SoundSystem
{
    public static class SoundTools
    {
        private static OAL_Context OpenAL_Cntx;

        internal static void InitStaticSoundSystem()
        {
            OpenAL_Cntx = new OAL_Context();
        }

        public unsafe static Sound LoadSndFile(string path)
        {
            if (File.Exists(path)) // Existe Fichero?
            {
                SF_INFO sf_info = new SF_INFO();
                sf_info.format = 0;
                IntPtr ptr_snd = dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_open(path, OpenMode.SFM_READ, ref sf_info) : ImportsL.sf_open(path, OpenMode.SFM_READ, ref sf_info);
                    #if DEBUG
                    if (ptr_snd == IntPtr.Zero) // Si se ha dado algun error...
                    {
                        Console.WriteLine(dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_error(ptr_snd).ToString() : ImportsL.sf_error(ptr_snd).ToString()); // Hay que saber cual.
                        return null;
                    }
                    #endif

                List<short> l_data = new List<short>();
                short[] data = new short[4096];
                long datareaded = 0;
                while ((datareaded = (dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_read_short(ptr_snd, ref data, 4096) : ImportsL.sf_read_short(ptr_snd, ref data, 4096))) != 0)
                {
                    short[] s_datatemp = new short[datareaded];
                    Array.Copy(data, s_datatemp, datareaded);
                    l_data.AddRange(s_datatemp);
                }
                data = l_data.ToArray();
                if (dge.Core.GetOS() == dge.Core.OperatingSystem.Windows)
                {
                    ImportsW.sf_close(ptr_snd);
                }
                else
                {
                    ImportsL.sf_close(ptr_snd);
                }

                //s_ret.FileName = path;
                // AL_FORMAT alf = sf_info.channels > 1? AL_FORMAT.AL_FORMAT_STEREO16 : AL_FORMAT.AL_FORMAT_MONO16;

                OpenAL_Cntx.MakeCurrent();
                //s_ret.IDBuffer = AL.alGenBuffer(); // Lo metemos en el constructor del Sonido.

                Sound s_ret = new Sound(path, (byte)sf_info.channels, data, sf_info.samplerate);
                // AL.alBufferData(s_ret.ID, alf, data, data.Length*sizeof(short), sf_info.samplerate); // Lometemos en el constructor de Sound.


                return s_ret;
            }
            else // Si no existe...
            {
                return null;
            }
        }

        public unsafe static Sound LoadSndFile(byte[] bytes, string name)
        {
            if (bytes.Length > 0) // Existen bytes?
            {
                SF_INFO sf_info = new SF_INFO();
                sf_info.format = 0;
                SF_VIRTUAL_IO sf_v_io = new SF_VIRTUAL_IO();

                IntPtr BytesPrt = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, BytesPrt, bytes.Length);

                IntPtr ptr_snd = dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_open_virtual(ref sf_v_io, OpenMode.SFM_READ, ref sf_info, BytesPrt) : ImportsL.sf_open_virtual(ref sf_v_io, OpenMode.SFM_READ, ref sf_info, BytesPrt);
                    #if DEBUG
                    if (ptr_snd == IntPtr.Zero) // Si se ha dado algun error...
                    {
                        Console.WriteLine(dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_error(ptr_snd).ToString() : ImportsL.sf_error(ptr_snd).ToString()); // Hay que saber cual.
                        return null;
                    }
                    #endif

                List<short> l_data = new List<short>();
                short[] data = new short[4096];
                long datareaded = 0;
                while ((datareaded = (dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_read_short(ptr_snd, ref data, 4096) : ImportsL.sf_read_short(ptr_snd, ref data, 4096))) != 0)
                {
                    short[] s_datatemp = new short[datareaded];
                    Array.Copy(data, s_datatemp, datareaded);
                    l_data.AddRange(s_datatemp);
                }
                data = l_data.ToArray();
                if (dge.Core.GetOS() == dge.Core.OperatingSystem.Windows)
                {
                    ImportsW.sf_close(ptr_snd);
                }
                else
                {
                    ImportsL.sf_close(ptr_snd);
                }

                OpenAL_Cntx.MakeCurrent();

                Sound s_ret = new Sound(name, (byte)sf_info.channels, data, sf_info.samplerate);

                Marshal.FreeHGlobal(BytesPrt);
                return s_ret;
            }
            else
            {
                return null;
            }
        }
    }
}