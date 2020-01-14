using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core
{
    public class SetMenuMicroGame : MonoBehaviour
    {
        [SerializeField] private SuperTextMesh nameGame;

        public void SetGameID(GameID id)
        {
            nameGame.text = id.GetLanguageSpecificInfo(Language.English)[0];
        }

    }
}

