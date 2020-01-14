using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

namespace Game.Core
{
    [CustomEditor(typeof(AssetGroup))]
    public class AssetGroupEditor : Sirenix.OdinInspector.Editor.OdinEditor
    {
        private AssetGroup source;
        
        private void OnEnable() => source = (AssetGroup)serializedObject.targetObject;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);
            
            if (GUILayout.Button("Set group type"))
            {
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                    .Where(t => typeof(ScriptableObject).IsAssignableFrom(t));
            
                var selectedTypes = new List<Type>();
                var selector = new TypeSelector(types, false);
                selector.SetSelection(selectedTypes);
                selector.SelectionConfirmed += selection => source.SetGroupType(selection.FirstOrDefault());
                selector.ShowInPopup();
            }

            if (GUILayout.Button("Verify group")) source.VerifyGroup();
        }
    }
}