using UnityEngine;

namespace StateMachine
{
	public class OnExitSetBool : StateMachineBehaviour
	{
		[SerializeField] string parameter = string.Empty;
		[SerializeField] bool value = false;

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> animator?.SetBool(parameter, value);
	}
}