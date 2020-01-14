using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Game.Core;

namespace Game.Playlist
{
    public class ModifyGameIdPlaylist : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector]
        public GameID thisGame;

        [HideInInspector]
        public int emplacementList;

        private void Start()
        {
            if (thisGame != null)
                GetComponent<Image>().sprite = thisGame.Thumbnail;
        }

        private void RemoveGame()
        {
            Debug.Log("Test2");
            PlaylistManager.RemoveGame(emplacementList);
            Destroy(gameObject);
        }

        private void ChangeDifficulty()
        {
            Debug.Log("Test3");
            PlaylistManager.ChangeDifficulty(emplacementList);
            //Changer l'affichage
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                RemoveGame();
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                ChangeDifficulty();
            }
        }
    }
}