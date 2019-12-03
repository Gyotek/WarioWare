using UnityEngine;
using Game.Core;
using UnityAtoms;

namespace Game
{
    public class Macro : MonoBehaviour
    {
		[SerializeField] IntVariable difficulty = default;

        [SerializeField] BeatEngine beatEngine = default;
		[SerializeField] VerbLoader verbLoader = default;
		[SerializeField] GameState gameState = default;
		[SerializeField] TimerHandler timerHandler = default;

        private static IntVariable _difficulty = default;
        private static BeatEngine _beatEngine = default;
		private static VerbLoader _verbLoader = default;
		private static GameState _gameState = default;
		private static TimerHandler _timerHandler = default;

		public static int Difficulty => _difficulty.Value;
		public static int BeatCount => BeatEngine.BeatsSinceStart;
		public static int BPM => BeatEngine.CurrentBpm;
		public static float RemainingTime => _timerHandler.Value;
		public static double TimeSinceLastBeat => BeatEngine.TimeSinceLast;
		public static double TimeBeforeNextBeat => BeatEngine.TimeBeforeNext;

		private void Awake() => Initialize();

		private void Initialize()
		{
			_beatEngine = beatEngine;
			_verbLoader = verbLoader;
			_gameState = gameState;
			_timerHandler = timerHandler;
			_difficulty = difficulty;
		}

		public static void StartGame()
        {
            _gameState.SetCurrentState("Starting");
            _beatEngine.PlayActionOnBeat(() => 
            {
                BeatEngine.BeatsSinceStart = 0;
                _gameState.SetCurrentState("Started");
            });
        }	
        public static void EndGame()
        {
            _gameState.SetCurrentState("Ending");
            _beatEngine.PlayActionOnBeat(() => 
            {
                _gameState.SetCurrentState("Ended");
                CallbackManager.Instance.allowCallbacks = false;
				if (!_timerHandler.IsComplete)
					_timerHandler.Complete();
			});
		}

		public static void Win() => _gameState.SetCurrentWinState("Won");
        public static void Lose() => _gameState.SetCurrentWinState("Lost");

        public static void StartTimer(float time, bool bpmAffect) => _timerHandler.StartTimer(bpmAffect ? AffectTimeByBPM(time) : time);
        public static void StartTimer(int beatDuration) => _timerHandler.StartTimer(ConvertBeatsToTime(beatDuration));

        public static float AffectTimeByBPM(float time) => (float)BeatEngine.BeatLength * time;
        public static float ConvertBeatsToTime(int beatDuration) => (float)BeatEngine.BeatLength * beatDuration;

        public static void DisplayActionVerb() => _verbLoader.ShowVerb();
        public static void DisplayActionVerb(string verb) => _verbLoader.ShowVerb(verb);
        public static void DisplayActionVerb(string verb, int beatDuration) => _verbLoader.ShowVerb(verb, ConvertBeatsToTime(beatDuration));
    }
}