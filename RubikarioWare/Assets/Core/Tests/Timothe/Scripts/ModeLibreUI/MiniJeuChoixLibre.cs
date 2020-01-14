using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class MiniJeuChoixLibre : MonoBehaviour
    {
        public GameID thisGame;

        public AffichageMenuMiniJeu affMini;
        
        private void Start()
        {
            if (thisGame != null)
                GetComponent<Image>().sprite = thisGame.Thumbnail;
        }

        public void ShowGame()
        {
            if (thisGame != null)
            {
                affMini.ShowMenu(thisGame);
            }
        }
    }
}
