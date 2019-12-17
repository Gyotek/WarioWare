using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dinventoryablo
{
    public class DinventoryabloGameManager : MicroMonoBehaviour
    {
        private bool gameEnded = false;
        [SerializeField] private string actionVerb;
        [SerializeField] private int actionVerbDuration;

        // Start is called before the first frame update
        void Start()
        {
            Macro.StartGame();
        }

        protected override void OnGameStart()
        {
            Macro.DisplayActionVerb(actionVerb, actionVerbDuration);
        }

        protected override void OnActionVerbDisplayEnd()
        {
            Macro.StartTimer(10, true);
        }

        protected override void OnTimerEnd()
        {
            FinishGame(false);
        }

        public void FinishGame(bool _win)
        {
            if (!gameEnded)
                StartCoroutine(FinishGameCoroutine(_win));
        }

        private IEnumerator FinishGameCoroutine(bool _win)
        {
            gameEnded = true;

            if (_win)
                Macro.Win();
            else
                Macro.Lose();

            yield return new WaitForSeconds(60f / (float)Macro.BPM * 4);

            Macro.EndGame();
        }

    }
}

