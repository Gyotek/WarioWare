using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Core.Serialization;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using UnityAtoms;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class CheatSystem : MonoBehaviour
    {
        #region EncapsuledTypes

        [AttributeUsage(AttributeTargets.Method, Inherited =  false, AllowMultiple = false)]
        private class CommandAttribute : Attribute
        {
            public string commandName;

            public CommandAttribute(string commandName) => this.commandName = commandName;
        }

        #endregion
        
        [SerializeField] private TMP_InputField inputField;
        
        [Space]
        
        [SerializeField] private Image inputFieldBackground;
        [SerializeField] private Color normalColor, correctColor, incorrectColor;
        
        [Space]
        
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private GameSequenceHandler gameSequenceHandler;
        [SerializeField] private GameContextHandler gameContextHandler;

        [Space] 
        
        [SerializeField] private SceneField mainScene;
        [SerializeField] private AssetGroup gameGroup, rivalGroup;

        [Space] 
        
        [SerializeField] private IntVariable difficultyAtom;
        [SerializeField] private VoidEvent onInterfaceRefreshAtomEvent;

        private Scene currentMicroScene;

        private Dictionary<string, Action> commands;
        
        void Start()
        { 
            commands = new Dictionary<string, Action>();

            var attributes = new List<CommandAttribute>();
            var methods = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(method => HasCommandAttribute(method)).ToArray();

            for (var i = 0; i < methods.Length; i++)
            {
                commands.Add(ToInvariant(attributes[i].commandName), (Action)methods[i].CreateDelegate(typeof(Action), this));
            }

            bool HasCommandAttribute(MethodInfo methodInfo)
            {
                var commandAttribute = methodInfo.GetCustomAttribute<CommandAttribute>(false);
                if (commandAttribute != null)
                {
                    attributes.Add(commandAttribute);
                    return true;
                }
                else return false;
            }
                
            inputField.onSubmit.AddListener(CallCommand);
            inputField.onValueChanged.AddListener(CheckFieldCorrectness);
        }

        private string ToInvariant(string source) => source.ToLower().Replace(" ", "");

        private void CallCommand(string commandName)
        {
            if (commands.TryGetValue(ToInvariant(commandName), out var command)) command();
            
            inputField.SetTextWithoutNotify(string.Empty);
            inputFieldBackground.color = normalColor;
        }
        private void CheckFieldCorrectness(string commandName)
        {
            if (commands.ContainsKey(ToInvariant(commandName))) inputFieldBackground.color = correctColor;
            else  inputFieldBackground.color = incorrectColor;
        }

        [Command("invincible")]
        private void CallInvincibleCommand()
        {
            if (healthSystem.HealthAtomSettingCall == null) healthSystem.SubscribeHealthAtom();
            else healthSystem.UnsubscribeHealthAtom();
        } 
        [Command("gain life")]
        private void CallGainLifeCommand() => healthSystem.Hurt(-1);
        [Command("lose life")]
        private void CallLoseLifeCommand() => healthSystem.Hurt(1);
        [Command("reset health")]
        private void CallResetHealthCommand() => healthSystem.ResetLife();
        
        [Command("lose")]
        private void CallLoseCurrentGameCommand()
        {
            if (gameSequenceHandler.IsFinished) return;
            
            var state = (int) gameContextHandler.CurrentGameState;
            if (state <= 3 && state >= 5) return;
            
            Macro.Lose();
            Macro.EndGame();
        }
        [Command("win")]
        private void CallWinCurrentGameCommand()
        {
            if (gameSequenceHandler.IsFinished) return;
            
            var state = (int) gameContextHandler.CurrentGameState;
            if (state <= 3 && state >= 5) return;
            
            Macro.Win();
            Macro.EndGame();
        }
        [Command("end")]
        private void CallEndCurrentGameCommand()
        { 
            if (gameSequenceHandler.IsFinished) return;
            if ((int)gameContextHandler.CurrentGameState > 3) Macro.EndGame();
        }
        
        [Command("lose all")]
        private void CallLoseAllCurrentGameCommand()
        {
            if (gameSequenceHandler.IsFinished) return;
            
            var state = (int) gameContextHandler.CurrentGameState;
            if (state <= 3 && state >= 5) return;
            
            var sequence = gameSequenceHandler.CurrentSequence;
            var index = sequence.Advancement;

            for (var i = index; i < sequence.Games.Length; i++)
            {
                sequence.Games[i].IncrementPlayCount((Difficulty)(difficultyAtom.Value - 1), false);
            }
            gameSequenceHandler.CloseCurrentSequence();
        }
        [Command("win all")]
        private void CallWinAllCurrentGameCommand()
        {
            if (gameSequenceHandler.IsFinished) return;
            
            var state = (int) gameContextHandler.CurrentGameState;
            if (state <= 3 && state >= 5) return;

            var sequence = gameSequenceHandler.CurrentSequence;
            var index = sequence.Advancement;

            for (var i = index; i < sequence.Games.Length; i++)
            {
                sequence.Games[i].IncrementPlayCount((Difficulty)(difficultyAtom.Value - 1), true);
            }
            gameSequenceHandler.EndCurrentSequence();
        }
        [Command("end all")]
        private void CallEndAllCurrentGameCommand()
        { 
            if (gameSequenceHandler.IsFinished) return;
            if ((int)gameContextHandler.CurrentGameState > 3) gameSequenceHandler.CloseCurrentSequence();
        }

        [Command("lock all game")]
        private void CallLockAllGameCommand()
        {
            var games = gameGroup.GetAssets<GameID>();
            foreach (var game in games)
            {
                var cheatAccessible = game as ICheatAccessible;

                var playCounts = (int[]) cheatAccessible.objects[0];
                for (var i = 0; i < playCounts.Length; i++) playCounts[i] = 0;
                var winCounts = (int[]) cheatAccessible.objects[1];
                for (var i = 0; i < winCounts.Length; i++) winCounts[i] = 0;
            }
            onInterfaceRefreshAtomEvent.Raise();
        }
        [Command("unlock all game")]
        private void CallUnlockAllGameCommand()
        {
            var games = gameGroup.GetAssets<GameID>();
            foreach (var game in games)
            {
                var cheatAccessible = game as ICheatAccessible;

                var playCounts = (int[]) cheatAccessible.objects[0];
                for (var i = 0; i < playCounts.Length; i++) playCounts[i] = 1;
                var winCounts = (int[]) cheatAccessible.objects[1];
                for (var i = 0; i < winCounts.Length; i++) winCounts[i] = 1;
            }
            onInterfaceRefreshAtomEvent.Raise();
        }

        [Command("lock all rival")]
        private void CallLockAllRivalCommand()
        {
            var storySequences = rivalGroup.GetAssets<StorySequence>().ToList();
            storySequences.RemoveAt(0);
            
            foreach (var storySequence in storySequences) storySequence.isAccessible = false;
            onInterfaceRefreshAtomEvent.Raise();
        }
        [Command("unlock all rival")]
        private void CallUnlockAllRivalCommand()
        {
            var storySequences = rivalGroup.GetAssets<StorySequence>();
            foreach (var storySequence in storySequences) storySequence.isAccessible = true;
            onInterfaceRefreshAtomEvent.Raise();
        }
        
        [Command("finish all rival")]
        private void CallFinishAllRivalCommand()
        {
            var storySequences = rivalGroup.GetAssets<StorySequence>();
            foreach (var storySequence in storySequences) storySequence.hasBeenPerfectlyCompleted = true;
            onInterfaceRefreshAtomEvent.Raise();
        }
        [Command("unfinish all rival")]
        private void CallUnfinishAllRivalCommand()
        {
            var storySequences = rivalGroup.GetAssets<StorySequence>();
            foreach (var storySequence in storySequences) storySequence.hasBeenPerfectlyCompleted = false;
            onInterfaceRefreshAtomEvent.Raise();
        }

        [Command("lock all")]
        private void CallLockAllCommand()
        {
            CallLockAllGameCommand();
            CallLockAllRivalCommand();
            CallUnfinishAllRivalCommand();
        }
        [Command("unlock all")]
        private void CallUnlockAllCommand()
        {
            CallUnlockAllGameCommand();
            CallUnlockAllRivalCommand();
            CallFinishAllRivalCommand();
        }

        [Command("save")]
        private void CallSaveCommand()
        {
            var games = gameGroup.GetAssets<GameID>();
            foreach (var game in games) SaveSystem.SaveData(game.GetInstanceID(), "Games");

            var storySequences = rivalGroup.GetAssets<StorySequence>();
            foreach (var storySequence in storySequences) SaveSystem.SaveData(storySequence.GetInstanceID(), "StorySequences");
            
            onInterfaceRefreshAtomEvent.Raise();
        }
        
        [Command("load")]
        private void CallLoadCommand()
        {
            var games = gameGroup.GetAssets<GameID>();
            foreach (var game in games) SaveSystem.LoadData(game.GetInstanceID(), "Games");

            var storySequences = rivalGroup.GetAssets<StorySequence>();
            foreach (var storySequence in storySequences) SaveSystem.LoadData(storySequence.GetInstanceID(), "StorySequences");
            
            onInterfaceRefreshAtomEvent.Raise();
        }
        
        [Command("restart")]
        private void CallRestartCommand()
        {
            var operations = new List<AsyncOperation>();
            
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (!scene.isSubScene) continue;

                var operation = SceneManager.UnloadSceneAsync(scene);
                operation.completed += op => operations.Remove(op);
                operations.Add(operation);
            }
            
            if (operations.Count > 0) StartCoroutine(RestartRoutine(operations));
            else
            {
                mainScene.Load(LoadSceneMode.Single);
                Time.timeScale = 1;
            }
        }

        private IEnumerator RestartRoutine(List<AsyncOperation> operations)
        {
            while (operations.Count > 0) yield return null;
            
            mainScene.Load(LoadSceneMode.Single);
            Time.timeScale = 1;
        }
    }
}