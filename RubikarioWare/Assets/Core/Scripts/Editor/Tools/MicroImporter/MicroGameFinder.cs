using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Editor
{
    public static class MicroGameFinder
    {
        public static string GetAssetName(this MicroGame _game)
        {
            string index = (int)_game < 10 ? "0" + ((int)_game).ToString() : ((int)_game).ToString();
            return index + "_" + _game.ToString();
        }

        public static MicroGame GuessMicrogame(this string _assetName)
        {
            string assetName = _assetName.Replace(".unitypackage", "").ToLower();

            float bestScore = 0.0f;
            MicroGame result = MicroGame.ChewingGOUM;

            for (int i = 1; i < 83; i++)
            {
                int score = 0;
                string game = (((MicroGame)i).ToString()).ToLower();

                for (int c = 0; c < game.Length; c++)
                {
                    if (assetName.Contains(game[c].ToString())) score++;
                }

                if((float)score / (float)game.Length > bestScore)
                {
                    bestScore = (float)score / (float)game.Length;
                    result = (MicroGame)i;
                }
            }

            Debug.Log(result.ToString() + " (" + bestScore + ")");
            return result;
        }
    }
}