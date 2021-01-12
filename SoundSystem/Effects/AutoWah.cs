using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class AutoWah : I_SoundEffect
    {
        internal uint ui_ID;

        public AutoWah()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_AUTOWAH);
        }
        ~AutoWah()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public float Attack_Timer
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_ATTACK_TIME, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_ATTACK_TIME); }
        }

        public float Release_Timer
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_RELEASE_TIME, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_RELEASE_TIME); }
        }

        public float Resonance
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_RESONANCE, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_RESONANCE); }
        }

        public float Peak_Gain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_PEAK_GAIN, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_AUTOWAH_PEAK_GAIN); }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.AutoWah ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}