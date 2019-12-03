using UnityEngine;

namespace Game.HumanTower
{
	public class GizmosSphere : MonoBehaviour
	{
#if UNITY_EDITOR
		[SerializeField] Color m_color = Color.white;
		[SerializeField] float m_size = .5f;
		[SerializeField] bool m_localPosition = false;

		private void OnDrawGizmos()
		{
			Gizmos.color = m_color;
			Vector3 position = Vector3.zero;
			if (m_localPosition)
				position = transform.localPosition;
			else
				position = transform.position;
			Gizmos.DrawSphere(position, m_size);
		}
#endif
	}
}