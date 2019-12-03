using System;
using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

using Sirenix.OdinInspector;

#pragma warning disable CS0649
namespace Game.Core
{
	[RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(TimerHandler))]
    public class BeatEngine : SerializedMonoBehaviour
    {
        #region References
        [SerializeField]
        private VoidEvent onBeat;
        [SerializeField] private TimerHandler timerHandler;
        [Space]
        [SerializeField] private bool playMetronome = true;
        [SerializeField]
        private (AudioClip first, AudioClip second) sounds;
        private AudioSource _source;
		#region Output
		[SerializeField] private FloatVariable outputTimeSinceLast = default;
		[SerializeField] private FloatVariable outputTimeBeforeNext = default;
		[SerializeField] private IntVariable outputBeatCount = default;
		#endregion
		#endregion

		#region Debug

		//[BoxGroup("Debug"), ShowInInspector, ReadOnly]
		public static bool IsPaused { get; private set; }
        [SerializeField, BoxGroup("Debug"), ShowInInspector]
        private IntVariable _bpm;

        [BoxGroup("Debug"), ShowInInspector, Range(0f, 1f)]
        public static double percentage;

        //[BoxGroup("Debug"), ShowInInspector, ReadOnly]
        public static int CurrentBpm { get; private set; }
        
        [BoxGroup("Debug"), ShowInInspector] 
        public static double BeatLength => 60f / CurrentBpm;
        
        [BoxGroup("Debug"), ShowInInspector, ReadOnly]
        public static double TimeSinceLast { get; private set; }

        [BoxGroup("Debug"), ShowInInspector, ReadOnly]
        public static double TimeBeforeNext { get; private set; }

        [BoxGroup("Debug"), ShowInInspector, ReadOnly]
        public static int BeatsSinceStart { get; set; }

        //[BoxGroup("Debug"), ShowInInspector, ReadOnly]
        private static double PrevBeat => NextBeat - BeatLength;

        //[BoxGroup("Debug"), ShowInInspector, ReadOnly]
        private static double NextBeat { get; set; }

        private static double PauseTime { get; set; }

        #endregion

        #region Delayed Actions

        private List<DelayedAction> delayedActions = new List<DelayedAction>();
        

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Awake()
        {
            IsPaused = true;
            Play();
            if(!timerHandler) timerHandler = GetComponent<TimerHandler>();
        }

        private void Update()
        {
            if (!IsPaused) UpdateValues();
			OutputValue();
		}

		private void OutputValue()
		{
			outputBeatCount.SetValue(BeatsSinceStart);
			outputTimeBeforeNext.SetValue((float)TimeBeforeNext);
			outputTimeSinceLast.SetValue((float)TimeSinceLast);
		}
        #endregion

        #region Beat
        private void UpdateValues()
        {
            if (AudioSettings.dspTime >= NextBeat)
            {
                Beat();
            }

            TimeSinceLast = BeatLength - (NextBeat - AudioSettings.dspTime);
            TimeBeforeNext = NextBeat - AudioSettings.dspTime;
            percentage = TimeSinceLast / BeatLength;
        }

        private void Beat()
        {
            if(_source && sounds.first && sounds.second && playMetronome) PlayBeat();
            else if(playMetronome) Debug.Log("Beat Engine doesn't have any source or sounds to play");

            CurrentBpm = _bpm.Value;
            NextBeat += 60.0f / CurrentBpm;
            BeatsSinceStart++;
            onBeat.Raise();
            PlayDelayedAction();
        }

        private void PlayBeat()
        {
            if (BeatsSinceStart % 4 == 0) _source.PlayOneShot(sounds.second);
            else _source.PlayOneShot(sounds.first);
        }
        
        public static float BeatsToSeconds(int nbrOfBeats)
        {
            return nbrOfBeats * (float)BeatLength;
        }
        
        public static float SecondsAffectedByBeat(float nbrOfSeconds)
        {
            return nbrOfSeconds / 0.625f * (float)BeatLength;
        }

        public void PlayActionOnBeat(Action actionToPlay, int beatDelay = 0)
        {
            delayedActions.Add(new DelayedAction(actionToPlay, beatDelay));
        }

        private void PlayDelayedAction()
        {
            for (int i = 0; i < delayedActions.Count; i++)
            {
                if (delayedActions[i].beatDelay == 0)
                {
                    delayedActions[i].action.Invoke();
                    delayedActions.RemoveAt(i);
                }
                else
                {
                    DelayedAction modifiedAction = new DelayedAction(delayedActions[i]);
                    delayedActions[i] = modifiedAction;
                }
            }
        }

        #endregion

        #region Utilities
        [ButtonGroup("Utilities")]
        public void Play()
        {
            if(!IsPaused) Debug.Log("Beat Engine is already paused");
            else
            {
                IsPaused = false; 
                NextBeat += AudioSettings.dspTime - PauseTime;
            }
        }
        
        [ButtonGroup("Utilities")]
        public void Pause()
        {
            if(IsPaused) Debug.Log("Beat Engine is already playing");
            else
            {
                IsPaused = true;
                PauseTime = AudioSettings.dspTime;
            }
        }
        
        [ButtonGroup("Utilities")]
        public void Reset()
        {
            BeatsSinceStart = 0;
        }

        #endregion

        private struct DelayedAction
        {
            public DelayedAction(DelayedAction delayedAction)
            {
                this.action = delayedAction.action;
                this.beatDelay = delayedAction.beatDelay - 1;
            }
            public DelayedAction(Action action, int beatDelay)
            {
                this.action = action;
                this.beatDelay = beatDelay;
            }
            public Action action;
            public int beatDelay;
        }
    }
}


