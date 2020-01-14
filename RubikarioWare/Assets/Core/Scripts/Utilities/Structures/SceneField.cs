using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    [System.Serializable]
    public class SceneField : ISerializationCallbackReceiver {
 
        public SceneField(UnityEditor.SceneAsset sceneAsset)
        {
            this.sceneAsset = sceneAsset;
        }
        
        #if UNITY_EDITOR
        public UnityEditor.SceneAsset sceneAsset;
        #endif
 
        #pragma warning disable 414
        [SerializeField, HideInInspector]
        private string sceneName = "";
        public string SceneName => sceneName;
        #pragma warning restore 414
 
        // Makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneField sceneField) {
        #if UNITY_EDITOR
            return System.IO.Path.GetFileNameWithoutExtension(UnityEditor.AssetDatabase.GetAssetPath(sceneField.sceneAsset));
        #else
        return sceneField.sceneName;
        #endif
        }
 
        public void OnBeforeSerialize() 
        {
        #if UNITY_EDITOR
            sceneName = this;
        #endif
        }
        public void OnAfterDeserialize() {}
        
        public void Load(LoadSceneMode _mode)
        {
            SceneManager.LoadScene(SceneName, _mode);
        }

        public Scene Load(LoadSceneParameters _parameters)
        {
            return SceneManager.LoadScene(SceneName, _parameters);
        }

        public void UnLoad()
        {
            SceneManager.UnloadSceneAsync(SceneName);
        }

        public AsyncOperation LoadAsync(LoadSceneMode _mode)
        {
            return SceneManager.LoadSceneAsync(SceneName, _mode);
        }

        public AsyncOperation LoadAsync(LoadSceneParameters _parameters)
        {
            return SceneManager.LoadSceneAsync(SceneName, _parameters);
        }
    }
}