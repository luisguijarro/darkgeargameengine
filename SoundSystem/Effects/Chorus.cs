using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class Chorus : C_SoundEffect
    {
        // internal EffectSlot slot;
        // internal uint ui_ID;

        public Chorus() : base(AL_Effect_Type.AL_EFFECT_CHORUS)
        {
            // this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            // EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_CHORUS);
        }

        private void UpdateEffect2Slot()
        {
            slot.AttachEffect(this);
        }

        ~Chorus()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public int WaveForm
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_CHORUS_WAVEFORM, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_CHORUS_WAVEFORM); }
        }

        public int Phase
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_CHORUS_PHASE, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_CHORUS_PHASE); }
        }

        public float Rate
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_RATE, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_RATE); }
        }

        public float Depth
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_DEPTH, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_DEPTH); }
        }

        public float FeedBack
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_FEEDBACK, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_FEEDBACK); }
        }

        public float Delay
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_DELAY, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_CHORUS_DELAY); }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.Chorus ;}
        }
/*
        public uint ID
        {
            get { return this.ui_ID;}
        }*/
    }
}