using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Game.Core.Editor
{
    public class MicroGameImporter : EditorWindow
    {
        private bool imported = false;
        private string[] directories;
        private string refPath;
        private string dir;
        private string gamesIdsDirectory;
        private string gamesScenesDirectory;
        private string gamesAssetsDirectory;
        private bool forceOverwrite;
        private string packagePath = "";

        [MenuItem("Tools/WarioWare/MicroGame Importer")]
        public static void ShowWindow()
        {
            GetWindow<MicroGameImporter>("MiniGame Importer");
        }

        private void OnGUI()
        {
            refPath = Application.dataPath + "/Micro";

            gamesAssetsDirectory = "Micro/Assets"; //Chemin d'accès au dossier où les dossiers d'assets des microjeux sont stockés.
            gamesScenesDirectory = "Micro/Scenes"; // Chemin d'accès au dossier où les scènes des microjeux sont stockées.
            gamesIdsDirectory = "Micro/GameIds"; // Chemin d'accès au dossier où les GameIDs sont stockés.
            forceOverwrite = EditorGUILayout.Toggle("Force overwrite ?", forceOverwrite); // Force la suppression des fichiers d'un import précédents du Package.

            if (GUILayout.Button("Import Package"))
            {
                imported = true; // Indique que l'on est en train d'importer un packageunity.

                directories = Directory.GetDirectories(refPath); //Enregistre avant l'importation la structure du dossier Micro.
                packagePath = EditorUtility.OpenFilePanel("Get UnityPackage", "", "unitypackage"); // Ouvre l'explorateur windows pour récupérer le Package;

                AssetDatabase.ImportPackage(packagePath, false); // Importe Package
            }

            if (imported && directories.Length != Directory.GetDirectories(refPath).Length)
            {
                Debug.Log("path: " + packagePath);
                OnImportationDone(packagePath);

                UpdateImportationStatus();
            }
        }

        /// <summary>
        /// Renvoie le nom du nouveau dossier ajouté par le Package.
        /// </summary>
        /// <returns></returns>
        private string GetNewDirectoryName()
        {
            string[] currentDirectory = Directory.GetDirectories(refPath);

            for (int i = 0; i < currentDirectory.Length; i++)
            {
                if (directories.Contains(currentDirectory[i])) continue;
                else
                {
                    Debug.Log("New Directory Name: " + currentDirectory[i]);
                    return GetName(currentDirectory[i], false);
                }
            }

            return "";
        }

        /// <summary>
        /// Renvoie le nom du fichier ou du dossier au bout du chemin.
        /// </summary>
        /// <param name="_filePath">Chemin d'accès</param>
        /// <param name="_removeExtension">Booléen pour retirer l'extension ".unitypackage"</param>
        /// <returns></returns>
        private string GetName(string _filePath, bool _removeExtension)
        {
            List<char> name = new List<char>();

            for (int i = _removeExtension ? _filePath.Length - 14 : _filePath.Length - 1; i > 0; i--)
            {
                if (_filePath[i] == '/' || _filePath[i] == '\\') break;

                name.Add(_filePath[i]);
            }

            string result = "";

            for (int i = name.Count - 1; i >= 0; i--)
            {
                result += name[i];
            }

            return result;
        }

        /// <summary>
        /// Renvoie le chemin relativement au dossier Unity.
        /// </summary>
        /// <param name="_fullPath">Chemin d'accès complet.</param>
        /// <returns></returns>
        private string GetRelativePath(string _fullPath)
        {
            string result = _fullPath.Replace('\\', '/');

            return result.Replace(Application.dataPath, "Assets");
        }

        private void OnImportationDone(string _path)
        {
            MicroGamePackage package = new MicroGamePackage(_path, Application.dataPath + "/" + "Micro" + "/" + GetNewDirectoryName());
            string scenePath = package.Organize(forceOverwrite);
            AssetDatabase.Refresh();
            ForceRecompile();

            UpdateScene(scenePath);
        }

        /// <summary>
        /// Fonction appelée au moment de la fin de l'importation, et de la mise à jour des dossiers.
        /// </summary>
        private void OnImportationDone()
        {
            dir = GetNewDirectoryName();

            MoveMicroGame();
            GetSceneAndID();
            DeleteExportFolder();
        }

        private void DeleteExportFolder()
        {
            UnityEngine.Windows.Directory.Delete(Application.dataPath + "/" + gamesAssetsDirectory + "/" + dir + "/Export");
            File.Delete(Application.dataPath + "/" + gamesAssetsDirectory + "/" + dir + "/Export.meta");

            AssetDatabase.Refresh();
        }

        private void MoveMicroGame()
        {
            AssetDatabase.MoveAsset("Assets/Micro/" + dir, "Assets/" + gamesAssetsDirectory + "/" + dir);
        }

        private void GetSceneAndID()
        {
            string[] files = Directory.GetFiles(Application.dataPath + "/" + gamesAssetsDirectory + "/" + dir + "/Export");
            AssetDatabase.MoveAsset(GetRelativePath(files[0]), "Assets/" + gamesIdsDirectory + "/" + GetName(files[0], false));
            AssetDatabase.MoveAsset(GetRelativePath(files[2]), "Assets/" + gamesScenesDirectory + "/" + GetName(files[2], false));
        }

        private void UpdateImportationStatus()
        {
            directories = Directory.GetDirectories(Application.dataPath);
            imported = false;
        }

        private void UpdateScene(string _scenePath)
        {
            EditorSceneManager.OpenScene(_scenePath);

            DestroyImmediate(GameObject.Find("Debug"));
            GameObject.Find("Intermediary")?.GetComponent<MicroActivator>().SetRootActives();
            DestroyImmediate(GameObject.Find("Intermediary"));
            Camera cam = FindObjectOfType<Camera>();
            if (cam)
            {
                if (cam.orthographicSize != 5) Debug.LogWarning("Orthographic size doesn't match.");
                cam.depth = 100;
            }
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
        }

        private void ForceRecompile()
        {
            MonoScript cMonoScript = MonoImporter.GetAllRuntimeMonoScripts()[0];
            MonoImporter.SetExecutionOrder(cMonoScript, MonoImporter.GetExecutionOrder(cMonoScript));
        }
    }
}