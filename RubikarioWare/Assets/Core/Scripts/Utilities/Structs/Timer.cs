using System;

namespace Game
{
	[Serializable]
	public struct Timer
	{
		public float goal;

		public float Value { get; private set; }
		public bool IsComplete { get; private set; }
		public delegate void OnComplete();
		public OnComplete OnCompleted { get; private set; }

		public Timer(float timer)
		{
			this.goal = timer;
			Value = 0f;
			IsComplete = true;
			OnCompleted = default;
		}

		public void Assign(OnComplete onComplete) => OnCompleted = onComplete;
        public void Unassign() => OnCompleted = null;

		public void Start(float timer)
		{
			this.goal = timer;
			Value = 0f;
			IsComplete = false;
		}

		public void Tick(float value)
		{
			if (IsComplete)
				return;
			if (Value >= goal)
			{
                Complete();
                return;
			}
			Value += value;
		}

        public void Complete()
        {
            Value = goal;
            IsComplete = true;
            OnCompleted?.Invoke();
        }
	}
}