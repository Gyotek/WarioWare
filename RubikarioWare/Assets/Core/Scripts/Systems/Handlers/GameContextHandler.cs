using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Game;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    [RequireComponent(typeof(HealthSystem))]
    public partial class GameContextHandler : MonoBehaviour
    {
        #region EncapsulatedTypes
        
        [Serializable]
        private class StateEvent
        {
            public GameStates gameState = default;
            public UnityEvent OnStateEnter = default;
        }

        #endregion

        [SerializeField] private HealthSystem playerHealth;
        
        [SerializeField] private StateEvent[] stateEvents = default; 
        [SerializeField] private IntVariable difficultyAtom = default;
        public IntVariable DifficultyAtom => difficultyAtom;
        
        [SerializeField] private GameModes gameMode;
        public GameModes CurrentGameMode => gameMode;
        
        [ShowInInspector, ReadOnly] private GameStates gameState = GameStates.None;
        public GameStates CurrentGameState => gameState;

        private Dictionary<GameStates, UnityEvent> stateCallbacks = new Dictionary<GameStates, UnityEvent>();

        
        private void Awake() => Array.ForEach(stateEvents, se => stateCallbacks.Add(se.gameState, se.OnStateEnter));

        public void SetCurrentMode(string gameModeName)
        {
            if (!GameModes.TryParse(gameModeName, true, out GameModes newGameMode)) return;
            SetCurrentMode(newGameMode);
        }
        public void SetCurrentMode(GameModes gameMode)
        {
            if (gameMode == this.gameMode) return;
            this.gameMode = gameMode;
            
            if (gameMode == GameModes.Story || gameMode == GameModes.Endless) stateCallbacks[GameStates.Lost].AddListener(HurtCall);
            else stateCallbacks[GameStates.Lost].RemoveListener(HurtCall);

            void HurtCall() => playerHealth.Hurt(1);
        }
        
        public void SetCurrentState(string gameStateName)
        {
            if (!GameStates.TryParse(gameStateName, true, out GameStates newGameState)) return;
            SetCurrentState(newGameState);
        }
        public void SetCurrentState(GameStates gameState)
        {
            if (gameState == this.gameState) return;

            this.gameState = gameState;
            stateCallbacks[gameState].Invoke();
        }
    }
}
