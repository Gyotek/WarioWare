using UnityEngine;

namespace Game.Editor
{
	using static AssetMenuUtils;
	[CreateAssetMenu(menuName = MacroMenu + nameof(Note))]
    public class Note : ScriptableObject
    {
#pragma warning disable IDE0051 // Supprimer les membres privés non utilisés 
#pragma warning disable CS0414 // Supprimer les membres privés non utilisés 
		[SerializeField]
		private Object to = default;

		[SerializeField]
		private int line = default;

		[SerializeField]
		private string date = default;
		[Multiline]
		[TextArea(10, 20)]
		[SerializeField]
		private string commentary = default;

		[SerializeField]
		private string from = default;

		[SerializeField]
		private bool seen = default;
#pragma warning restore IDE0051 // Supprimer les membres privés non utilisés
#pragma warning restore CS0414 // Supprimer les membres privés non utilisés
	}
}