using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

namespace Game.Core
{
    public class GameSheet : MonoBehaviour
    {
        [SerializeField] private SuperTextMesh gameName, gameDescription;
        
        [Space, SerializeField] private RawImage thumbnail;
        [SerializeField] private Texture defaultThumbnail;
        
        [Space, SerializeField] private Image[] stars;
        [SerializeField] private Color enabledColor, disabledColor;

        [Space, SerializeField] private Slider bpmSlider;
        [SerializeField] private Slider difficultySlider;

        [Space, SerializeField] private Button playOnceButton;
        [SerializeField] private Button playOnRepeatButton;

        [Space, SerializeField] private GameSequenceHandler gameSequenceHandler;
        [SerializeField] private FreeSequenceLauncher freeSequenceLauncher;

        [ShowInInspector, ReadOnly] private GameID currentGame;
        private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup => canvasGroup;


        void Start() => canvasGroup = GetComponent<CanvasGroup>();
            
        public void RegisterGameTape(GameTape tape) => tape.OnClick.AddListener(() => Refresh(tape.Game));

        public void Refresh(GameID game)
        {
            currentGame = game;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            var gameInfo = game.GetLanguageSpecificInfo(Language.English);
            
            gameName.text = gameInfo[0].Remove(0,4);
            thumbnail.texture = game.Thumbnail.texture;
            thumbnail.SetNativeSize();
            gameDescription.text = gameInfo[1];

            var difficulties = Enum.GetValues(typeof(Difficulty)) as Difficulty[];
            for (var i = 0; i < difficulties.Length; i++)
            {
                stars[i].color = game.GetWinCount(difficulties[i]) > 0 ? enabledColor : disabledColor;
            }
            var playCount = difficulties.Sum(t => game.GetPlayCount(t) > 0 ? 1 : 0);
            
            bpmSlider.maxValue = Mathf.Clamp(playCount, 1 , 3);
            difficultySlider.maxValue = Mathf.Clamp(playCount, 1, 3);
        }

        public void Reset()
        {
            canvasGroup.alpha = 0.75f;
            canvasGroup.blocksRaycasts = false;
            
            gameName.text = "GameName";
            thumbnail.texture = defaultThumbnail;
            thumbnail.SetNativeSize();
            gameDescription.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla non justo arcu.";

            foreach (var star in stars) star.color = disabledColor;

            bpmSlider.maxValue = 1;
            difficultySlider.maxValue = 1;
        }

        public void LaunchCurrentGameOnce()
        {
            freeSequenceLauncher.SetupSingleSequence(currentGame);
            gameSequenceHandler.StartNewSequence(freeSequenceLauncher as IGameSequence);
        }
        public void LaunchCurrentGameOnRepeat()
        {
            freeSequenceLauncher.SetupRepeatableSequence(currentGame);
            gameSequenceHandler.StartNewSequence(freeSequenceLauncher as IGameSequence);
        }
    }
}