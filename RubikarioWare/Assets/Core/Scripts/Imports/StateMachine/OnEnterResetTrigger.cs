using UnityEngine;

namespace StateMachine
{
	public class OnEnterResetTrigger : StateMachineBehaviour
	{
		[SerializeField] private string parameter = string.Empty;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> animator?.ResetTrigger(parameter);
	}
}