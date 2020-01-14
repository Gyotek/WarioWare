using UnityEngine;

namespace StateMachine
{
	public class InverseLerpParameter : StateMachineBehaviour
	{
		[SerializeField] string getParamater = "";
		[SerializeField] string setParamater = "";
		[Space]
		[SerializeField] float startValue = 0;
		[SerializeField] float endValue = 1;
		[Header("Settings")]
		[SerializeField] float setOffset = 0;
		[SerializeField] float setMultiplier = 1;

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float t = Mathf.InverseLerp(startValue, endValue, animator.GetFloat(getParamater));
			animator.SetFloat(setParamater, (t * setMultiplier) + setOffset);
		}
	}
}