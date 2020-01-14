using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public class OnEnterRaise : StateMachineBehaviour
    {
        [SerializeField] private VoidEvent[] events;
        
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            foreach (var onEnterEvent in events) onEnterEvent.Raise();
        }
    }
}

