using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Playlist
{
    public class ListUnlockedGamePlaylist : MonoBehaviour
    {
        [SerializeField] private AssetGroup gameIds;

        [SerializeField]
        private GameObject interactibleIcon;

        [SerializeField]
        private Transform content;

        [SerializeField]
        private Transform contentorPlaylist;

        void Awake()
        {
            foreach (GameID game in gameIds.GetAssets<GameID>())
            {
                if (game.GetPlayCount() <= 0) continue;

                var microScript = Instantiate(interactibleIcon, content).GetComponent<AddGameIdPlaylist>();
                microScript.thisGame = game;
                microScript.content = contentorPlaylist;
            }
        }
    }
}
