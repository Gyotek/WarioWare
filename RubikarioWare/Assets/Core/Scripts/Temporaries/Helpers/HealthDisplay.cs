using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    internal class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private List<Image> heartImages;

        public void Refresh(HealthSystem health)
        {
            for (var i = 1; i < heartImages.Count + 1; i++)
            {
                heartImages[i - 1].enabled = i <= health.CurrentHealth;
            }
        }
    }
}