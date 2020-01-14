using UnityEngine;

namespace StateMachine
{
	public class OnEnterSetFloat : StateMachineBehaviour
	{
		[SerializeField] private string parameterName = string.Empty;
		[SerializeField] private float value = 0f;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> animator?.SetFloat(parameterName, value);
	}
}