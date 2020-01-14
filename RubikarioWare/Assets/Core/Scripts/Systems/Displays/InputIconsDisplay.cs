using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class InputIconsDisplay : MonoBehaviour
    {
        #region EncapsuledTypes

        [Serializable]
        private struct InputColorPair
        {
            [SerializeField, EnumPaging] private Inputs input;
            public Inputs Input => input;
            
            [SerializeField] private Color color;
            public Color Color => color;
        }

        #endregion

        [SerializeField] private InputColorPair[] pairs;
        private Dictionary<Inputs, Color> dictionary;
        
        [Space]
        
        [SerializeField] private GameObject bottomIcons;
        [SerializeField] private Image[] images;

        private GameID displayedGame;
        
        void Start()
        {
            dictionary = new Dictionary<Inputs, Color>();
            foreach (var pair in pairs) dictionary.Add(pair.Input, pair.Color);
        }

        public void SetDisplayedGame(GameID game) => displayedGame = game;
        
        public void Refresh()
        {
            var inputs = FetchInputs();
            
            bottomIcons.SetActive(inputs.Length > 2);
            for (var i = 0; i < images.Length; i++)
            {
                if (i < inputs.Length)
                {
                    images[i].gameObject.SetActive(true);
                    images[i].color = dictionary[inputs[i]];
                }
                else images[i].gameObject.SetActive(false);
            }
        }

        protected virtual Inputs[] FetchInputs() => displayedGame.Inputs.Split().ToArray();
    }
}