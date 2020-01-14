using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Game.Core;

using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class IdSceneLinker : EditorWindow
{
    [MenuItem("Tools/WarioWare/GameId Scene Linker")]
    public static void ShowWindow()
    {
        GetWindow<IdSceneLinker>("GameId Scene Linker");
    }
    
    private const string scenesPath = "Assets/Micro/Scenes";
    private const string idsPath = "Assets/Micro/GameIDs";

    private Sprite genericThumbnail;

    private string[] firstNames = new string[]
    {
        "Jean",
        "Louis",
        "Lucas",
        "Philippe",
        "Gérard",
        "Bertrand",
        "Pauline",
        "Camille",
        "Pierre",
        "Luc"
    };

    private string[] lastNames = new string[]
    {
        "Chevalier",
        "Dupont",
        "Delaterre",
        "Dulatre",
        "Mezencore",
        "Pommepardi",
        "Bizon",
        "Parglaz",
        "Jarowski",
        "Zlav"
    };
    
    void OnGUI()
    {
        if (GUILayout.Button("Link"))
        {
            var field = typeof(GameID).GetField("scene", BindingFlags.Instance | BindingFlags.NonPublic);
            
            var scenes = Directory.GetFiles(scenesPath, "*.unity")
                .ToList()
                .ConvertAll(filePath => AssetDatabase.LoadAssetAtPath<SceneAsset>(filePath));
            
            var gameIds = Directory.GetFiles(idsPath, "*.asset")
                .ToList()
                .ConvertAll(filePath => AssetDatabase.LoadAssetAtPath<GameID>(filePath));

            for (var i = 0; i < scenes.Count; i++) field.SetValue(gameIds[i], new SceneField(scenes[i]));
        }

        EditorGUILayout.Space();
        genericThumbnail = (Sprite)EditorGUILayout.ObjectField("Generic Thumbnail", genericThumbnail, typeof(Sprite), false);
        
        if (GUILayout.Button("Reset"))
        {
            var scenes = Directory.GetFiles(scenesPath, "*.unity")
                .ToList()
                .ConvertAll(filePath => AssetDatabase.LoadAssetAtPath<SceneAsset>(filePath));

            foreach (var scene in scenes)
            {
                var splittedSceneName = scene.name.Split('_');
                
                var gameNumber = splittedSceneName[0];
                var gameTitle = splittedSceneName[1];
                gameTitle = gameTitle.Remove(gameTitle.Length - 5);
                
                var assetName = $"{gameNumber}_{gameTitle}ID";
                var gameID = ScriptableObject.CreateInstance<GameID>();
                gameID.name = assetName;

                var titles = new string[] {$"[FR]{gameTitle}", $"[EN]{gameTitle}"};
                var descriptions = new string[] {"404 Description non trouvé", "404 Description not found"};
                var actionVerbs = new string[] {"Joue!", "Play!"};

                var gameScene = new SceneField(scene);

                var designer = new string[] {firstNames[Random.Range(0, firstNames.Length)], lastNames[Random.Range(0, lastNames.Length)]};
                var developer = new string[] {firstNames[Random.Range(0, firstNames.Length)], lastNames[Random.Range(0, lastNames.Length)]};
                
                var inputs = Utility.GetRandomFlag<Inputs>(Random.Range(1, 3));
                var rythmConstraints = Utility.GetRandomFlag<Rythm>(Random.Range(1, 3));
                var rivals = Utility.GetRandomFlag<Rivals>(2);
                var theme = Utility.GetRandomEnumValue<Theme>();

                ((Iinitializable) gameID).Intialize(titles,
                    descriptions,
                    actionVerbs,
                    gameScene,
                    genericThumbnail,
                    designer,
                    developer,
                    inputs,
                    rythmConstraints,
                    rivals,
                    theme);
                AssetDatabase.CreateAsset(gameID, $"{idsPath}/{assetName}.asset");
            }
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
        }
    }
}
