using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Game.Editor
{
	public class EventReferenceSeeker : EditorWindow
	{
		const string ToolType = "Seeker/";
		const string WindowName = "Event Reference Seeker";

		public string MethodName { get; private set; } = "Name of the method";

		Vector2 scrollPos = Vector2.zero;
		Dictionary<Object, string> references = new Dictionary<Object, string>();
		GUIStyle referenceStyle = new GUIStyle();

		[MenuItem("Tools/" + ToolType + WindowName)]
		public static void ShowWindow()
		{
			GetWindow<EventReferenceSeeker>("Event Reference Seeker");
		}

		void OnGUI()
		{
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos,
											 false,
											 false);

			GUILayout.Label("Unity Event Reference Seeker", EditorStyles.largeLabel);
			MethodName = EditorGUILayout.TextField("Method Name", MethodName);

			if (GUILayout.Button("Search"))
			{
				PopulateRefs(references);
			}

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			{
				if (references.Count > 0)
					ShowMethods();
				else
					GUILayout.Label("No reference founded", EditorStyles.miniLabel);
			}
			EditorGUILayout.EndVertical();

			GUILayout.Label("by Ewan Argouse", EditorStyles.miniLabel);

			EditorGUILayout.EndScrollView();
		}

		private void ShowMethods()
		{
			GUILayout.Label("Game Object - UnityEvent", EditorStyles.boldLabel);

			foreach(var reference in references)
			{
				EditorGUILayout.BeginHorizontal();
				{
					if (referenceStyle == null)
						referenceStyle = new GUIStyle();
					referenceStyle.normal.textColor = Color.cyan;

					GUILayout.Label($"{reference.Key.name} - {reference.Value}", referenceStyle);
					if (GUILayout.Button("Select", GUILayout.MaxWidth(55)))
					{
						Selection.activeObject = reference.Key;
					}
					if (GUILayout.Button("Ping", GUILayout.MaxWidth(55)))
					{
						EditorGUIUtility.PingObject(reference.Key);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		void PopulateRefs(Dictionary<Object, string> refs)
		{
			refs.Clear();
			var allObjects = Resources.FindObjectsOfTypeAll<Object>();
			int length = allObjects.Length;
			BrowseObjects(allObjects, length, refs);
		}

		void BrowseObjects(Object[] objets, int length, Dictionary<Object, string> refs)
		{
			for (int o = 0; o < length; o++)
			{
				var obj = objets[o];

				if (obj is GameObject)
				{
					var go = (GameObject)obj as GameObject;
					var components = go.GetComponents<Component>();
					for (int i = 0; i < components.Length; i++)
					{
						var component = components[i];
						if (component == null) continue;

						FindEventMethodNameReference(component, refs, MethodName);
					}
				}
			}
		}

		void FindEventMethodNameReference<T>
			(T o, Dictionary<Object, string> references, string methodName)
			where T : Object
		{
			var so = new SerializedObject(o);
			var sp = so.GetIterator();

			while (sp.NextVisible(true))
			{
				bool hasReference = HasReferenceMethod(sp, methodName);
				if (hasReference)
					if(!references.ContainsKey(o))
						references.Add(o, sp.displayName);
			}
		}

		bool HasReferenceMethod(SerializedProperty sp, string methodName)
		{
			bool HasMethod(SerializedProperty serializedProperty) => serializedProperty
					.FindPropertyRelative("m_MethodName").stringValue == methodName ? true : false;

			SerializedProperty persistentCalls = sp.FindPropertyRelative("m_PersistentCalls.m_Calls");
			if (persistentCalls == null) return false;
			for (int u = persistentCalls.arraySize - 1; u >= 0; --u)
			{
				if (HasMethod(persistentCalls.GetArrayElementAtIndex(u)))
				{
				return true;
				}
			}
			return false;
		}
	}
}