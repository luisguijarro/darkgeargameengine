using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class RingModulator : I_SoundEffect
    {
        internal uint ui_ID;

        public RingModulator()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_RING_MODULATOR);
        }
        ~RingModulator()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public float Frequency
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_RING_MODULATOR_FREQUENCY, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_RING_MODULATOR_FREQUENCY); }
        }

        public float Highpass_Cutoff
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_RING_MODULATOR_HIGHPASS_CUTOFF, value); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_RING_MODULATOR_HIGHPASS_CUTOFF); }
        }

        public int WaveForm
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_RING_MODULATOR_WAVEFORM, (int)value); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_RING_MODULATOR_WAVEFORM); }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.RingModulator ;}
        }

        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}