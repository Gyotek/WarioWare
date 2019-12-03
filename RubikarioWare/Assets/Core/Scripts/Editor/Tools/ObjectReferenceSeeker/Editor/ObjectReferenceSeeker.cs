using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;
using UnityEditor.Animations;

namespace ObjectReferenceSeeker.Editor
{
	public class ObjectReferenceSeeker : EditorWindow
	{
		const string ToolType = "Seeker/";
		const string WindowName = "Object Reference Seeker";

		Texture2D IconGo { get; set; }
		Texture2D IconAsset { get; set; }
		Texture2D IconAnimatorController { get; set; }

		Dictionary<Object, Type> references = new Dictionary<Object, Type>();
		Vector2 scrollPos = Vector2.zero;
		Object @object = default;

		[MenuItem("Tools/" + ToolType + WindowName)]
		private static void Window()
		{
			var window = (ObjectReferenceSeeker)
				GetWindow(typeof(ObjectReferenceSeeker), false, WindowName);
			window.Show();
		}

		private void OnEnable()
		{
			IconGo = EditorGUIUtility.IconContent("GameObject Icon").image as Texture2D;
			IconAsset = EditorGUIUtility.IconContent("Prefab Icon").image as Texture2D;
			IconAnimatorController = EditorGUIUtility.IconContent("AnimatorController Icon").image as Texture2D;
		}

		void OnGUI()
		{
			GUILayout.Label(WindowName, EditorStyles.largeLabel);

			@object = EditorGUILayout.ObjectField(@object, typeof(Object), true);

			EditorGUILayout.Space();

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos,
											 false,
											 false);
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("References", EditorStyles.largeLabel);

			if (GUILayout.Button("Search") && @object != null)
			{
				PopulateRefs();
			}

			EditorGUILayout.Space();
			if (references.Count > 0)
				ShowReferences();
			else
				GUILayout.Label("No reference founded", EditorStyles.miniLabel);


			EditorGUILayout.EndVertical();

			GUILayout.Label("by Ewan Argouse", EditorStyles.miniLabel);

			EditorGUILayout.EndScrollView();
		}

		GUIContent GetIcon(Object obj)
		{
			switch (obj)
			{
				case GameObject go:
					return new GUIContent(IconGo, nameof(GameObject));
				case AnimatorController an:
					return new GUIContent(IconAnimatorController, "AnimatorController");
				case ScriptableObject so:
					return new GUIContent(
						AssetDatabase.GetCachedIcon(
							AssetDatabase.GetAssetPath(obj)), "ScriptableObject");
				case null:
					return new GUIContent(IconAsset, "Asset");
			}
			return new GUIContent(IconAsset, "Asset");
		}

		void ShowReferences()
		{
			Color gbc = GUI.backgroundColor;
			foreach (var @ref in references)
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxHeight(44));
				EditorGUILayout.BeginHorizontal();
				GUIContent icon = GetIcon(@ref.Key);
				EditorGUILayout.LabelField(icon, GUILayout.Width(30), GUILayout.Height(25), GUILayout.ExpandHeight(true));
				ShowReferenceLabel(@ref);
				GUI.backgroundColor = Color.cyan;
				if (GUILayout.Button("Select", GUILayout.MaxWidth(55), GUILayout.ExpandHeight(true)))
				{
					Selection.activeObject = @ref.Key;
				}
				GUI.backgroundColor = Color.green;
				if (GUILayout.Button("Ping", GUILayout.MaxWidth(55), GUILayout.ExpandHeight(true)))
				{
					EditorGUIUtility.PingObject(@ref.Key);
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
			GUI.backgroundColor = gbc;
		}

		void ShowReferenceLabel(KeyValuePair<Object, Type> @ref)
		{
			GUIStyle goName = new GUIStyle(EditorStyles.boldLabel);
			goName.alignment = TextAnchor.MiddleLeft;
			EditorGUILayout.BeginVertical();
			if (@ref.Key != null && @ref.Value != null)
			{
				EditorGUILayout.LabelField($"{@ref.Key.name}", goName,
					GUILayout.MinWidth(100), GUILayout.ExpandHeight(true));
				EditorGUILayout.LabelField($"∟ {@ref.Value.Name}", EditorStyles.boldLabel,
					GUILayout.MinWidth(100), GUILayout.ExpandHeight(true));
			}
			else
			{
				EditorGUILayout.LabelField("Destroyed", goName,
					GUILayout.MinWidth(100), GUILayout.ExpandHeight(true));
			}
			EditorGUILayout.EndVertical();
		}

		void PopulateRefs()
		{
			references.Clear();
			List<Object> allSceneObjects = new List<Object>();
			allSceneObjects.AddRange(FindObjectsOfType<GameObject>());
			allSceneObjects.AddRange(FindObjectsOfType<Animator>());
			int length = allSceneObjects.Count;
			allSceneObjects.AddRange(FindObjectsOfType<ScriptableObject>());
			BrowseObjects(allSceneObjects.ToArray(), length, references);
			List<Object> allObjects = new List<Object>();
			allObjects.AddRange(Resources.FindObjectsOfTypeAll<GameObject>());
			allObjects.AddRange(Resources.FindObjectsOfTypeAll<Animator>());
			length = allObjects.Count;
			BrowseObjects(allObjects.ToArray(), length, references);
			ScriptableObject[] scriptableObj = GetAllInstances<ScriptableObject>();
			length = scriptableObj.Length;
			BrowseObjects(scriptableObj, length, references);
		}

		void BrowseObjects(Object[] objets, int length, Dictionary<Object, Type> refs)
		{
			for (int o = length - 1; o >= 0; --o)
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

						FindReferenceInField(component, component, refs);
					}
				}
				if (obj is Animator)
				{
					var animator = (Animator)obj as Animator;
					AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
					if (controller == null) continue;

					foreach (AnimatorControllerLayer layer in controller.layers)
					{
						foreach (ChildAnimatorState childAnimState in layer.stateMachine.states)
						{
							foreach (StateMachineBehaviour smb in childAnimState.state.behaviours)
							{
								FindReferenceInField(controller, smb, refs);
							}
						}
					}
				}

				if (obj is ScriptableObject)
				{
					var sObj = obj as ScriptableObject;

					FindReferenceInField(sObj, sObj, refs);
				}
			}
		}

		void FindReferenceInField<T, U>
			(T o, U type, Dictionary<Object, Type> references)
			where T : Object
		{
			var so = new SerializedObject(o);
			var sp = so.GetIterator();

			while (sp.NextVisible(true))
			{
				if (sp.propertyType == SerializedPropertyType.ObjectReference)
				{
					if (sp.objectReferenceValue == @object)
					{
						if (!references.ContainsKey(o))
							references.Add(o, type.GetType());
					}
				}

				bool isUnityEvent = HasReferenceInUnityEvent(sp);
				if (isUnityEvent)
					if (!references.ContainsKey(o))
						references.Add(o, type.GetType());
			}
		}

		public static T[] GetAllInstances<T>() where T : ScriptableObject
		{
			string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
			int length = guids.Length;
			T[] a = new T[length];
			for (int i = length - 1; i >= 0; i--)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
			}
			return a;
		}

		bool HasReferenceInUnityEvent(SerializedProperty sp)
		{
			bool IsTarget(SerializedProperty serializedProperty) => serializedProperty
					.FindPropertyRelative("m_Target").objectReferenceValue == @object ? true : false;

			bool IsArgument(SerializedProperty serializedProperty) => serializedProperty
					.FindPropertyRelative("m_Arguments")
					.FindPropertyRelative("m_ObjectArgument").objectReferenceValue == @object ? true : false;

			SerializedProperty persistentCalls = sp.FindPropertyRelative("m_PersistentCalls.m_Calls");
			if (persistentCalls == null) return false;
			for (int u = persistentCalls.arraySize - 1; u >= 0; --u)
			{
				if (IsTarget(persistentCalls.GetArrayElementAtIndex(u)) || IsArgument(persistentCalls.GetArrayElementAtIndex(u)))
				{
					return true;
				}
			}
			return false;
		}
	}
}