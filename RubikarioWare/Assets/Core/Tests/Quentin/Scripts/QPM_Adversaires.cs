using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core
{
	using static AssetMenuUtils;
    [CreateAssetMenu(menuName = MacroMenu + "QPM_Adversaire")]
    public class QPM_Adversaires : ScriptableObject
    {
        public List<GameID> gamesID;
        
    }
}