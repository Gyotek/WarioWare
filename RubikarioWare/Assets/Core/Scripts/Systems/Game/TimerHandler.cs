using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
	internal class TimerHandler : MonoBehaviour
	{
		[SerializeField] FloatVariable outputRemainingTime = default;
		public UnityEvent OnComplete = default;

		public bool IsComplete => timer.IsComplete;

		Timer timer = new Timer(0f);

		public float Value => timer.Value;

        private void Awake()
        {
            OnComplete.AddListener(new UnityAction(() => outputRemainingTime.SetValue(-1)));
            timer.Assign(OnComplete.Invoke);
        }

		private void Update()
		{
			timer.Tick(Time.deltaTime);
            if (!timer.IsComplete) outputRemainingTime.SetValue(timer.goal - timer.Value);
		}

		public void StartTimer(float value)
		{
			if (!timer.IsComplete)
				throw new System.Exception("StartTimer can't be called multiple times");
			timer.Start(value);
		}

        public void Complete()
        {
            outputRemainingTime.SetValue(-1);
            timer.Complete();
        }
	}
}