using UnityEngine;

namespace Game
{
	public class GizmosSphere : MonoBehaviour
	{
#if UNITY_EDITOR
		[SerializeField] Color color = Color.white;
		[SerializeField] float size = .5f;
		[SerializeField] bool localPosition = false;

		private void OnDrawGizmos()
		{
			Gizmos.color = color;
			Vector3 position;
			if (localPosition)
				position = transform.localPosition;
			else
				position = transform.position;
			Gizmos.DrawSphere(position, size);
		}
#endif
	}
}