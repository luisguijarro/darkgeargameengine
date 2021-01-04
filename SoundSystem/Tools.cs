using System;
using System.IO;
using System.Runtime.InteropServices;

using dgtk.OpenAL;

namespace dge.SoundSystem
{
    public static class Tools
    {
        public static unsafe Sound LoadSndFile(string path)
        {
            if (File.Exists(path)) // Existe Fichero?
            {
                dgtk.SoundSystem.OpenAlContext.MakeCurrent();
                SF_INFO sf_info = new SF_INFO();
                sf_info.format = 0;
                IntPtr ptr_snd = Imports.sf_open(path, OpenMode.SFM_READ, ref sf_info);
                    #if DEBUG
                    if (ptr_snd == IntPtr.Zero) // Si se ha dado algun error...
                    {
                        Console.WriteLine(Imports.sf_error(ptr_snd).ToString()); // Hay que saber cual.
                        return null;
                    }
                    #endif
                
                short[] data = new short[sf_info.channels*sf_info.frames];
                if (Imports.sf_readf_short(ptr_snd, ref data, sf_info.frames) != sf_info.channels*sf_info.frames)
                {
                    #if DEBUG
                    if (ptr_snd == IntPtr.Zero) // Si se ha dado algun error...
                    {
                        Console.WriteLine(Imports.sf_error(ptr_snd).ToString()); // Hay que saber cual.
                        return null;
                    }
                    #endif
                }

                Sound s_ret = new Sound();
                s_ret.FileName = path;
                AL_FORMAT alf = sf_info.channels > 1? AL_FORMAT.AL_FORMAT_STEREO16 : AL_FORMAT.AL_FORMAT_MONO16;

                s_ret.IDBuffer = AL.alGenBuffer();

                //IntPtr data_ptr = Marshal.AllocHGlobal(data.Length);
                //Marshal.Copy(data, 0, data_ptr, data.Length);

                AL.alBufferData(s_ret.IDBuffer, alf, data, data.Length*sizeof(short), sf_info.samplerate);
                //Marshal.FreeHGlobal(data_ptr);

                Imports.sf_close(ptr_snd);

                return s_ret;
            }
            else // Si no existe...
            {
                return null;
            }
        }

        public static void SetDistanceAttenuationModel(AL_DistanceModel adm)
        {
            AL.alDistanceModel(adm);
        }
    
        #region Listener

        public static void SetListener(float PositionX, float PositionY, float PositionZ, float DirectionX, float DirectionY, float DirectionZ, float UpX, float UpY, float UpZ)
        {
            dgtk.SoundSystem.OpenAlContext.MakeCurrent();
            AL.alListener3f(AL_Listener3vParam.AL_POSITION, PositionX, PositionY, PositionZ);
            AL.alListener3f(AL_Listener3vParam.AL_VELOCITY, 0f, 0f, 0f);
            AL.alListenerfv(AL_Listener3vParam.AL_ORIENTATION, new float[]{DirectionX, DirectionY, DirectionZ, UpX, UpY, UpZ});
        }

        public static void SetListenerPosition(float PositionX, float PositionY, float PositionZ)
        {
            dgtk.SoundSystem.OpenAlContext.MakeCurrent();
            AL.alListener3f(AL_Listener3vParam.AL_POSITION, PositionX, PositionY, PositionZ);
        }

        public static void SetListenerOrientation(float DirectionX, float DirectionY, float DirectionZ, float UpX, float UpY, float UpZ)
        {
            dgtk.SoundSystem.OpenAlContext.MakeCurrent();
            AL.alListenerfv(AL_Listener3vParam.AL_ORIENTATION, new float[]{DirectionX, DirectionY, DirectionZ, UpX, UpY, UpZ});
        }

        #endregion
    } 
}