using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Core
{
    [CustomEditor(typeof(GameID))]
    public class GameIDEditor : OdinEditor
    {
        private GameID source;
        private InspectorProperty[] languageSpecificProperties;
        
        private Language language;
        private int languageIndex => (int)language;
        private int languagesCount;

        private InspectorProperty rivalProperty;

        private SerializedProperty designerProperty;
        private SerializedProperty developerProperty;

        protected override void OnEnable()
        {
            source = (GameID)serializedObject.targetObject;
            
            var tree = Tree;
            languageSpecificProperties = new InspectorProperty[]
            {
                tree.GetPropertyAtPath("titles"),
                tree.GetPropertyAtPath("descriptions"),
                tree.GetPropertyAtPath("actionVerbs")
            };

            languagesCount = Enum.GetNames(typeof(Language)).Length;
            foreach (var languageSpecificProperty in languageSpecificProperties)
            {
                var array = languageSpecificProperty.ValueEntry.WeakSmartValue as string[];
                Array.Resize(ref array, languagesCount);

                for (var i = 0; i < array.Length; i++)
                {
                    if (array[i] == null) array[i] = string.Empty;
                }
                
                languageSpecificProperty.ValueEntry.WeakSmartValue = array;
            }
            
            rivalProperty = tree.GetPropertyAtPath("associatedRivals");
            var rivalValue = (Rivals)rivalProperty.ValueEntry.WeakSmartValue;
            var rivals = rivalValue.Split().ToArray();

            if (rivals.Length != 2) rivalProperty.ValueEntry.WeakSmartValue = Rivals.Melo | Rivals.Theo;

            designerProperty = serializedObject.FindProperty("designer");
            InitializeAuthor(designerProperty);
            developerProperty = serializedObject.FindProperty("developer");
            InitializeAuthor(developerProperty);
        }

        private void InitializeAuthor(SerializedProperty authorProperty)
        {
            if (authorProperty.arraySize == 2) return;
            
            authorProperty.arraySize = 2;
            for (var i = 0; i < 2; i++)
            {
                var nameProperty = designerProperty.GetArrayElementAtIndex(i);
                if (nameProperty.stringValue == null) nameProperty.stringValue = string.Empty;
            }
        }
        
        public override void OnInspectorGUI()
        {
            var tree = Tree;
            InspectorUtilities.BeginDrawPropertyTree(tree, true);
            
            var size = (EditorGUIUtility.fieldWidth / 16) * 9 * 3.3f;
            EditorGUIUtility.labelWidth = size + 75;
            
            language = (Language)EditorGUILayout.EnumPopup(new GUIContent($"Language {languageIndex + 1}/{languagesCount}"), language);
            GUILayout.Space(10);

            DrawLanguageSpecificInfo(new GUIContent("<b>Title</b>"), 0, 20);
            DrawLanguageSpecificInfo(new GUIContent("<b>Action Verb</b>"), 2, 25);
            DrawLanguageSpecificInfo(new GUIContent("<b>Description</b>"), 1, 180, true);
            
            var rect = GUILayoutUtility.GetRect(1, size);
            
            var thumbnailRect = new Rect(rect.x, rect.y, size, rect.height);
            var thumbnailProperty = tree.GetPropertyAtPath("thumbnail");
            thumbnailProperty.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(thumbnailRect,
                GUIContent.none, (Sprite) thumbnailProperty.ValueEntry.WeakSmartValue, typeof(Sprite),
                false, ObjectFieldAlignment.Left);
            
            EditorGUIUtility.labelWidth = 75;
            
            var sceneRect = new Rect(thumbnailRect.xMax + 3, thumbnailRect.y - 7, rect.width - thumbnailRect.width, EditorGUIUtility.singleLineHeight + 9);
            EditorGUI.PropertyField(sceneRect, serializedObject.FindProperty("scene"), new GUIContent("Scene"));

            var inputsRect = new Rect(sceneRect.x, sceneRect.yMax - 5, sceneRect.width - 2, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(inputsRect, serializedObject.FindProperty("inputs"), new GUIContent("Inputs"));

            var rythmRect = new Rect(inputsRect.x, inputsRect.yMax + 11, inputsRect.width, inputsRect.height);
            EditorGUI.PropertyField(rythmRect, serializedObject.FindProperty("rythmConstraints"), new GUIContent("Constraints"));
            
            var rivalsRect = DrawRivals(rythmRect);

            var themeRect = new Rect(rythmRect.x, rivalsRect.yMax + 3, rythmRect.width, rythmRect.height);
            EditorGUI.PropertyField(themeRect, serializedObject.FindProperty("theme"), new GUIContent("Theme"));
            
            EditorGUIUtility.labelWidth = size;    
            EditorGUILayout.Separator();
            
            var designerRect = GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight);
            DrawAuthor(designerRect, designerProperty, new GUIContent("Designer"));
            
            GUILayout.Space(2);
            
            var developerRect =  GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight);
            DrawAuthor(developerRect, developerProperty, new GUIContent("Developer"));
            
            InspectorUtilities.EndDrawPropertyTree(tree);
        }
        
        private void DrawLanguageSpecificInfo(GUIContent label, int infoIndex, int maxCharCount, bool textArea = false)
        {
            var array = languageSpecificProperties[infoIndex].ValueEntry.WeakSmartValue as string[];

            var labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.richText = true;
            
            var charCount = array[languageIndex].Length.ToString();
            if (array[languageIndex].Length < 10 && maxCharCount < 100 || array[languageIndex].Length > 10 && array[languageIndex].Length < 100 && maxCharCount > 100)
            {
                charCount = $"0{array[languageIndex].Length}";
            }
            else if (array[languageIndex].Length < 10 && maxCharCount > 100) charCount = $"00{array[languageIndex].Length}";

            var rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
            
            var labelRect = new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, rect.height);
            EditorGUI.LabelField(labelRect, label, labelStyle);
            
            if (textArea)
            {
                var charCountRect = new Rect(rect.xMax - 50, rect.y, 50, rect.height);
                EditorGUI.LabelField(charCountRect, new GUIContent($"{charCount}/{maxCharCount}"));
                
                var textAreaStyle = new GUIStyle(GUI.skin.textArea);
                textAreaStyle.wordWrap = true;
                
                var textAreaOptions = new GUILayoutOption[]
                {
                    GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 5)
                };
                
                array[languageIndex] = EditorGUILayout.TextArea(array[languageIndex], textAreaStyle, textAreaOptions);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                var halfSpace = 7;
                
                var contentRect = new Rect(labelRect.xMax, rect.y, rect.width - labelRect.width - 42 - halfSpace, rect.height);
                array[languageIndex] = EditorGUI.TextField(contentRect, GUIContent.none, array[languageIndex]);

                var charCountRect = new Rect(contentRect.xMax + halfSpace, rect.y, 42 - halfSpace, rect.height);
                EditorGUI.LabelField(charCountRect, new GUIContent($"{charCount}/{maxCharCount}"));
                
                EditorGUILayout.EndHorizontal();
            }

            if (array[languageIndex].Length > maxCharCount) array[languageIndex] = array[languageIndex].Remove(maxCharCount);

            var copy = new string[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                copy[i] = array[i];
            }
            
            languageSpecificProperties[infoIndex].ValueEntry.WeakSmartValue = copy;
            
            GUILayout.Space(2);
        }

        private Rect DrawRivals(Rect rect)
        {
            var rivals = source.AssociatedRivals.ToArray();
            var rivalNames = Enum.GetNames(typeof(Rivals)).ToList();
            rivalNames.RemoveAt(0);

            var firstRivalRect = new Rect(rect.x, rect.yMax + 3, (rect.width * 0.65f) - 3, rect.height);
            rivalNames.Remove(rivals[1].ToString());

            var indexOne = rivalNames.IndexOf(rivals[0].ToString());
            indexOne = EditorGUI.Popup(firstRivalRect, "Rival 01", indexOne, rivalNames.ToArray());
            var rivalOne = rivalNames[indexOne];

            var secondRivalRect =
                new Rect(firstRivalRect.xMax + 3, firstRivalRect.y, rect.width * 0.35f, firstRivalRect.height);
            rivalNames.Add(rivals[1].ToString());
            rivalNames.Remove(rivals[0].ToString());

            var indexTwo = rivalNames.IndexOf(rivals[1].ToString());
            indexTwo = EditorGUI.Popup(secondRivalRect, indexTwo, rivalNames.ToArray());

            rivalProperty.ValueEntry.WeakSmartValue = Enum.Parse(typeof(Rivals), $"{rivalOne}, {rivalNames[indexTwo]}");
            return firstRivalRect;
        }
        
        private void DrawAuthor(Rect rect, SerializedProperty authorProperty, GUIContent label)
        {
            var firstNameRect = new Rect(rect.x, rect.y, rect.width * 0.7f - 2, rect.height);
            var firstNameProperty = authorProperty.GetArrayElementAtIndex(0);
            EditorGUI.PropertyField(firstNameRect, firstNameProperty, label);
            CorrectAuthorName(firstNameProperty);
            
            var lastNameRect = new Rect(firstNameRect.xMax + 2, firstNameRect.y, rect.width * 0.3f, firstNameRect.height);
            var lastNameProperty = authorProperty.GetArrayElementAtIndex(1);
            EditorGUI.PropertyField(lastNameRect, lastNameProperty, GUIContent.none);
            CorrectAuthorName(lastNameProperty);
        }
        private void CorrectAuthorName(SerializedProperty authorName)
        {
            if (authorName.stringValue.Length <= 0) return;
            
            var firstChar =  char.ToUpper(authorName.stringValue[0]);
            authorName.stringValue = authorName.stringValue.Remove(0, 1).ToLower();
            authorName.stringValue = authorName.stringValue.Insert(0, firstChar.ToString());
        }
    }
}