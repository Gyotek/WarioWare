using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core
{
    public class CallbackManager : Singleton<CallbackManager>
    {
        protected CallbackManager() { }

        public bool AllowCallbacks { get; set; } = true;

        [SerializeField, ReadOnly] private List<MicroMonoBehaviour> subscribers
            = new List<MicroMonoBehaviour>();
        
        private Dictionary<MicroMonoBehaviour, Dictionary<string, Action>> actions 
            = new Dictionary<MicroMonoBehaviour, Dictionary<string, Action>>();

        public void Subscribe(MicroMonoBehaviour subscriber, Action[] callbacks)
        {
            if (subscribers.Contains(subscriber)) return;
            
            subscribers.Add(subscriber);
            var actionsByNames = callbacks.ToDictionary(callback => callback.Method.Name);
            actions.Add(subscriber, actionsByNames);
        }
        public void Unsubscribe(MicroMonoBehaviour subscriber)
        {
            if (subscribers.Remove(subscriber)) actions.Remove(subscriber);
        }

        private void LaunchCallback(string key)
        {
            if (!AllowCallbacks) return;
            
            foreach (var subscriber in subscribers) actions[subscriber][key]();
        }

        public void CallOnGameStart() => LaunchCallback("OnGameStart");
        public void CallOnBeat() => LaunchCallback("OnBeat");
        public void CallOnActionVerbDisplayEnd() => LaunchCallback("OnActionVerbDisplayEnd");
        public void CallOnTimerEnd() => LaunchCallback("OnTimerEnd");
    }
}