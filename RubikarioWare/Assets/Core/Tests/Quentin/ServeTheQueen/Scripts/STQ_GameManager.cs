using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ServeTheQueen
{
    public class STQ_GameManager : MicroMonoBehaviour
    {
        [SerializeField] private DatasTea teaDatas;
        [SerializeField] private UnityEngine.Events.UnityEvent OnLoose;
        [SerializeField] private UnityEngine.Events.UnityEvent OnWin;
        [SerializeField] private UnityEngine.Events.UnityEvent OnGameStartEvent;

        void Start() => teaDatas.Reset();

        private void Update() 
        {
            if (teaDatas.hasLoose)
            {
                Debug.Log("heleled");
                OnLoose.Invoke();
            }
         
        }
        public void ShowVerb()
        {
            Macro.DisplayActionVerb("Fill it !");


        }

        protected override void OnActionVerbDisplayEnd()
        {
            Debug.Log("heleled");
            Macro.StartGame();
            
        }

        protected override void OnGameStart()
        {
            Macro.StartTimer(5,false);
            OnGameStartEvent.Invoke();
            Debug.Log("heleled");

        }

        protected override void OnTimerEnd()
        {
            if (teaDatas.hasWin)
            {
                OnWin.Invoke();
                Macro.Win();
                StartCoroutine(WaitEndGame());
            }
            else
            {
                OnLoose.Invoke();
                Macro.Lose();
                StartCoroutine(WaitEndGame());
            } 


        }

        IEnumerator WaitEndGame()
        {
            yield return new WaitForSeconds(4.5f);
            Macro.EndGame();
        }



    }

}

