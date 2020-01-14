using System;
using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public class TransitionResolver : MonoBehaviour
    {
        [SerializeField] private Animator transitionScreenAnimator;
        private readonly Dictionary<GameModes, int> transitionIds = new Dictionary<GameModes, int>()
        {
            {GameModes.Story, Animator.StringToHash("IsStoryTransition")},
            {GameModes.Free, Animator.StringToHash("IsFreeTransition")},
            {GameModes.Endless, Animator.StringToHash("IsEndlessTransition")},
        };

        [SerializeField] private GameContextHandler gameContextHandler;

        public void ResolveSequenceStart()
        {
            transitionScreenAnimator.SetTrigger(transitionIds[gameContextHandler.CurrentGameMode]);
        }
    }
}
