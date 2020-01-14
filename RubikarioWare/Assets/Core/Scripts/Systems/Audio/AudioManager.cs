using System.Collections;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Core
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private IntVariable bpmAtom;
        [SerializeField] private FloatVariable timeBeforeNextBeatAtom;

        [Space]
        
        [SerializeField] private BeatSystem beatSystem;
        [SerializeField] private AudioSystem audioSystem;

        [Space] 
        
        [SerializeField] private AudioMixerSnapshot macroSnapshot;
        [SerializeField] private AudioMixerSnapshot microSnapshot;
        [SerializeField] private float standardTransitionTime;

        public void PlaySoundtrack(float transitionTime, int musicBpm, float timeBeforeMusicFirstBeat, AudioSource audioSource)
        {
            TransitionToMicroSoundtrack(transitionTime);
            StartCoroutine(PlaySoundtrackOnTimeRoutine(musicBpm, timeBeforeMusicFirstBeat, audioSource));
        }
        
        private IEnumerator PlaySoundtrackOnTimeRoutine(int musicBpm, float timeBeforeMusicFirstBeat, AudioSource audioSource)
        {
            if (musicBpm != bpmAtom.Value) audioSource.pitch = (float)bpmAtom.Value / musicBpm;
            timeBeforeMusicFirstBeat /= audioSource.pitch;

            var secondsPerBeat = Macro.ConvertBeatsToTime(1);
            var waitTime = default(float);
            
            if (timeBeforeMusicFirstBeat > secondsPerBeat)
            {
                var count = secondsPerBeat;
                while (count + secondsPerBeat < timeBeforeMusicFirstBeat) count += secondsPerBeat;
                
                var delay = timeBeforeMusicFirstBeat - count;

                if (timeBeforeNextBeatAtom.Value > delay) waitTime = timeBeforeNextBeatAtom.Value - delay;
                else waitTime = timeBeforeNextBeatAtom.Value + (secondsPerBeat - delay);
            }
            else
            {
                if (timeBeforeNextBeatAtom.Value > timeBeforeMusicFirstBeat) waitTime = timeBeforeNextBeatAtom.Value - timeBeforeMusicFirstBeat;
                else waitTime = timeBeforeNextBeatAtom.Value + (secondsPerBeat - timeBeforeMusicFirstBeat);
            }
            
            yield return new WaitForSeconds(waitTime);
            audioSource.Play();
        } 

        public void TransitionToMacroSoundtrack(float transitionTime = -1f)
        {
            macroSnapshot.TransitionTo(transitionTime == -1f ? standardTransitionTime : transitionTime);
        }
        public void TransitionToMicroSoundtrack(float transitionTime = -1f)
        {
            microSnapshot.TransitionTo(transitionTime == -1f ? standardTransitionTime : transitionTime);
        }
        
        private void OnDisable() => audioSystem.Reset();
    }
}