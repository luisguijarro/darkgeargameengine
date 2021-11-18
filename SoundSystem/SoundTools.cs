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

        public static OAL_Context OpenAL_Context
        {
            get { return OpenAL_Cntx; }
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

                List<float> l_data = new List<float>();
                float[] data = new float[4096];
                long datareaded = 0;
                while ((datareaded = (dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_read_float(ptr_snd, ref data, 4096) : ImportsL.sf_read_float(ptr_snd, ref data, 4096))) != 0)
                {
                    float[] s_datatemp = new float[datareaded];
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
                //byte[] nbytes = new byte[524288];
                //bytes.CopyTo(nbytes, 0);

                VIO_DATA v_data = new VIO_DATA
                {
                    offset = 0,
                    Length = bytes.Length,
                    //data = bytes
                };

                fixed(byte* b_ptr = bytes)
                {
                    v_data.data = b_ptr;
                }

                int SizeInBytes_of_v_data = (sizeof(long)*2) + bytes.Length;

                IntPtr PtrData = Marshal.AllocHGlobal(SizeInBytes_of_v_data); //Marshal.SizeOf(v_data));
                Marshal.StructureToPtr(v_data, PtrData, false);

                SF_INFO sf_info = new SF_INFO();
                sf_info.format = 0;
                SF_VIRTUAL_IO v_io = new SF_VIRTUAL_IO
                {
                    get_filelen = sf_vio_get_filelen,
                    seek = sf_vio_seek,
                    read = sf_vio_read,
                    write = sf_vio_write,
                    tell = sf_vio_tell
                };

                IntPtr ptr_snd = dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? 
                ImportsW.sf_open_virtual(ref v_io, OpenMode.SFM_READ, ref sf_info, PtrData) : 
                ImportsL.sf_open_virtual(ref v_io, OpenMode.SFM_READ, ref sf_info, PtrData);

                    #if DEBUG
                    if (ptr_snd == IntPtr.Zero) // Si se ha dado algun error...
                    {
                        int error = (int)(dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_error(ptr_snd) : ImportsL.sf_error(ptr_snd));
                        Console.WriteLine(dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_error_number(error).ToString() : ImportsL.sf_error_number(error).ToString()); // Hay que saber cual.
                        return null;
                    }
                    #endif

                List<float> l_data = new List<float>();
                float[] data = new float[4096];
                long datareaded = 0;
                while ((datareaded = (dge.Core.GetOS() == dge.Core.OperatingSystem.Windows ? ImportsW.sf_read_float(ptr_snd, ref data, 4096) : ImportsL.sf_read_float(ptr_snd, ref data, 4096))) != 0)
                {
                    float[] s_datatemp = new float[datareaded];
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

                Marshal.FreeHGlobal(PtrData);
                
                return s_ret;
            }
            else
            {
                return null;
            }
        }

        
        private static long sf_vio_get_filelen (IntPtr user_data)
        {
            VIO_DATA vd = (VIO_DATA)Marshal.PtrToStructure(user_data, typeof(VIO_DATA));
            
            int length = (int)vd.Length;
            Marshal.StructureToPtr(vd, user_data, true);
            return length;
        }
        private static long sf_vio_seek (long offset, Whence whence, IntPtr user_data)
        {
            VIO_DATA vd = (VIO_DATA)Marshal.PtrToStructure(user_data, typeof(VIO_DATA));
            switch (whence)
            {	case Whence.SEEK_SET :
                    vd.offset = offset ;
                    break ;

                case Whence.SEEK_CUR :
                    vd.offset = vd.offset + offset ;
                    break ;

                case Whence.SEEK_END :
                    vd.offset = vd.Length + offset ;
                    break ;
                default :
                    break ;
            } ;
            Marshal.StructureToPtr(vd, user_data, true);
            return vd.offset;
        }
        private static unsafe long sf_vio_read (IntPtr ptr, long count, IntPtr user_data)
        {
            VIO_DATA vd = (VIO_DATA)Marshal.PtrToStructure(user_data, typeof(VIO_DATA));

            if (vd.offset + count >= vd.Length)
            {
                count = vd.Length - vd.offset;
            }
            byte[] bytes = new byte[vd.Length];
            for (long i=0;i<vd.Length;i++)
            {
                bytes[i] = vd.data[i];
            }

            Marshal.Copy(bytes, (int)vd.offset, ptr, (int)count);
            
            vd.offset += count;

            Marshal.StructureToPtr(vd, user_data, true);

            return count;
        }
        private static long sf_vio_write (IntPtr ptr, long count, IntPtr user_data)
        {
            // FALTA CÓDIGO
            return count ;
        }
        private static long sf_vio_tell (IntPtr user_data)
        {
            VIO_DATA vd = (VIO_DATA)Marshal.PtrToStructure(user_data, typeof(VIO_DATA));

            int offset = (int)vd.offset;
            Marshal.StructureToPtr(vd, user_data, true);
            return offset;
        }
        
    }
}