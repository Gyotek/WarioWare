using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class TestSceneLoading : MonoBehaviour
    {
        [SerializeField] private GameID gameID = default;


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) gameID.GetScene().Load(LoadSceneMode.Single);
        }
    }
}