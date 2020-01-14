using UnityEngine;

namespace Game
{
	public static class LayerMaskUtility
	{
		public static LayerMask ToLayerMask(int layer)
		{
			return 1 << layer;
		}

		public static bool Includes(this LayerMask mask, int layer)
		{
			return (mask.value & 1 << layer) > 0;
		}
	}
}