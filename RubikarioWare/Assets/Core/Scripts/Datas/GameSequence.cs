using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Game.Core
{
    public abstract class GameSequence : SerializedScriptableObject, IGameSequence
    {
        [SerializeField] protected GameID[] sequence = default;
        public GameID[] Games => sequence;
        [SerializeField] protected GameTransitions transitions = default;
        public VoidEvent OpeningTransitionEvent => transitions.OpeningEvent;
        public VoidEvent ClosingTransitionEvent => transitions.ClosingEvent;

        protected int currentAdvancement = 0;
        int IGameSequence.Advancement => currentAdvancement - 1;
        
        [NonSerialized, ShowInInspector, ReadOnly] protected List<GameID> currentSequence = new List<GameID>();
        public GameID FirstGame => currentSequence.First();
        

        public virtual void Initialize()
        {
            currentAdvancement = 1;
            
            currentSequence.Clear();
            GenerateSequence();
        }
        
        public bool TryGetNextGame(out GameID nextGame)
        {
            if (currentAdvancement < currentSequence.Count)
            {
                nextGame = currentSequence[currentAdvancement++];
                return true;
            }
            else
            {
                nextGame = null;
                return false;
            }
        }
        public void TryCallTransitionEvent() => transitions.TryCallTransitionEvent(currentAdvancement - 1);

        public void SetSequence(GameID[] sequence)
        {
            if (sequence.Length >= this.sequence.Length) this.sequence = sequence;
        }
        
        protected abstract void GenerateSequence();
    }
}

