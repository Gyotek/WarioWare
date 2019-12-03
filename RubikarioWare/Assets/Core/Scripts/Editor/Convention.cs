using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Convention : MonoBehaviour
	{
		[SerializeField] private string something = default;

		public UnityEvent onSomething = default;

		public int AnotherThing { get; private set; } = 0;
		public string Something => something;

		// please avoid managers :(
		public static Convention Instance { get; private set; } = default;
		// at least, or use singleton scriptableObject
		public static bool IsSomething { get; private set; } = false;

		private bool privateIsBottom = true;

		// Unity methods on top
        private void Start()
        {
			// hello I'm on top
			privateIsBottom = false;
            Debug.Log(privateIsBottom);
		}

		private void Update() => SomethingHandler();

		private void SomethingHandler()
		{
			// avoid all here
			StartCoroutine("HahaJ'utiliseLesStrings");
			FindObjectOfType<Convention>();
			GameObject.Find("Haha");
			GameObject.FindGameObjectWithTag("Haha");

			// try to avoid region use it if it's really better
			bool heavy = gameObject.tag == "banana";
			bool isBetter = gameObject.CompareTag("banana");

			// don't chain if
			if (heavy)
			{
				if (isBetter)
				{
					if (true)
					{
						PleaseEndMeNow();
					}
				}
			}

			// or at least try this, because it's clearer
			if (heavy)
				if (isBetter)
					if (true)
						PleaseEndMeNow();
		}

		private void PleaseEndMeNow() => Debug.Log("Dead");

		public void TriggerSomething()
		{
			if (something == string.Empty) return;
            // defensive programming, always check error first

            Debug.Log(something);
			onSomething?.Invoke();
		}

		public void SetAnotherThing(int value) => AnotherThing = value;
	}
}