using UnityAtoms;
using UnityEngine;

namespace StateMachine.Atoms
{
	public class OnUpdateSetBool : StateMachineBehaviour
	{
		[SerializeField] string paramater = string.Empty;
		[SerializeField] BoolVariable value = default;

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			=> animator?.SetBool(paramater, value?.Value ?? false);
	}
}