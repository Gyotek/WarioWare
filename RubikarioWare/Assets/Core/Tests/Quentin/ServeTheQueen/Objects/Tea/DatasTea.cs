using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ServeTheQueen
{
    [CreateAssetMenu(menuName = "Wario Ware/Micro/ServeTheQueen/TeaDatas")]
    public class DatasTea : ScriptableObject
    {
        public float limitMin;
        public float limitMax;
        public float offset;
        public bool hasWin;
        public bool hasLoose;
        public Transform prefabLimitMin;
        public Transform prefabLimitMax;

        public void Reset()
        {
            hasWin = false;
            hasLoose = false;
        }

    }
}

