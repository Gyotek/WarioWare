using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
	public class OnEnterEvent : StateMachineBehaviour
	{
		[SerializeField] UnityEvent onEnter = default;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> onEnter?.Invoke();
	}
}