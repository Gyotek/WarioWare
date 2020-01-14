using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms;

namespace Game.Core
{
    public class AffichageMenuMiniJeu : MonoBehaviour
    {
        private GameID currentgame;

        [SerializeField]
        private Image icone;

        [SerializeField]
        private Image input;

        [SerializeField]
        private SuperTextMesh nomDuJeu;

        [SerializeField]
        private List<Image> etoiles;

        [SerializeField]
        private List<Button> difficultyChoice;

        [SerializeField]
        private ToggleBoolAction toggle;

        [SerializeField]
        private IntVariable difficulty;

        [SerializeField]
        private IntVariable speed;

        public void ShowMenu(GameID nextGame)
        {
            if(currentgame != nextGame)
            {
                difficulty.Value = difficulty.InitialValue;
                speed.Value = speed.InitialValue;

                if (currentgame != null)
                {
                    toggle.Do();
                }

                currentgame = nextGame;

                icone.sprite = nextGame.Thumbnail;
                nomDuJeu.text = nextGame.GetLanguageSpecificInfo(Language.English)[0];

                if(nextGame.GetWinCount(Difficulty.Easy) > 0)
                {
                    etoiles[0].gameObject.SetActive(true);
                    difficultyChoice[0].interactable = true;
                }
                else
                {
                    etoiles[0].gameObject.SetActive(false);
                    difficultyChoice[0].interactable = false;
                }

                if (nextGame.GetWinCount(Difficulty.Easy) > 0)
                {
                    etoiles[1].gameObject.SetActive(true);
                    difficultyChoice[1].interactable = true;
                }
                else
                {
                    etoiles[1].gameObject.SetActive(false);
                    difficultyChoice[1].interactable = false;
                }

                if (nextGame.GetWinCount(Difficulty.Hard) > 0)
                {
                    etoiles[2].gameObject.SetActive(true);
                    difficultyChoice[2].interactable = true;
                }
                else
                {
                    etoiles[2].gameObject.SetActive(false);
                    difficultyChoice[2].interactable = false;
                }
            }
        }
    }
}
