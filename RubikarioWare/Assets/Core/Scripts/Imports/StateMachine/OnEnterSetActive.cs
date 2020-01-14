using UnityEngine;

namespace StateMachine
{
	public class OnEnterSetActive : StateMachineBehaviour
	{
		[SerializeField] bool setActive = false;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> animator?.gameObject.SetActive(setActive);
	}
}