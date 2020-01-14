using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public class EndlessSequenceLauncher : MonoBehaviour, IGameSequence
    {
        [SerializeField] private AssetGroup gameIdsGroup;
        [SerializeField] private GameTransitions transitions = default;
        public VoidEvent OpeningTransitionEvent => transitions.OpeningEvent;
        public VoidEvent ClosingTransitionEvent => transitions.ClosingEvent;
        
        [Space(15)] 
        
        [SerializeField] private int phaseLength;

        private GameID firstGame;
        public GameID FirstGame => firstGame;
        
        [ShowInInspector, ReadOnly] private int currentAdvancement;
        int IGameSequence.Advancement => currentAdvancement;
        
        private List<GameID> currentSequence = new List<GameID>();
        public GameID[] Games => currentSequence.ToArray();

        [ShowInInspector, ReadOnly] private int phaseCountDown;
        private GameID[] gameIds;
        private int currentPhase;
        

        void Start() => gameIds = gameIdsGroup.GetAssets<GameID>().ToArray();
        
        public void Initialize()
        {
            currentAdvancement = 1;

            currentPhase = 1;
            phaseCountDown = phaseLength;
            
            GenerateSequence();
            firstGame = currentSequence.First();
        }

        public bool TryGetNextGame(out GameID nextGame)
        {
            phaseCountDown--;
            if (phaseCountDown == 0)
            {
                currentPhase++;
                phaseCountDown = phaseLength;
                
                GenerateSequence();
            }
            var index = -((currentPhase - 1) * phaseLength) + currentAdvancement++;
            nextGame = currentSequence[index];
            
            return true;
        }
        public void TryCallTransitionEvent() => transitions.TryCallTransitionEvent(currentAdvancement - 1);

        private void GenerateSequence()
        {
            currentSequence.Clear();
            
            while (currentSequence.Count != phaseLength)
            {
                var randIndex = Random.Range(0, gameIds.Length);
                if (currentSequence.Contains(gameIds[randIndex])) continue;
                currentSequence.Add(gameIds[randIndex]);
            }
        }
    }
}