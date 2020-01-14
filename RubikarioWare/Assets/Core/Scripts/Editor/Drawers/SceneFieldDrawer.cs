using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;
using UnityEditor;

namespace Game.Core
{
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            SerializedProperty sceneAssetProp = _property.FindPropertyRelative("sceneAsset");
 
            EditorGUI.BeginProperty(_position, _label, sceneAssetProp);
            EditorGUI.PropertyField(_position, sceneAssetProp, _label);
            EditorGUI.EndProperty();
        }
    }
}