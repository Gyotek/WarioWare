using UnityEngine;

namespace Game.Core
{
    public interface IAudioSequence
    {
        AudioClip[] Clips { get; }
        AudioClip[] TransitionClips { get; }
    }
}