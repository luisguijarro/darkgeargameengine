using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class Echo : I_SoundEffect
    {
        internal uint ui_ID;

        public Echo()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_ECHO);
        }
        ~Echo()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public float Delay
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_DELAY, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_DELAY); }
        }

        public float LR_Delay
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_LRDELAY, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_LRDELAY); }
        }

        public float Damping
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_DAMPING, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_DAMPING); }
        }

        public float FeedBack
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_FEEDBACK, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_FEEDBACK); }
        }

        public float Spread
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_SPREAD, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_ECHO_SPREAD); }
        }


        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.Echo ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}