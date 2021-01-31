using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class VocalMorpher : C_SoundEffect
    {
        // internal EffectSlot slot;
        // internal uint ui_ID;

        public VocalMorpher() : base (AL_Effect_Type.AL_EFFECT_VOCAL_MORPHER)
        {
            // this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            // EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_VOCAL_MORPHER);
        }

        private void UpdateEffect2Slot()
        {
            slot.AttachEffect(this);
        }

        ~VocalMorpher()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public AL_Vocal_Morpher_Param Phoneme_A
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEA, (int)value); this.UpdateEffect2Slot(); }
            get { return (AL_Vocal_Morpher_Param)EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEA); }
        }

        public AL_Vocal_Morpher_Param Phoneme_B
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEB, (int)value); this.UpdateEffect2Slot(); }
            get { return (AL_Vocal_Morpher_Param)EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEB); }
        }

        public int Phoneme_A_Coarse_Tunning
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEA_COARSE_TUNING, (int)value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEA_COARSE_TUNING); }
        }

        public int Phoneme_B_Coarse_Tunning
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEB_COARSE_TUNING, (int)value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_PHONEMEB_COARSE_TUNING); }
        }

        public int WaveForm
        {
            set { EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_WAVEFORM, (int)value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffecti(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_WAVEFORM); }
        }

        public float Rate
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_RATE, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_VOCAL_MORPHER_RATE); }
        }

        #endregion

        public SoundEffectType Effect
        {
            get { return SoundEffectType.VocalMorpher ;}
        }
/*
        public uint ID
        {
            get { return this.ui_ID;}
        }*/
    }
}