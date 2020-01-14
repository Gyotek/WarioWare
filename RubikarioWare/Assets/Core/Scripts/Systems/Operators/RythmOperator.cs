using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public class RythmOperator : MonoBehaviour
    {
        [SerializeField] private IntVariable bpmAtom;
        [SerializeField] private FloatVariable timeBeforeNextBeatAtom;
        
        [Space]
        
        [SerializeField] private Animator animator;
        private readonly int speedUpId = Animator.StringToHash("SpeedUp");

        [Space] 
        
        [SerializeField] private AudioSource macroSoundtrackSource;
        private IAudioSequence currentAudioSequence;

        private int iterations = 1;
        private Dictionary<int, int> bpmTraductions = new Dictionary<int, int>()
        {
            {1, 96},
            {2, 120},
            {3, 160},
            {4, 180}
        };
        
        public void Initialize() => iterations = 1;

        public void SetCurrentAudioSequence(Object obj)
        {
            if (obj is IAudioSequence sequence) currentAudioSequence = sequence;
        }
        
        public void SpeedUp()
        {
            animator.SetTrigger(speedUpId);
            
            if (currentAudioSequence.TransitionClips != null)
            {
                var nextClip = currentAudioSequence.Clips[iterations + 1];
                var transitionClip = currentAudioSequence.TransitionClips[iterations];

                StartCoroutine(SwitchClipRoutine(nextClip, transitionClip));
            }
            else
            {
                
            }
            
            iterations++;
            bpmAtom.SetValue(bpmTraductions[iterations]);
        }

        private IEnumerator SwitchClipRoutine(AudioClip nextClip, AudioClip transitionClip)
        {
            yield return new WaitForSeconds(timeBeforeNextBeatAtom.Value);
            macroSoundtrackSource.clip = transitionClip;
            macroSoundtrackSource.Play();
            
            yield return new WaitForSeconds(transitionClip.length);
            macroSoundtrackSource.clip = nextClip;
            macroSoundtrackSource.Play();
        }
    }
}