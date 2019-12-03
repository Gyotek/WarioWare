using System;
using UnityEngine;
using Game.Core;

namespace Game
{
    public abstract class MicroMonoBehaviour : MonoBehaviour
    {
        void OnEnable()
        {
            var callbacks = new Action[]
            {
                OnGameStart,
                OnBeat,
                OnActionVerbDisplayEnd,
                OnTimerEnd
            };
            CallbackManager.Instance?.Subscribe(this, callbacks);
        }
        private void OnDisable() => CallbackManager.Instance?.Unsubscribe(this);

        protected virtual void OnGameStart() { }
        protected virtual void OnBeat() { }
        protected virtual void OnActionVerbDisplayEnd() { }
        protected virtual void OnTimerEnd() { }
    }
}