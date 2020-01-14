using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


namespace Game.Core
{
    [CreateAssetMenu(menuName = "Wario Ware/Macro/Sound Systems")]
    public class AudioSystem : ScriptableObject
    {
        [SerializeField] private GameObject referenceSound;
        [SerializeField] private List<AudioSource> sourceList;
        private GameObject parentSounds;

        [Space] 
        
        [SerializeField, Range(0f, 1f)] private float maxVolume;
        [SerializeField] private float soundtrackFadeInTime;
        [SerializeField] private float soundtrackFadeOutTime;
        
        public void RivalSoundLaunch(AudioSource audioSource)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }

        public void PrefabSoundLaunch(AudioClip clip)
        {
            if (sourceList.Count == 0) { parentSounds = new GameObject("Instantiated Sounds "); CreateAudioSource(clip); }
            else
            {
                for (var i = 0; i < sourceList.Count; i++)
                {
                    if (!sourceList[i].isPlaying)
                    {
                        SetClipAndPlay(sourceList[i], clip);
                        i = sourceList.Count;
                    }
                    else if (sourceList[i].isPlaying && i == sourceList.Count - 1)
                    {
                        CreateAudioSource(clip);
                        i = sourceList.Count;
                    }
                }
            }
        }

        void CreateAudioSource(AudioClip clip)
        {
            GameObject reference = Instantiate(referenceSound, parentSounds.transform, true);
            AudioSource source = reference.GetComponent<AudioSource>();
            SetClipAndPlay(source, clip);
            sourceList.Add(source);
        }

        void SetClipAndPlay(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();

        }

        public void Reset()
        {
            sourceList.Clear();
            parentSounds = null;
        }

        
        public void IncreaseVolume(AudioSource source)
        {
            DOTween.To(() => source.volume, x => source.volume = x, maxVolume, soundtrackFadeInTime);
        }
        public void DecreaseVolume(AudioSource source)
        {
            DOTween.To(() => source.volume, x => source.volume = x,0, soundtrackFadeOutTime );
        }
        
        public void IncreaseVolumeAndPlay(AudioSource source)
        {
            var tween = DOTween.To(() => source.volume, x => source.volume = x, maxVolume, soundtrackFadeInTime);
            tween.OnComplete(source.Play);
        }
        public void DecreaseVolumeAndStop(AudioSource source)
        {
            var tween = DOTween.To(() => source.volume, x => source.volume = x, 0, soundtrackFadeOutTime);
            tween.OnComplete(source.Stop);
        }
    }
}

