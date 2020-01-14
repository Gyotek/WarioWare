using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Game.Core.Serialization;

[CustomEditor(typeof(SaveSystem))]
public class ShowSaveFolder : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button(new GUIContent("Folder")))
        {
            ShowPersistentData();
        }
    }

    static void ShowPersistentData()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }
}