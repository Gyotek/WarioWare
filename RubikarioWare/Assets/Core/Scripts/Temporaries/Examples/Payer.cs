using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Example
{
    public class Payer : MicroMonoBehaviour
    {
        [SerializeField] private CircleCollider2D targetCollider = default;
        [SerializeField] private int clickGoal;

        private int clickCount = 0;
        private bool isGameStarted = false;

        void Start()
        {
            Macro.StartGame();
            clickGoal *= Macro.Difficulty;
        }

        protected override void OnGameStart() => Macro.DisplayActionVerb("Catch !", 3);
        protected override void OnActionVerbDisplayEnd() => isGameStarted = true;
            
        protected override void OnTimerEnd()
        {
            Macro.Lose();
            Macro.EndGame();
        }

        void Update()
        {
            if(isGameStarted && Input.GetMouseButtonDown(0))
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (targetCollider.OverlapPoint(mousePosition))
                {
                    clickCount++;
                    if(clickCount == clickGoal)
                    {
                        Macro.Win();
                        Macro.EndGame();
                    }
                }
            }
        }
    }

}