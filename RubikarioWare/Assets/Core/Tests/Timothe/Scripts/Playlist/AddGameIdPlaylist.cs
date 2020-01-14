using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;

namespace Game.Playlist
{
    public class AddGameIdPlaylist : MonoBehaviour
    {
        [HideInInspector]
        public GameID thisGame;

        [SerializeField]
        private GameObject interactibleIcon;

        [HideInInspector]
        public Transform content;

        private void Start()
        {
            if (thisGame != null)
                GetComponent<Image>().sprite = thisGame.Thumbnail;
        }

        public void AddGame()
        {
            if (PlaylistManager.CanAddGame())
            {
                var microScript = Instantiate(interactibleIcon, content).GetComponent<ModifyGameIdPlaylist>();
                microScript.thisGame = thisGame;
                microScript.emplacementList = PlaylistManager.currentList.Count;
                PlaylistManager.AddGame(thisGame, microScript);
            }
        }
    }
}
