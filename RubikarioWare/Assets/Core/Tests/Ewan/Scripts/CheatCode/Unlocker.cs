using System;
using System.Linq;

using UnityEngine;

using Sirenix.OdinInspector;

namespace Game.Core
{
    public class Unlocker : MonoBehaviour
    {
        [SerializeField] private AssetGroup gameIds;
        private GameID[] games = default;

        private void Awake() => games = gameIds.GetAssets<GameID>().ToArray();

        [Button]
        public void UnlockAllGame()
        {
            foreach (var game in games)
            {
                game.IncrementPlayCount(Difficulty.Easy, true);
                game.IncrementPlayCount(Difficulty.Medium, true);
                game.IncrementPlayCount(Difficulty.Hard, true);
            }
        }
    }
}