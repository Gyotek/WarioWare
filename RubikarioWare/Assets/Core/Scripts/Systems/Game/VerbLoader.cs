using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
	[System.Serializable]
	public class StringUnityEvent : UnityEvent<string> { }

	internal class VerbLoader : MonoBehaviour
	{
		[SerializeField] float showDuration = default;

		public string CurrentVerb { get; private set; } = default;
		public string GameIDVerb => currentGameID.GetVerb();
		public bool IsVerbDisplayed => timer.IsComplete;

		public StringUnityEvent OnShow = default;
		public UnityEvent OnHide = default;

		GameID currentGameID = default;
		Timer timer = new Timer(0f);

		private void Awake() => timer.Assign(OnHide.Invoke);

		private void OnEnable() => GameIDChecker.OnGameIDCheck += SetCurrentGameID;

		private void OnDisable() => GameIDChecker.OnGameIDCheck -= SetCurrentGameID;

		private void Update() => timer.Tick(Time.deltaTime);

		private void SetCurrentGameID(GameID gameID) => currentGameID = gameID;

		public void ShowVerb()
		{
			OnShow?.Invoke(CurrentVerb);
			CurrentVerb = GameIDVerb;

			timer.Start(showDuration);
        }

		public void ShowVerb(string text)
		{
			OnShow?.Invoke(text);
			CurrentVerb = text;

			timer.Start(showDuration);
        }

		public void ShowVerb(string text, float showDuration)
		{
			OnShow?.Invoke(text);
			CurrentVerb = text;

			timer.Start(showDuration);
		}
	}
}