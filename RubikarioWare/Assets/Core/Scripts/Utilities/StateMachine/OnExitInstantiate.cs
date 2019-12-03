using UnityEngine;

public class OnExitInstantiate : StateMachineBehaviour
{
	[SerializeField] GameObject prefab = default;

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		=> Instantiate(prefab, animator.transform.position, Quaternion.identity);
}
