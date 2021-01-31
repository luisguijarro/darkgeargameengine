using System;
using System.IO;
using System.Collections.Generic;

using dgtk.OpenAL;
using dge.SoundSystem;

namespace dge
{
    public partial class dgWindow
    {
        public class SndSystem
        {
            private OAL_Context cntxt;
            internal SndSystem(OAL_Context context)
            {
                this.cntxt = context;
            }
            public unsafe Sound LoadSndFile(string path)
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

                    this.cntxt.MakeCurrent();
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

            public void SetDistanceAttenuationModel(AL_DistanceModel adm)
            {
                this.cntxt.MakeCurrent();
                AL.alDistanceModel(adm);
            }
        
            #region Listener

            public void SetListener(float PositionX, float PositionY, float PositionZ, float DirectionX, float DirectionY, float DirectionZ, float UpX, float UpY, float UpZ)
            {
                this.cntxt.MakeCurrent();
                AL.alListener3f(AL_Listener3vParam.AL_POSITION, PositionX, PositionY, PositionZ);
                AL.alListener3f(AL_Listener3vParam.AL_VELOCITY, 0f, 0f, 0f);
                AL.alListenerfv(AL_Listener3vParam.AL_ORIENTATION, new float[]{DirectionX, DirectionY, DirectionZ, UpX, UpY, UpZ});
            }

            public void SetListenerPosition(float PositionX, float PositionY, float PositionZ)
            {
                this.cntxt.MakeCurrent();
                AL.alListener3f(AL_Listener3vParam.AL_POSITION, PositionX, PositionY, PositionZ);
            }

            public void SetListenerOrientation(float DirectionX, float DirectionY, float DirectionZ, float UpX, float UpY, float UpZ)
            {
                this.cntxt.MakeCurrent();
                AL.alListenerfv(AL_Listener3vParam.AL_ORIENTATION, new float[]{DirectionX, DirectionY, DirectionZ, UpX, UpY, UpZ});
            }

            public void SetMetersPerUnit(float meters)
            {
                AL.alListenerf(AL_ListenerifParam.AL_METERS_PER_UNIT, meters);
            }

            #endregion
        
            #region Sources

            public SoundSource3D CreateSoundSource3D()
            {
                this.cntxt.MakeCurrent();
                return new SoundSource3D();
            }

            #endregion

            public bool MakeCurrent()
            {
                return this.cntxt.MakeCurrent();
            }
        } 
    }
}