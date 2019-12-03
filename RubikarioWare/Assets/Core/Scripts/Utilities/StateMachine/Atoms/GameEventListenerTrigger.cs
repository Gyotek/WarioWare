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

		public override void OnStateMachineEnter(
			Animator animator,
			int stateMachinePathHash)
		{
			base.OnStateMachineEnter(animator, stateMachinePathHash);
			Event.RegisterListener(this);
			this.animator = animator;
		}

		public override void OnStateMachineExit(
			Animator animator,
			int stateMachinePathHash)
		{
			base.OnStateMachineExit(animator, stateMachinePathHash);
			Event.UnregisterListener(this);
		}

		public void OnEventRaised(Void @void) => animator.SetTrigger(paramater);
	}
}