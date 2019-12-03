using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


namespace Game.Core
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] UnityEvent OnLoad = default;
        [SerializeField] Animator anim = default;
        [SerializeField] QPM_Adversaires adversaire = default;
        [SerializeField] List<SceneField> gamesScenes = default;
        bool checkProgressLoad;

        AsyncOperation asyncOperation = null;

        SceneField currentScene;

        private void Update()
        {
            Debug.Log(adversaire);
            CheckProgressLoad();
        }

        void CheckProgressLoad()
        {
            if (checkProgressLoad)
            {

                while (asyncOperation.progress < .88f)
                {
                    Debug.Log(asyncOperation.progress);
                }

                if (asyncOperation.progress >= .89f)
                {
                    anim.SetTrigger("EndLoad");
                    asyncOperation.allowSceneActivation = true;
                    checkProgressLoad = false;
                }
            }
        }

        public void LoadLevel()
        {
           

            int random = Random.Range(0, gamesScenes.Count);

            if (currentScene != null &&  currentScene.SceneName != "")
            {
                currentScene.UnLoad();
            }

            currentScene = gamesScenes[random];

            asyncOperation = gamesScenes[random].LoadAsync(LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;

            gamesScenes.Remove(gamesScenes[random]);

            checkProgressLoad = true;
        }

        public void AddAdversaire(QPM_Adversaires adversaire)
        {

            for (int i = 0; i < adversaire.gamesID.Count; i++)
            {
                gamesScenes.Add(adversaire.gamesID[i].GetScene());
            }

        }

        public void InvokeOnLoad()
        {
            OnLoad.Invoke();
        }
        
        
    
    }
}