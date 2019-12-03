using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [System.Serializable]
    public class SceneField
    {
        [SerializeField] private Object m_SceneAsset;
        [SerializeField] private string m_SceneName = "";
        public string SceneName => m_SceneName;

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
            return SceneManager.LoadSceneAsync(SceneName,_parameters);
        }

    }
}