using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class EventWrapper : MonoBehaviour
    {
        [SerializeField] private UnityEvent unityEvent;

        public void Execute() => unityEvent.Invoke();
    }
}