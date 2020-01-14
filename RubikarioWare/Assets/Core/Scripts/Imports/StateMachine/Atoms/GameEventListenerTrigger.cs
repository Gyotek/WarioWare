using UnityAtoms;
using UnityEngine;

namespace StateMachine.Atoms
{
	public class GameEventListenerTrigger : StateMachineBehaviour, IAtomListener<Void>
	{
		[Tooltip("Event to register with.")]
		[SerializeField] VoidEvent Event = default;

		[Tooltip("Triggered paramater")]
		[SerializeField] string paramater = string.Empty;

		private Animator animator = default;

		public override void OnStateEnter(
			Animator animator,
			AnimatorStateInfo stateInfo,
			int stateMachinePathHash)
		{
			Event.RegisterListener(this);
			this.animator = animator;
		}

		public override void OnStateExit(
			Animator animator,
			AnimatorStateInfo stateInfo,
			int stateMachinePathHash)
		{
			Event.UnregisterListener(this);
		}

		public void OnEventRaised(Void @void) => animator.SetTrigger(paramater);
	}
}