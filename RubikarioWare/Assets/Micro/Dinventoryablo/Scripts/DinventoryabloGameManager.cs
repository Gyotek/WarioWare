using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dinventoryablo
{
    public class DinventoryabloGameManager : MicroMonoBehaviour
    {
        public static DinventoryabloGameManager instance;
        private void Awake() { instance = this; }

        public bool gameEnded = false;
        public bool gameStarted = false;
        public bool gameLost = false;
        public bool gameWon = false;
        [SerializeField] private string actionVerb;
        [SerializeField] private int actionVerbDuration;

        [SerializeField] private GameObject[] listeLD;

        [SerializeField] private ParticleSystem coinParticle;

        int cursorID=1;
        [SerializeField] private SpriteRenderer Curseur;
        [SerializeField] private Camera cam;
        [SerializeField] private Sprite Curseur1;
        [SerializeField] private Sprite Curseur2;
        [SerializeField] private Sprite Curseur3;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
            Curseur.sprite = Curseur1;
            Macro.StartGame();
        }

        private void Update()
        {
            Curseur.transform.position = new Vector3 (cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, Curseur.transform.position.z);
        }

        public void SetCurseur(int ID)
        {
            /*if (cursorID == 3 && ID != 2 || gameEnded) return;
            else*/ cursorID = ID;
            switch (cursorID)
            {
                case 1:
                    Curseur.sprite = Curseur1;
                    //Cursor.SetCursor(Curseur1, Vector2.zero, CursorMode.Auto);
                    break;
                case 2:
                    Curseur.sprite = Curseur2;
                    //Cursor.SetCursor(Curseur2, Vector2.zero, CursorMode.Auto);
                    break;
                    /*
                case 3:
                    Curseur.sprite = Curseur3;
                    Cursor.SetCursor(Curseur3, Vector2.zero, CursorMode.Auto);
                    break;
                    */
            }

        }

        protected override void OnGameStart()
        {
            coinParticle.Play();
            Dinventoryablo.AudioManager.instance.PlaySFX(Dinventoryablo.AudioManager.SFX.OpenChest);
            Macro.DisplayActionVerb(actionVerb, actionVerbDuration);
        }

        protected override void OnActionVerbDisplayEnd()
        {

            gameStarted = true;
            int randomLD = Random.Range(0, 3);
            if (Macro.Difficulty == 2)
                randomLD += 3;
            else if (Macro.Difficulty == 3)
                randomLD += 6;

            listeLD[randomLD].SetActive(true);
            Macro.StartTimer(20, true);
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
            {
                Dinventoryablo.AudioManager.instance.PlaySFX(Dinventoryablo.AudioManager.SFX.Win);
                gameWon = true;
                Macro.Win();
            }
            else
                Macro.Lose();



            yield return new WaitForSeconds(60f / (float)Macro.BPM * 4);
            Cursor.visible = true;
            Macro.EndGame();
        }

    }
}

