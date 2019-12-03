//Copyright (c) Ewan Argouse - http://ewan.design/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ScriptTemplates.Editor
{
	static class ScriptTemplatesPreferences
	{
		public static string comment = "";
		private static bool prefsLoaded = false;

		[SettingsProvider]
		public static SettingsProvider CreateScriptTemplatesPreferencesProvider()
		{
			var provider = new SettingsProvider("Preferences/ScriptTemplatesPreferences", SettingsScope.User)
			{
				label = "Script Templates",
				guiHandler = (searchContext) =>
				{
					if (!prefsLoaded)
					{
						comment = EditorPrefs.GetString("commentKey", string.Empty);
						prefsLoaded = true;
					}

					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine($"#PATH# - Script path");
					stringBuilder.AppendLine($"#CREATIONDATE# - {DateTime.Now}");
					stringBuilder.AppendLine($"#PROJECTNAME# - {PlayerSettings.productName}");
					stringBuilder.AppendLine($"#DEVELOPERS# - {PlayerSettings.companyName}");
					stringBuilder.AppendLine($"#NAMESPACE# - {"Game"}");
					stringBuilder.Append($"#COMMENT# - Comment bellow");
					EditorGUILayout.HelpBox(stringBuilder.ToString(), MessageType.Info, true);

					EditorGUILayout.LabelField("Comment");
					comment = EditorGUILayout.TextArea(comment, GUILayout.MinHeight(32), GUILayout.MaxHeight(128));

					if (GUI.changed)
						EditorPrefs.SetString("commentKey", comment);
				},

				keywords = new HashSet<string>(new[] { "Script Templates", "Templates", "Comment", "Namespace", "ScriptTemplates" })
			};

			return provider;
		}
	}
}
