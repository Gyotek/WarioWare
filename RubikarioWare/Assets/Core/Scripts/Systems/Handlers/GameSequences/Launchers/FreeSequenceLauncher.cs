using System;
using System.Collections;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class FreeSequenceLauncher : MonoBehaviour, IGameSequence
    {
        [SerializeField] private VoidEvent openingEvent = default;
        public VoidEvent OpeningTransitionEvent => openingEvent;
        [SerializeField] private VoidEvent closingEvent = default;
        public VoidEvent ClosingTransitionEvent => closingEvent;
        
        private bool isRepeatable;
        private GameID game;
        public GameID FirstGame => game;
        public GameID[] Games => new GameID[] { game };

        int IGameSequence.Advancement => 0;
        
        public void SetupRepeatableSequence(GameID game)
        {
            this.game = game;
            isRepeatable = true;
        }
        public void SetupSingleSequence(GameID game)
        {
            this.game = game;
            isRepeatable = false;
        }
        
        public void Initialize() { }
        public bool TryGetNextGame(out GameID nextGame)
        {
            if (isRepeatable)
            {
                nextGame = game;
                return true;
            }
            else
            {
                nextGame = null;
                return false;
            }
        }
        public void TryCallTransitionEvent() { }
    }
}