using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core
{
    public class TimeSpeedSetter : MonoBehaviour
    {
        [SerializeField, MinValue(0)] private float timeScale;
        
        [Button]
        private void Refresh()
        {
            Time.timeScale = timeScale;
        }
    }
}