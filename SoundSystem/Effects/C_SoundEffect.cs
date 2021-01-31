using System;
using dgtk.OpenAL;

namespace dge.SoundSystem
{
    public class C_SoundEffect
    {
        internal uint ui_ID;
        SoundEffectType Effect { get; }
        //uint ID { get; }
        internal EffectSlot slot {get; set;}
        public C_SoundEffect(AL_Effect_Type eftype)
        {
            this.ui_ID = dgtk.OpenAL.EFX.alGenEffect();
            EFX.alEffecti(this.ui_ID, AL_EffectParam.AL_EFFECT_TYPE, (int)eftype);
        }
        public uint ID
        {
            get { return this.ui_ID;}
        }
    }
    
}