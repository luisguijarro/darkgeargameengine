using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class Distortion : I_SoundEffect
    {
        internal uint ui_ID;

        public Distortion()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_DISTORTION);
        }
        ~Distortion()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:



        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.Distortion ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}