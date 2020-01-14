using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class GameTape : MonoBehaviour, ISearchable<TapeSearchArguments>
    {
        [SerializeField] private GameID game;
        public GameID Game => game;
        
        [Space(15)]
        
        [SerializeField] private SuperTextMesh textMesh;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        
        [Space(15)] 
        
        [SerializeField] private Button button;
        [SerializeField] private RawImage rivalOneImage, rivalTwoImage, themeImage;
        [SerializeField] private Image[] inputImages;
        
        public Button.ButtonClickedEvent OnClick => button.onClick;

        private static Dictionary<Enum, string> enumHexes = new Dictionary<Enum, string>()
        {
            {Rivals.Arsene, "#D162E9"},
            {Rivals.Hortensia, "#E960C9"},
            {Rivals.Dode, "#E96091"},
            {Rivals.Emmanalyst, "#E96069"},
            {Rivals.Vlad, "#E97560"},
            {Rivals.Fabala, "#E99860"},
            {Rivals.Jesse, "#E9C960"},
            {Rivals.Theo, "#E9E860"},
            {Rivals.Melo, "#C2E960"},
            
            {Inputs.Mouse, "#6160E9"},
            {Inputs.Cursor, "#608CE9"},
            {Inputs.Arrows, "#60B1E9"},
            {Inputs.Keyboard, "#60D1E9"},
            
            {Theme.Fiction, "#8AE960"},
            {Theme.NewTechnologies, "#65E960"},
            {Theme.DailyLife, "#60E988"},
            {Theme.Diverse, "#60E9B2"},
        };
        
        public void Initialize(GameID game)
        {
            this.game = game;

            var rivals = game.AssociatedRivals.ToArray();
            UnityEngine.ColorUtility.TryParseHtmlString(enumHexes[rivals[0]], out var rivalOneColor);
            rivalOneImage.color = rivalOneColor;
            
            UnityEngine.ColorUtility.TryParseHtmlString(enumHexes[rivals[1]], out var rivalTwoColor);
            rivalTwoImage.color = rivalTwoColor;
            
            UnityEngine.ColorUtility.TryParseHtmlString(enumHexes[game.Theme], out var themeColor);
            themeImage.color = themeColor;

            var inputs = game.Inputs.Split().ToArray();
            for (var i = 0; i < inputs.Length; i++)
            {
                inputImages[i].gameObject.SetActive(true);
                UnityEngine.ColorUtility.TryParseHtmlString(enumHexes[inputs[i]], out var inputColor);
                inputImages[i].color = inputColor;
            }
            for (var i = inputs.Length; i < inputImages.Length; i++) inputImages[i].gameObject.SetActive(false);

            textMesh.text = game.GetLanguageSpecificInfo(Language.English)[0].Remove(0,4);
        }

        bool ISearchable<TapeSearchArguments>.IsMatch(TapeSearchArguments arguments)
        {
            var info = game.GetLanguageSpecificInfo(Language.English);
            
            var lowercaseName = info[0].ToLower().Replace(" ", "");
            var lowercaseSearch = arguments.SearchedContent.ToLower().Replace(" ", "");

            var isContentMatch = lowercaseName.Contains(lowercaseSearch);
            var isRivalMatch = game.AssociatedRivals.Any(rival => arguments.Rivals.HasFlag(rival)) || arguments.Rivals == Rivals.None;
            var isInputMatch = game.Inputs.Split().Any(input => arguments.Inputs.HasFlag(input)) || arguments.Inputs == Inputs.None;
            var isThemeMatch = arguments.Themes.Contains(game.Theme) || arguments.Themes.Length == 0;
            
            return isContentMatch && isRivalMatch && isInputMatch && isThemeMatch;
        }

        void ISearchable<TapeSearchArguments>.Show() => gameObject.SetActive(true);
        void ISearchable<TapeSearchArguments>.Hide() => gameObject.SetActive(false);
    }
}