using UnityEngine;

namespace UnityAtoms
{
    using static AtomMenu;
	[CreateAssetMenu(fileName = "Debug", menuName = Actions + "Debug", order = Order)]
	public class DebugAction : VoidAction
	{
		[SerializeField] LogType logType = LogType.Log;
		[SerializeField, Multiline] string message = string.Empty;
		[SerializeField] StringVariable otherMessage = default;

		[SerializeField] Object context = default;
		[SerializeField] BoolVariable assertCondition = default;

		public string OtherMessage
		{
			get
			{
				if (otherMessage == null)
					return "";
				return $" | {otherMessage.Value}";
			}
		}

		public override void Do()
		{
			switch (logType)
			{
				case LogType.Error:
					if(otherMessage == null)
					Debug.LogError($"{message}{OtherMessage}", context);
					break;
				case LogType.Assert:
					Debug.Assert(assertCondition.Value, $"{message}{OtherMessage}", context);
					break;
				case LogType.Warning:
					Debug.LogWarning($"{message}{OtherMessage}", context);
					break;
				case LogType.Log:
					Debug.Log($"{message}{OtherMessage}", context);
					break;
				case LogType.Exception:
					Debug.LogException(new System.Exception($"{message}{OtherMessage}"), context);
					break;
			}
		}
	}
}
