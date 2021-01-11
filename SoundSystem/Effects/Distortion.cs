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


        public float Edge
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_EDGE, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_EDGE); }
        }

        public float Gain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_GAIN, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_GAIN); }
        }

        public float Lowpass_Cutoff
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_LOWPASS_CUTOFF, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_LOWPASS_CUTOFF); }
        }

        public float EQ_Center
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_EQCENTER, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_EQCENTER); }
        }

        public float EQ_Bandwidth
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_EQBANDWIDTH, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_DISTORTION_EQBANDWIDTH); }
        }


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