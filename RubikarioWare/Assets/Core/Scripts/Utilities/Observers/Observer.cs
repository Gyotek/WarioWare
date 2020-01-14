using UnityEngine;
using UnityEngine.Events;

namespace Game
{
	[System.Serializable]
	public class Collider2DUnityEvent : UnityEvent<Collider2D> { }

	public class Observer<T> : MonoBehaviour where T : Observable
	{
		[SerializeField]
		protected LayerMask mask = default;

		public Collider2DUnityEvent onNotify = default;

		protected virtual void OnTriggerEnter2D(Collider2D other) => OnTrigger(other);

		protected T GetObservable(Collider2D other)
		{
			if (!mask.Includes(other.gameObject.layer))
				return null;
			return other.GetComponentInParent<T>();
		}

		protected virtual void OnTrigger(Collider2D other)
		{
			var observable = GetObservable(other);
			if (observable == null) return;
			onNotify?.Invoke(other);
			Notify(observable);
		}

		protected virtual void Notify(T observable)
		{
			observable.Notify();
		}
	}
}