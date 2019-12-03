using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Core
{
    public class CallbackManager : Singleton<CallbackManager>
    {
        protected CallbackManager() { }

        public bool allowCallbacks = true;

        private List<MicroMonoBehaviour> subscribers = new List<MicroMonoBehaviour>();
        private Dictionary<MicroMonoBehaviour, Dictionary<string, Action>> actions = new Dictionary<MicroMonoBehaviour, Dictionary<string, Action>>();

        public void Subscribe(MicroMonoBehaviour subscriber, Action[] callbacks)
        {
            if (!subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);

                var actionsByNames = new Dictionary<string, Action>();
                for (int i = 0; i < callbacks.Length; i++) actionsByNames.Add(callbacks[i].Method.Name, callbacks[i]);
                actions.Add(subscriber, actionsByNames);
            }
        }
        public void Unsubscribe(MicroMonoBehaviour subscriber)
        {
            if (subscribers.Remove(subscriber)) actions.Remove(subscriber);
        }

        private void LaunchCallback(string key)
        {
            if (allowCallbacks) for (int i = 0; i < subscribers.Count; i++) actions[subscribers[i]][key]();
        }

        public void CallOnGameStart() => LaunchCallback("OnGameStart");
        public void CallOnBeat() => LaunchCallback("OnBeat");
        public void CallOnActionVerbDisplayEnd() => LaunchCallback("OnActionVerbDisplayEnd");
        public void CallOnTimerEnd() => LaunchCallback("OnTimerEnd");
    }
}