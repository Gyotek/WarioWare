using UnityEngine;

namespace Game.Core
{
	using static AssetMenuUtils;
    [CreateAssetMenu(menuName = MicroMenu + "GameID")]
    public class GameID : ScriptableObject
    {
        [SerializeField, Tooltip("Title of the WarioWare minigame.")]
        private string gameName = default;

        [SerializeField, TextArea(2,10), Tooltip("Little description about the minigame's context and goal.")]
        private string description = default;

        [SerializeField, Tooltip("Verb of the WarioWare minigame.")]
        private string verb = default;

        [SerializeField, Tooltip("To which BPM the minigame should not be played ?")]
        private SerieConstraints serieConstraints = default;

        [SerializeField, Tooltip("The scene asset of your minigame.")]
        private SceneField scene = default;

        [Space(15)]

        [SerializeField, Tooltip("Minigame's Game Designer. (Surname NAME)")]
        private string designer = default;

        [SerializeField, Tooltip("Minigame's Game Programmer (you <3). (Surname NAME)")]
        private string programmer = default;

        [Space(15)]

        [SerializeField, DisplaySprite, Tooltip("Thumbnail of the minigame. It will be shown in the player's collection.")]
        private Sprite thumbnail = default;

        [SerializeField, DisplaySprite, Tooltip("Sprite of the minigame's input. It will be shown during the verb display.")]
        private Sprite inputSprite = default;

        [HideInInspector]
        public GameData data = default;

        public string GetGameName() => gameName;
        public string GetDescription() => description;
        public SerieConstraints GetConstraints() => serieConstraints;
        public string GetVerb() => verb;
        public SceneField GetScene() => scene;
        public string GetDesigner() => designer;
        public string GetProg() => programmer;
        public Sprite GetThumbnail() => thumbnail;
        public Sprite GetInputSprite() => inputSprite;
        public GameData GetData() => data;

        public bool Check()
        {
            bool check = true;

            if (gameName == "")
            {
                Debug.LogError("The gameName field of the GameID is empty !");
                check = false;
            }
            if (description == "")
            {
                Debug.LogError("The description field of the GameID is empty !");
                check = false;
            }
            if (verb == "")
            {
                Debug.LogError("The verb field of the GameID is empty !");
                check = false;
            }
            if (scene == null)
            {
                Debug.LogError("The scene field of the GameID is empty !");
                check = false;
            }
            if (designer == "")
            {
                Debug.LogError("The designer field of the GameID is empty !");
                check = false;
            }
            if (programmer == "")
            {
                Debug.LogError("The programmer field of the GameID is empty !");
                check = false;
            }
            /*if (!thumbnail)
            {
                Debug.LogError("The thumbnail field of the GameID is empty !");
                check = false;
            }
            if (!inputSprite)
            {
                Debug.LogError("The inputSprite field of the GameID is empty !");
                check = false;
            }*/

            return check;
        }
    }
}

