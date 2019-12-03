using System;
using UnityEditor;
using UnityEngine;

namespace ScriptTemplates.Editor
{
	public class KeywordReplace : UnityEditor.AssetModificationProcessor
	{
		public static void OnWillCreateAsset(string path)
		{
			path = path.Replace(".meta", "");
			int index = path.LastIndexOf(".");
			if (index < 0)
				return;
			string file = path.Substring(index);
			if (file != ".cs") return;
			index = Application.dataPath.LastIndexOf("Assets");
			path = Application.dataPath.Substring(0, index) + path;
			file = System.IO.File.ReadAllText(path);

			file = file.Replace("#COMMENT#", ScriptTemplatesPreferences.comment);
			file = file.Replace("#CREATIONDATE#", DateTime.Now.ToString());
			file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
			file = file.Replace("#DEVELOPERS#", PlayerSettings.companyName);
			file = file.Replace("#NAMESPACE#", "Game");
			file = file.Replace("#PATH#", path);

			System.IO.File.WriteAllText(path, file);
			AssetDatabase.Refresh();
		}
	}
}