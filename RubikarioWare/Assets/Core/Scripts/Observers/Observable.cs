using UnityEngine;

namespace Game
{
	public class Observable : MonoBehaviour
	{
		public bool IsObservable { get; set; } = true;

		public virtual void Notify()
		{
			if (!IsObservable)
				return;
		}
	}
}