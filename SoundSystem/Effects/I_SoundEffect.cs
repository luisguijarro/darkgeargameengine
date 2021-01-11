using System;

namespace dge.SoundSystem
{
    public interface I_SoundEffect
    {
        SoundEffectType Effect { get; }
        uint ID { get; }
    }
}