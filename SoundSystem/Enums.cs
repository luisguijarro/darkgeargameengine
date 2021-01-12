using System;

namespace dge.SoundSystem
{
    public enum SoundEffectType // AL_Effect_Type
    {
        Null = 0x0000,
        Reverb = 0x0001,
        Chorus = 0x0002,
        Distortion = 0x0003,
        Echo = 0x0004,
        Flanger = 0x0005,
        FrequencyShifter = 0x0006,
        VocalMorpher = 0x0007,
        PitchShifter = 0x0008,
        RingModulator = 0x0009,
        AutoWah = 0x000A,
        Compressor = 0x000B,
        Equalizer = 0x000C
    }


    public enum FilterType
    {
        Null = 0x0000,
        LowPass = 0x0001,
        HighPass = 0x0002,
        BandPass = 0x0003
    }

}