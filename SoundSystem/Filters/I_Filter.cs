using System;

namespace dge.SoundSystem
{
    public interface I_Filter
    {
        FilterType Filter { get; }
        uint ID { get; }
    }
}