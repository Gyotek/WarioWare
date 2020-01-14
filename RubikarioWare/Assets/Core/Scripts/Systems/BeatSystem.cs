using System;
using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Game.Core
{
    public class BeatSystem : MonoBehaviour
    {
        #region EncapsuledTypes

        private struct BeatDelayedCall
        {
            public BeatDelayedCall(int beatDelay, Action action)
            {
                remainingBeat = beatDelay;
                this.action = action;
            }
            
            public BeatDelayedCall(BeatDelayedCall origin)
            {
                remainingBeat = origin.remainingBeat - 1;
                action = origin.action;
            }
            
            public int remainingBeat;
            public Action action;
        }

        #endregion
        
        [SerializeField] private IntVariable bpmAtom = default;
        public IntVariable BpmAtom => bpmAtom;
        
        [Space(15)]
        
        [SerializeField] private FloatVariable timeSinceLastBeatAtom = default;
        public FloatVariable TimeSinceLastBeatAtom => timeSinceLastBeatAtom;
        [SerializeField] private FloatVariable timeBeforeNextBeatAtom = default;
        public FloatVariable TimeBeforeNextBeatAtom => timeBeforeNextBeatAtom;
        [SerializeField] private IntVariable beatCountAtom = default;
        public IntVariable BeatCountAtom => beatCountAtom;

        [Space(15)]
        
        [SerializeField] private VoidEvent onBeatAtom = default;
        
        private double usedBpm;
        private double timeToNextBeat;
        private double secondsPerBeat;
        public float SecondsPerBeat => (float)secondsPerBeat;
        
        private double pausedDifference;
        private bool isPlaying = false;

        private Queue<BeatDelayedCall> queuedCalls = new Queue<BeatDelayedCall>();

        
        void Awake()
        {
            usedBpm = bpmAtom.Value;
            secondsPerBeat = 60f / usedBpm;
        }
        
        void Update()
        {
            timeBeforeNextBeatAtom.SetValue((float) (timeToNextBeat - AudioSettings.dspTime));
            timeSinceLastBeatAtom.SetValue((float) secondsPerBeat - timeBeforeNextBeatAtom.Value);
            
            if (!isPlaying || AudioSettings.dspTime < timeToNextBeat) return;
            
            timeToNextBeat = AudioSettings.dspTime + secondsPerBeat;
            RefreshValues();
            onBeatAtom.Raise();
            
            beatCountAtom.ApplyChange(1);
            ResolveDelayedCalls();
        }

        public void Stop()
        {
            isPlaying = false;
            queuedCalls.Clear();
        }
        [Button]
        public void Play()
        {
            isPlaying = true;
            RefreshValues();
            beatCountAtom.SetValue(1);
        }

        public void Pause()
        {
            isPlaying = false;
            pausedDifference = timeToNextBeat - AudioSettings.dspTime;
        } 
        public void Continue()
        {
            timeToNextBeat = AudioSettings.dspTime + pausedDifference;
            isPlaying = true;
        }

        public void QueueCall(int beatDelay, Action call)
        {
            if (!isPlaying) return;
            
            beatDelay = (int)Mathf.Clamp(beatDelay, 1, Mathf.Infinity);
            queuedCalls.Enqueue(new BeatDelayedCall(beatDelay, call));
        }
        private void ResolveDelayedCalls()
        {
            var count = queuedCalls.Count;
            while (count > 0)
            {
                var delayedCall = queuedCalls.Dequeue();

                if (delayedCall.remainingBeat == 1) delayedCall.action();
                else queuedCalls.Enqueue(new BeatDelayedCall(delayedCall));
                count--;
            }
        }
        
        private void RefreshValues()
        {
            timeToNextBeat = AudioSettings.dspTime + secondsPerBeat;
            usedBpm = bpmAtom.Value;
            secondsPerBeat = 60f / usedBpm;
        }
    }
}

