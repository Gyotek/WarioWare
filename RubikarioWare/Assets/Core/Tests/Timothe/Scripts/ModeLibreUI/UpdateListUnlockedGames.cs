using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UpdateListUnlockedGames : MonoBehaviour
    {
        [SerializeField]  private AssetGroup gameIds;
        
        [SerializeField]
        private GameObject interactibleIcon;

        [SerializeField]
        private Transform content;

        [SerializeField]
        private AffichageMenuMiniJeu affMini;
        
        void Awake()
        {
            foreach(GameID game in gameIds.GetAssets<GameID>())
            {
                if (game.GetPlayCount() <= 0) continue;
                
                var microScript = Instantiate(interactibleIcon, content).GetComponent<MiniJeuChoixLibre>();
                microScript.thisGame = game;
                microScript.affMini = affMini;
            }
        }
    }
}