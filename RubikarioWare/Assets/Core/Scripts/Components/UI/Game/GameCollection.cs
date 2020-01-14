using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

namespace Game.Core
{
    public class GameCollection : MonoBehaviour
    {
        [SerializeField] private TapeSearchBar tapeSearchBar;
        [SerializeField] private AssetGroup gameIdGroup;
        
        [Space(15)]
        
        [SerializeField] private RectTransform contentDestination;
        [SerializeField] private GameTape contentTemplate;

        [Space(15)]
        
        [SerializeField] private GameSheet sheet;

        public void Refresh()
        {
            var tapes = new List<GameObject>();
            for (var i = 0; i < contentDestination.childCount; i++) tapes.Add(contentDestination.GetChild(i).gameObject);
            foreach (var tape in tapes) Destroy(tape);

            tapeSearchBar.Searchables.Clear();
            
            var games = gameIdGroup.GetAssets<GameID>();
            foreach (var game in games)
            {
                if (game.GetPlayCount() == 0) continue;
                
                var tape = Instantiate(contentTemplate, contentDestination);
                tape.Initialize(game);
                
                tapeSearchBar.Searchables.Add(tape);
                sheet.RegisterGameTape(tape);
            }

            StartCoroutine(RefreshSheetRoutine(games.First()));
        }

        private IEnumerator RefreshSheetRoutine(GameID firstGame)
        {
            yield return new WaitForEndOfFrame();
            
            if (contentDestination.childCount > 0) sheet.Refresh(firstGame);
            else sheet.Reset();
        }
    }
}