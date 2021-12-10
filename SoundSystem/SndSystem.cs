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
            internal SndSystem(OAL_Context context)
            {
                
            }
            public unsafe Sound LoadSndFile(string path)
            {
                 return dge.SoundSystem.SoundTools.LoadSndFile(path);
            }

            public void SetDistanceAttenuationModel(AL_DistanceModel adm)
            {
                dge.SoundSystem.SoundTools.SetDistanceAttenuationModel(adm);
            }
        
            #region Listener

            public void SetListener(float PositionX, float PositionY, float PositionZ, float DirectionX, float DirectionY, float DirectionZ, float UpX, float UpY, float UpZ)
            {
                dge.SoundSystem.SoundTools.SetListener(PositionX, PositionY, PositionZ, DirectionX, DirectionY, DirectionZ, UpX, UpY, UpZ);
            }

            public void SetListenerPosition(float PositionX, float PositionY, float PositionZ)
            {
                dge.SoundSystem.SoundTools.SetListenerPosition(PositionX, PositionY, PositionZ);
            }

            public void SetListenerOrientation(float DirectionX, float DirectionY, float DirectionZ, float UpX, float UpY, float UpZ)
            {
                dge.SoundSystem.SoundTools.SetListenerOrientation(DirectionX, DirectionY, DirectionZ, UpX, UpY, UpZ);
            }

            public void SetMetersPerUnit(float meters)
            {
                dge.SoundSystem.SoundTools.SetMetersPerUnit(meters);
            }

            #endregion
        
            #region Sources

            public SoundSource3D CreateSoundSource3D()
            {
                return dge.SoundSystem.SoundTools.CreateSoundSource3D();
            }

            #endregion

            public bool MakeCurrent()
            {
                return dge.SoundSystem.SoundTools.MakeCurrent();
            }
        } 
    }
}