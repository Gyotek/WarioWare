using System;
using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public class OnTimeReachedRaise : StateMachineBehaviour
    {
        #region EncapsuledTypes

        [Serializable]
        private struct TimedCall : IComparable<TimedCall>
        {
            [SerializeField] private int frameGoal;
            public int FrameGoal => frameGoal;

            [SerializeField] private VoidEvent callback;
            public VoidEvent Callback => callback;

            public int CompareTo(TimedCall other)
            {
                if (other.frameGoal == this.frameGoal) return 0;
                else if (other.frameGoal < this.frameGoal) return -1;
                else return 1;
            }
        }

        #endregion

        [SerializeField] private TimedCall[] timedCalls;
        private Stack<TimedCall> stackedTimedCalls = new Stack<TimedCall>();

        private void OnEnable() => Array.Sort(timedCalls);

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            stackedTimedCalls.Clear();
            foreach (var timedCall in timedCalls) stackedTimedCalls.Push(timedCall);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stackedTimedCalls.Count <= 0) return;
            
            var currentFrame = Mathf.RoundToInt(stateInfo.length * stateInfo.normalizedTime * 60);
            if (currentFrame >= stackedTimedCalls.Peek().FrameGoal)
            {
                stackedTimedCalls.Pop().Callback.Raise();
            }
        }
    }
}