using UnityEngine;

namespace StateMachine
{
	public class OnExitSetFloat : StateMachineBehaviour
	{
		[SerializeField] string parameter = string.Empty;
		[SerializeField] float value = 0f;

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> animator?.SetFloat(parameter, value);
	}
}