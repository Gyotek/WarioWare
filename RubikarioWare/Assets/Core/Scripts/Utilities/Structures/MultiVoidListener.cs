using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class MultiVoidListener : MonoBehaviour
    {
        [SerializeField] private VoidEvent[] voidEvents;
        [SerializeField] private UnityEvent unityEvent;

        void Start()
        {
            foreach (var voidEvent in voidEvents) voidEvent.Register(unityEvent.Invoke);
        }
    }
}