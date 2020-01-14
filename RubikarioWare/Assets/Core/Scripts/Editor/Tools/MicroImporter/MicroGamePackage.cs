using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;

namespace Game.Core.Editor
{
    public class MicroGamePackage
    {
        public string path { get; private set; }
        public string assetName { get; private set; }
        public MicroGame game { get; private set; }

        private string assetsFolder;
        private string scene;
        private string id;
        public int index { get; private set; }
        public string gameName => game.GetAssetName();
        public string AssetsFolder => assetsFolder;
        public string Scene => scene;
        public string ID => id;

        public MicroGamePackage(string _path, string _assetFolder)
        {
            path = _path;
            assetName = GetName(path, "");
            game = ParsePackageName(GetName(path, ".unitypackage"));
            assetsFolder = _assetFolder;
            FindWithExtension(_assetFolder + "/Export", "*.unity", out scene);
            FindWithExtension(_assetFolder + "/Export", "*.asset", out id);
            index = (int)game;
        }

        public string Organize(bool _forceOverwrite)
        {
            if(Directory.Exists(Application.dataPath+"/Micro/Assets/"+game.GetAssetName()))
            {
                if (_forceOverwrite)
                {
                    Debug.LogWarning("The package already exists. It will be overwriten.");

                    DeleteFolder(Application.dataPath + "/Micro/Assets/" + game.GetAssetName());

                    SetGUID(scene, AssetDatabase.AssetPathToGUID("Assets/Micro/Scenes/" + game.GetAssetName() + "Scene.unity"));
                    DeleteFolder(Application.dataPath + "/Micro/Scenes/" + game.GetAssetName() + "Scene.unity");

                    SetGUID(id, AssetDatabase.AssetPathToGUID("Assets/Micro/GameIDs/" + game.GetAssetName() + "ID.asset"));
                    DeleteFolder(Application.dataPath + "/Micro/GameIDs/" + game.GetAssetName() + "ID.asset");
                }
                else
                {
                    Debug.LogWarning("Package already exists.");

                    return default;
                }  
            }

            RenameAsset(ref scene, "Scene");
            RenameAsset(ref id, "ID");

            GameIDFix(id);
            LinkSceneAndGameID(scene, id);

            MoveAsset(ref scene, "Micro/Scenes/");
            MoveAsset(ref id, "Micro/GameIDs/");

            DeleteFolder(assetsFolder + "/Export");

            MoveAsset(ref assetsFolder, "Micro/Assets/");
            RenameAsset(ref assetsFolder);

            return scene;
        }

        private bool FindWithExtension(string _path, string _extension, out string _result)
        {
            string[] files = Directory.GetFiles(_path, _extension);

            if(files!=null && files.Length>0)
            {
                _result = files[0];
                Debug.Log("File: " + _result);
                return true;
            }
            else
            {
                _result = default;
                Debug.Log("File: ERROR");
                return false;
            }

        }

        private void SetGUID(string _path, string _newGUID)
        {
            string[] metaFile = File.ReadAllLines(_path + ".meta");

            for (int i = 0; i < metaFile.Length; i++)
            {
                if(metaFile[i].Contains("guid:"))
                {
                    metaFile[i] = "guid: " + _newGUID;
                    break;
                }
            }

            File.WriteAllLines(_path + ".meta", metaFile);
        }

        private MicroGame ParsePackageName(string _packageName)
        {
            return (MicroGame)int.Parse(_packageName);
        }

        public void RenameAsset(ref string _assetPath, string _suffixe = "")
        {
            Debug.Log(_assetPath+"\n"+RemoveExtension(GetName(_assetPath, "")) + " --> " + game.GetAssetName() + _suffixe);
            AssetDatabase.RenameAsset(GetProjectPath(_assetPath), game.GetAssetName() + _suffixe);
            _assetPath = _assetPath.Replace(RemoveExtension(GetName(_assetPath, "")), game.GetAssetName() + _suffixe);
            Debug.Log(_assetPath);

            AssetDatabase.Refresh();
        }

        private void GameIDFix(string _path)
        {
            Debug.Log("id: " + _path);

            string assetText = File.ReadAllText(_path);

            assetText = assetText.Replace("guid: b2fcdcfd9d60bfe4c89b024ff6caf274", "guid: 8ad708abf202d5145966ede5b30b588b"); // guid de la classe corrigé
            assetText = assetText.Replace("gameName", "name");
            assetText = assetText.Replace("verb", "actionVerb");
            assetText = assetText.Replace("serieConstraints", "rythmConstraints");
            assetText = assetText.Replace("designer", "designerName");
            assetText = assetText.Replace("programmer", "developerName");
            assetText = assetText.Replace("thumbnail: {fileID: 0}", "thumbnail: {fileID: 21300000, guid: 126cd1338861fdd43a35344843db42ff, type: 3}");
            assetText = assetText.Replace("inputSprite", "inputIcon");
            assetText = assetText.Replace("data: {}", "tags: 15 \n  playCounts: 010000000100000001000000\n  winCounts: 010000000100000001000000");

            File.WriteAllText(_path, assetText);

            AssetDatabase.Refresh();
        }

        public void MoveAsset(ref string _assetPath, string _folderPath)
        {
            Debug.Log("File to move: " + _assetPath);
            Debug.Log(GetProjectPath(_assetPath) + " --> " + "Assets/" + _folderPath + GetName(_assetPath, ""));
            AssetDatabase.MoveAsset(GetProjectPath(_assetPath), "Assets/"+ _folderPath + GetName(_assetPath, ""));
            _assetPath = Application.dataPath + "/" + _folderPath + GetName(_assetPath, "");
            Debug.Log(_assetPath);

            AssetDatabase.Refresh();
        }

        public string GetProjectPath(string _path)
        {
            return _path.Replace(Application.dataPath, "Assets");
        }

        private void DeleteFolder(string _path)
        {
            UnityEngine.Windows.Directory.Delete(_path);
            File.Delete(_path + ".meta");

            AssetDatabase.Refresh();
        }

        private string GetName(string _filePath, string _extentionToRemove)
        {
            List<char> name = new List<char>();

            for (int i = _filePath.Length - _extentionToRemove.Length - 1; i > 0; i--)
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

        private string RemoveExtension(string _filePath)
        {
            List<char> name = new List<char>();
            bool gotExtension = false;

            for (int i = _filePath.Length - 1; i > 0; i--)
            {
                if (_filePath[i] == '.')
                {
                    gotExtension = true;
                    break;
                }

                name.Add(_filePath[i]);
            }

            string extension = "";

            for (int i = name.Count - 1; i >= 0; i--)
            {
                extension += name[i];
            }

            return gotExtension ? _filePath.Replace("."+extension, "") : _filePath;
        }

        private void LinkSceneAndGameID(string _scenePath, string _gameIDPath)
        {
            string sceneGUID = AssetDatabase.AssetPathToGUID(GetProjectPath(_scenePath));
            string[] gameID = File.ReadAllLines(_gameIDPath);

            for (int i = 0; i < gameID.Length; i++)
            {
                if(gameID[i].Contains("    m_SceneAsset:"))
                {
                    gameID[i] = "    m_SceneAsset: { fileID: 102900000, guid:" + sceneGUID + ", type: 3}";
                    break;
                }
            }

            File.WriteAllLines(_gameIDPath, gameID);

            AssetDatabase.Refresh();
        }
    }
}