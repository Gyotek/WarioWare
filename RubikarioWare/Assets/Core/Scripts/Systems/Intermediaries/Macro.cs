using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Game.Core;

using Object = UnityEngine.Object;

namespace Game
{
    public class Macro : MonoBehaviour
    {
        #region EncapsuledTypes

        [AttributeUsage(AttributeTargets.Field, Inherited =  false, AllowMultiple = false)]
        private class InstancedReferenceAttribute : Attribute { }

        #endregion

        [SerializeField] private Object[] references;
        const BindingFlags Bindings = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
        
        [InstancedReference] private static BeatSystem beatSystem;
        public static int BPM => beatSystem.BpmAtom.Value;
        public static float TimeSinceLastBeat => beatSystem.TimeSinceLastBeatAtom.Value;
        public static float TimeBeforeNextBeat => beatSystem.TimeBeforeNextBeatAtom.Value;   
        public static int BeatCount => beatSystem.BeatCountAtom.Value;
        
        [InstancedReference] private static GameContextHandler gameContextHandler;
        public static int Difficulty => gameContextHandler.DifficultyAtom.Value;
        
        [InstancedReference] private static TimerWrapper timerWrapper;
        public static float RemainingTime => timerWrapper.Value;
        
        [InstancedReference] private static VerbDisplay verbDisplay;
        [InstancedReference] private static AudioManager audioManager;

        void Awake()
        {
            var typeValuePairs = new Dictionary<Type, Object>();
            foreach (var reference in references)
            {
                var referenceType = reference.GetType();
                if (reference != null && !typeValuePairs.ContainsKey(referenceType))
                {
                    typeValuePairs.Add(referenceType, reference);
                }
            }
            
            var fields = from field in typeof(Macro).GetFields(Bindings) 
                where field.GetCustomAttribute<InstancedReferenceAttribute>() != null select field;

            foreach (var field in fields)
            {
                if (!typeValuePairs.ContainsKey(field.FieldType)) continue;
                field.SetValue(null, typeValuePairs[field.FieldType]);
            }
        }

        private static bool CanStateBePassed(GameStates gameState)
        {
            return (int)gameState > (int)gameContextHandler.CurrentGameState;
        }
        
        public static void StartGame()
        {
            if (!CanStateBePassed(GameStates.Starting)) return;
            
            gameContextHandler.SetCurrentState(GameStates.Starting);
            beatSystem.QueueCall(1, DelayedCall);
            
            void DelayedCall()
            {
                beatSystem.BeatCountAtom.SetValue(0);
                gameContextHandler.SetCurrentState(GameStates.Started);
            }
        }

        public static void EndGame()
        {
            if (!CanStateBePassed(GameStates.Ending)) return;
            
            gameContextHandler.SetCurrentState(GameStates.Ending);
            beatSystem.QueueCall(1, DelayedCall);
            
            void DelayedCall()
            {
                gameContextHandler.SetCurrentState(GameStates.Ended);
                if (!timerWrapper.IsComplete) timerWrapper.Complete();
            }
        }

        public static void Win()
        {
            if (!CanStateBePassed(GameStates.Won) || gameContextHandler.CurrentGameState == GameStates.Lost) return;
            
            gameContextHandler.SetCurrentState(GameStates.Won);
        }
        public static void Lose()
        { 
            if (!CanStateBePassed(GameStates.Lost) || gameContextHandler.CurrentGameState == GameStates.Won) return;
            
            gameContextHandler.SetCurrentState(GameStates.Lost);
        }
        
        public static void StartTimer(float duration, bool isAffectedByBpm)
        {
            duration = isAffectedByBpm ? AffectTimeByBpm(duration) : duration;
            timerWrapper.StartTimer(duration);
        }
        public static void StartTimer(int beatDuration) => timerWrapper.StartTimer(ConvertBeatsToTime(beatDuration));
        
        public static float AffectTimeByBpm(float time) => beatSystem.SecondsPerBeat * time;
        public static float ConvertBeatsToTime(int beatDuration) => beatDuration / 0.625f * beatSystem.SecondsPerBeat;
        
        public static void DisplayActionVerb() => verbDisplay.BeginDisplay();
        public static void DisplayActionVerb(string verb) =>  verbDisplay.BeginDisplay(verb);
        public static void DisplayActionVerb(string verb, int beatDuration)
        {
            var time = ConvertBeatsToTime(beatDuration);
            verbDisplay.BeginDisplay(verb, time);
        }

        public static void PlayMicroGameSoundtrack(float transitionTime, int musicBpm, float timeBeforeMusicFirstBeat, AudioSource audioSource)
        {
            audioManager.PlaySoundtrack(transitionTime, musicBpm, timeBeforeMusicFirstBeat, audioSource);
        }
        public static void PlayMicroGameSoundtrack(int musicBpm, float timeBeforeMusicFirstBeat, AudioSource audioSource)
        {
            audioManager.PlaySoundtrack(-1f, musicBpm, timeBeforeMusicFirstBeat, audioSource);
        }
    }
}