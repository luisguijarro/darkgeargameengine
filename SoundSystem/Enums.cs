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
        Freqency_shifter = 0x0006,
        Vocal_Morpher = 0x0007,
        Pitch_Shifter = 0x0008,
        Ring_Modulator = 0x0009,
        Autowah = 0x000A,
        Compressor = 0x000B,
        Equalizer = 0x000C
    }
}