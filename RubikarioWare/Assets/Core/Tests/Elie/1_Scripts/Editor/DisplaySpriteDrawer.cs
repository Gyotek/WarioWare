using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DisplaySpriteAttribute))]
public class DisplaySpriteDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DisplaySpriteAttribute display = attribute as DisplaySpriteAttribute;

        property.objectReferenceValue = EditorGUILayout.ObjectField(property.displayName, property.objectReferenceValue, typeof(Sprite), true, GUILayout.ExpandWidth(true));
    }
}
