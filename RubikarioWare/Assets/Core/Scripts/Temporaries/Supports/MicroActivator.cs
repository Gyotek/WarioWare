using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Core
{
    public class MicroActivator : MonoBehaviour
    {
        [SerializeField] private GameObject[] roots = default;

        void Awake()
        {
            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].activeInHierarchy)
                {
                    Debug.LogError("The defined GameObject Roots for the Micro-Game content are active at the Start of the game. Please set their Active State to False");
                    Debug.Break();
                }
            }
        }

        public void SetRootActives()
        {
            for (int i = 0; i < roots.Length; i++)
            {
                roots[i].SetActive(true);
            }
        }
    }
}