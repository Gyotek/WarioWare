using UnityEngine;

namespace Game
{
	public static class GameObjectUtils
	{
		public static void SetActiveAll(this GameObject[] gameObjects, bool value)
		{
			int length = gameObjects.Length;
			for (int i = length - 1; i >= 0; i--)
			{
				if (gameObjects[i] == null) continue;
				gameObjects[i].SetActive(value);
			}
		}
	}
}