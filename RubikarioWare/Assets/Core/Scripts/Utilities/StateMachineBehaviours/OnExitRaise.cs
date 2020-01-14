using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public class OnExitRaise : StateMachineBehaviour
    {
        [SerializeField] private VoidEvent[] events;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var onExitEvent in events) onExitEvent.Raise();
        }
    }
}