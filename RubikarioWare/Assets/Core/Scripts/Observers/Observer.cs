using UnityEngine;
using UnityEngine.Events;

namespace Game
{
	[System.Serializable]
	public class ColliderUnityEvent : UnityEvent<Collider> { }

	public class Observer<T> : MonoBehaviour where T : Observable
	{
		[SerializeField]
		protected LayerMask mask = default;

		public ColliderUnityEvent onNotify = default;

		protected virtual void OnTriggerEnter(Collider other) => OnTrigger(other);

		protected T GetObservable(Collider other)
		{
			if (!mask.Includes(other.gameObject.layer))
				return null;
			return other.GetComponentInParent<T>();
		}

		protected virtual void OnTrigger(Collider other)
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