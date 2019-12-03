using UnityAtoms;
using UnityEngine;

namespace StateMachine.Atoms
{
	public class OnEnterRaiseEvent : StateMachineBehaviour
	{
		[SerializeField] VoidEvent onEnter = default;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> onEnter?.Raise();
	}
}