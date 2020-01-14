using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dinventoryablo
{
    public class DinventoryabloGameManager : MicroMonoBehaviour
    {
        public static DinventoryabloGameManager instance;
        private void Awake() { instance = this; }

        private bool gameEnded = false;
        public bool gameStarted = false;
        [SerializeField] private string actionVerb;
        [SerializeField] private int actionVerbDuration;

        [SerializeField] private GameObject[] listeLD;

        [SerializeField] private GameObject chestOpen;
        [SerializeField] private GameObject chestClosed;
        [SerializeField] private GameObject chestInventory;
        [SerializeField] private ParticleSystem coinParticle;
        
        // Start is called before the first frame update
        void Start()
        {
            Macro.StartGame();
        }

        protected override void OnGameStart()
        {
            chestClosed.SetActive(false);
            chestOpen.SetActive(true);
            chestInventory.SetActive(true);
            coinParticle.Play();
            Macro.DisplayActionVerb(actionVerb, actionVerbDuration);
        }

        protected override void OnActionVerbDisplayEnd()
        {

            gameStarted = true;
            int randomLD = Random.Range(0, 2);
            if (Macro.Difficulty == 2)
                randomLD += 3;
            else if (Macro.Difficulty == 3)
                randomLD += 6;

            listeLD[randomLD].SetActive(true);
            Macro.StartTimer(10, true);
        }

        protected override void OnTimerEnd()
        {
            gameStarted = false;
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

