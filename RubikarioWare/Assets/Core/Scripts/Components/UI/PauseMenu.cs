using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPause;
        [SerializeField] private UnityEvent onUnpause;

        private bool isPaused = false;
        
        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            if (!isPaused) Pause();
            else Unpause();

        }

        public void Pause()
        {
            if (!isPaused) PauseGame();
        }
        private void PauseGame()
        {
            Time.timeScale = 0;
            onPause.Invoke();
            isPaused = true;
        }
        
        public void Unpause()
        {
            if (isPaused) UnpauseGame();
        }
        private void UnpauseGame()
        {
            Time.timeScale = 1;
            onUnpause.Invoke();
            isPaused = false;
        }
    }
}