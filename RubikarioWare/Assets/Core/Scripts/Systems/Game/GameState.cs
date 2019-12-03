using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
	internal class GameState : MonoBehaviour
	{
		[SerializeField] private VoidEvent onGameStart = default;

        private string winState = "Unspecified";
        public string WinState => winState;

        private string state = "Not Loaded";
        public string State => state;

        public void SetCurrentState(string newState)
        {
            if (newState != state)
            {
                state = newState;
				if (state.Equals("Started"))
					onGameStart.Raise();
            }
        }

        public void SetCurrentWinState(string newWinState)
        {
            if (winState != newWinState)
            {
                winState = newWinState;
			}
        }
    }
}