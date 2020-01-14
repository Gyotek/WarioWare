using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "AssetGroup", menuName = "Unity Atoms/Lists/Audio Sequence")]
    public class AudioSequence : ExplicitAssetGroup<AudioClip>, IAudioSequence
    {
        AudioClip[] IAudioSequence.Clips => new AudioClip[] { group[0], group[1], group[2]};
        AudioClip[] IAudioSequence.TransitionClips => new AudioClip[] { group[3], group[4], group[5]};
    }
}