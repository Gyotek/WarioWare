using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class GameSequenceHandler : MonoBehaviour
    {
        [SerializeField] private VoidEvent onSequenceOpenedAtomEvent;
        [SerializeField] private IntVariable sequenceAdvancementAtom = default;

        [Space]
        
        [SerializeField] private IntVariable difficultyAtom;
        [SerializeField] private FloatVariable healthAtom;

        [Space]
        
        [SerializeField] private TransitionResolver transitionResolver = default;
        [SerializeField] private GameContextHandler gameContextHandler;
        
        [Space]
        
        [SerializeField] private Animator transitionScreenAnimator = default;
        private readonly int fadeInId = Animator.StringToHash("FadeIn");
        private readonly int fadeOutId = Animator.StringToHash("FadeOut");
        private readonly int isNormalTransitionId = Animator.StringToHash("IsNormalTransition");
        
        [Space(15)]
        
        private IGameSequence currentSequence = default;
        public IGameSequence CurrentSequence => currentSequence;

        [ShowInInspector, ReadOnly] private GameID previousGame = default;
        [ShowInInspector, ReadOnly] private GameID currentGame = default; 
        public GameID CurrentGame => currentGame;

        private AsyncOperation loadOperation = default;
        [ShowInInspector, ReadOnly] private bool isFinished = true;
        public bool IsFinished => isFinished;


        public void UpdateCurrentGameData(bool hasBeenWon)
        {
            currentGame.IncrementPlayCount((Difficulty) (difficultyAtom.Value - 1), hasBeenWon);
        }
        
        public void StartNewSequence(Object newSequence)
        {
            if(newSequence is IGameSequence sequence) StartNewSequence(sequence);
        }
        public void StartNewSequence(IGameSequence newSequence)
        {
            if (!isFinished) return;
            
            onSequenceOpenedAtomEvent.Raise();
            
            loadOperation = null;
            sequenceAdvancementAtom.SetValue(1);
            
            currentSequence = newSequence;
            currentSequence.OpeningTransitionEvent.Raise();
            isFinished = false;
            
            currentSequence.Initialize();
            currentGame = currentSequence.FirstGame;
            transitionScreenAnimator.SetTrigger(fadeInId);
        }
        
        public void CloseCurrentSequence()
        {
            healthAtom.SetValue(0);
            EndCurrentSequence();
        }
        public void EndCurrentSequence()
        {
            if (isFinished) return;
            
            isFinished = true;
            
            transitionScreenAnimator.SetTrigger(fadeInId);
            transitionScreenAnimator.SetTrigger(isNormalTransitionId);
        }

        public void PlayNextGame()
        {
            if (isFinished) return;
            
            if (currentSequence.TryGetNextGame(out var newGame))
            {
                loadOperation = null;
                sequenceAdvancementAtom.ApplyChange(1);
                
                previousGame = currentGame;
                currentGame = newGame;
                
                transitionScreenAnimator.SetTrigger(fadeInId);
            }
            else EndCurrentSequence();
        }

        public void OnTransitionStart()
        {
            if (isFinished) return;
            
            transitionResolver.ResolveSequenceStart();
            previousGame?.Scene.UnLoad();
            
            currentSequence.TryCallTransitionEvent();
            
            if (loadOperation != null) return;
            gameContextHandler.SetCurrentState(GameStates.Loading);
            
            loadOperation = currentGame.Scene.LoadAsync(LoadSceneMode.Additive);
            loadOperation.allowSceneActivation = false;
        }
        public void OnSequenceClosed()
        {
            currentGame.Scene.UnLoad();
            currentSequence.ClosingTransitionEvent.Raise();
            
            transitionScreenAnimator.SetTrigger(fadeOutId);
            previousGame = null;
        }
        public void OnTransitionEnd() =>  StartCoroutine(AllowSceneActivationRoutine());
        
        private IEnumerator AllowSceneActivationRoutine()
        {
            while (loadOperation.progress < 0.88f) yield return null;
            
            yield return new WaitForEndOfFrame();
            
            gameContextHandler.SetCurrentState(GameStates.Loaded);
            transitionScreenAnimator.SetTrigger(fadeOutId);
            loadOperation.allowSceneActivation = true;
        }
    }
}