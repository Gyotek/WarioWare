using System;
using Game;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class TimerWrapper : MonoBehaviour
    {
        [SerializeField] private FloatVariable timeGoalAtom;  
        [SerializeField] private FloatVariable timeAdvancementAtom = default;
        
        [Space]
        
        [SerializeField] private UnityEvent onComplete;
        [SerializeField] private UnityEvent onStart;

        private Timer timer = new Timer(0f);
        public bool IsComplete => timer.IsComplete;
        public float Value => timer.Value;


        void Awake()
        {
            onComplete.AddListener(() => timeAdvancementAtom.SetValue(-1));
            timer.Assign(onComplete.Invoke);
        }
        void Update()
        {
            timer.Tick(Time.deltaTime);
            if (!timer.IsComplete) timeAdvancementAtom.SetValue(timer.Value);
        }

        public void StartTimer(float time)
        {
            if (!timer.IsComplete) return;

            timeGoalAtom.SetValue(time);
            
            timer.Start(time);
            onStart.Invoke();
        }

        public void Complete()
        {
            if (timer.IsComplete) return;
            timer.Complete(true);
        }
        public void CompleteWithoutNotify()
        {
            if (timer.IsComplete) return;
            timer.Complete(false);
        }

        public void ResetTimer()
        {
            timer.Reset();
        }

        public override string ToString()
        {
            return this.name+"'s TimerWrapper component current value = "+Value;
        }
    }
}