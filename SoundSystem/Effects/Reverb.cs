using System;
using dgtk.OpenAL;

namespace dge.SoundSystem.Effects
{
    public class Reverb : I_SoundEffect
    {
        internal EffectSlot slot;
        internal uint ui_ID;
        public Reverb()
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)AL_Effect_Type.AL_EFFECT_REVERB);
        }

        private void UpdateEffect2Slot()
        {
            slot.AttachEffect(this);
        }

        ~Reverb()
        {
            dgtk.OpenAL.EFX.alDeleteEffect(this.ui_ID);
        }

        #region Parametros:

        public float Density
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DENSITY, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DENSITY); }
        }

        public float Diffusion
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DIFFUSION, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DIFFUSION); }
        }
        
        public float Gain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_GAIN, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_GAIN); }
        }
        
        public float GainHF
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_GAINHF, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_GAINHF); }
        }
        
        public float DecayTime
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DECAY_TIME, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DECAY_TIME); }
        }
        
        public float DecayHFRatio
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DECAY_HFRATIO, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DECAY_HFRATIO); }
        }
        
        public float ReflectionsGain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_REFLECTIONS_GAIN, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_REFLECTIONS_GAIN); }
        }
        
        public float ReflectionsDelay
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_REFLECTIONS_DELAY, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_REFLECTIONS_DELAY); }
        }
        
        public float LateReverbGain
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_LATE_REVERB_GAIN, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_LATE_REVERB_GAIN); }
        }
        
        public float LateReverbDelay
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_LATE_REVERB_DELAY, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_LATE_REVERB_DELAY); }
        }
        
        public float RoomRolloffFactor
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_ROOM_ROLLOFF_FACTOR, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_ROOM_ROLLOFF_FACTOR); }
        }
        
        public float AirAbsorptionGainHF
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_AIR_ABSORPTION_GAINHF, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_AIR_ABSORPTION_GAINHF); }
        }
        
        public float DecayHFLimit
        {
            set { EFX.alEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DECAY_HFLIMIT, value); this.UpdateEffect2Slot(); }
            get { return EFX.alGetEffectf(this.ui_ID, AL_EffectParam.AL_REVERB_DECAY_HFLIMIT); }
        }

        #endregion
        
        public SoundEffectType Effect
        {
            get { return SoundEffectType.Reverb ;}
        }
        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
}