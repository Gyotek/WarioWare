using UnityEngine;

namespace Game.ServeTheQueen
{
    public class Cinematic : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Events.UnityEvent OnCinematic;
        [SerializeField] private UnityEngine.Events.UnityEvent StartTheGame;


        public void InvokeOnCinematic()
        {
            OnCinematic.Invoke();
        }

        public void StartGame()
        {
            StartTheGame.Invoke();
        }


    }
}