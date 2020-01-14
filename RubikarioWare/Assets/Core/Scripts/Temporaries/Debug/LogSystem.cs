using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Game.Core
{
    public class LogSystem : MonoBehaviour
    {
        [SerializeField, BoxGroup("Value"), Range(0.25f, 10f)] private float loadTime = 1f;

        private float loadProgress = 0;

        [SerializeField, BoxGroup("References")] private IntVariable BPM = default;
        [SerializeField, BoxGroup("References")] private IntVariable BeatCount = default;
        [SerializeField, BoxGroup("References")] private IntVariable Difficulty = default;
        [SerializeField, BoxGroup("References")] private FloatVariable RemainingTime = default;
        [SerializeField, BoxGroup("References")] private VerbDisplay verbLoader = default;
        [SerializeField, BoxGroup("References")] private GameContextHandler gameState = default;
        [SerializeField, BoxGroup("References")] private MicroActivator microActivator = default;

        [SerializeField, BoxGroup("GUI")] private Vector2 anchor = new Vector2(10f, 10f);
        [SerializeField, BoxGroup("GUI")] private float margin = 10f;
        [SerializeField, BoxGroup("GUI")] private float width = 200f;
        [SerializeField, BoxGroup("GUI")] private float lineHeight = 25f;
        [SerializeField, BoxGroup("GUI")] private float adjusment = 0f;

        private int GUICount = 0;

		private void Start()
		{
            if (microActivator != null && verbLoader != null && gameState != null)
                return;
            microActivator = FindObjectOfType<MicroActivator>();
			verbLoader = FindObjectOfType<VerbDisplay>();
			gameState = FindObjectOfType<GameContextHandler>();
		}

		void OnGUI()
		{
		/*	if (gameState.State == "Not Loaded" && DrawButtons("LoadGame")[0]) StartCoroutine(FakeLoadRoutine());
            if (gameState.State == "Loading") DrawSlider(loadProgress, 0, 1);
            if (gameState.State == "Loaded" && DrawButtons("Start Game")[0]) Macro.StartGame();
            if (gameState.State == "Started" && gameState.WinState == "Unspecified")
            {
                var winStates = DrawButtons("Win", "Lose");
                if (winStates[0]) Macro.Win();
                if (winStates[1]) Macro.Lose();
            }
            if (gameState.State == "Started" && gameState.WinState != "Unspecified" && DrawButtons("End Game")[0]) Macro.EndGame();

            DrawLabel(string.Format("Game State : {0}", gameState.State));
            DrawLabel(string.Format("Win State : {0}", gameState.WinState));
            DrawLabel(string.Format("Timer : {0}", RemainingTime.Value == -1 ? "Not Started" : Math.Round(RemainingTime.Value, 2).ToString()));
            DrawLabel(string.Format("Displayed Verb : {0}", verbLoader.IsVerbDisplayed ? "None" : verbLoader.CurrentVerb));
            DrawLabel(string.Format("BeatCount : {0}", BeatCount.Value));

            DrawLabel(string.Format("BPM : {0}", BPM.Value));
           / if (gameState.State == "Not Loaded") BPM.SetValue((int)DrawSlider(BPM.Value, 1, 200));

            DrawLabel(string.Format("Difficulty : {0}", Difficulty.Value));
            if (gameState.State == "Not Loaded") Difficulty.SetValue((int)DrawSlider(Difficulty.Value, 1, 3));

            GUI.Box(new Rect(anchor.x, anchor.y, width + (margin * 2), (GUICount * lineHeight) + (margin * 2)), GUIContent.none);
            GUICount = 0;*/
        }
        private Rect GetNextRect(bool shouldHeightBeAdjusted)
        {
            var rect = default(Rect);

            if (shouldHeightBeAdjusted) rect = new Rect(anchor.x + margin, anchor.y + margin + (GUICount * lineHeight) + adjusment, width, lineHeight);
            else rect = new Rect(anchor.x + margin, anchor.y + margin + (GUICount * lineHeight), width, lineHeight);

            GUICount++;
            return rect;
        }

        private void DrawLabel(string label) => GUI.Label(GetNextRect(true), new GUIContent(label));
        private float DrawSlider(float value, float min, float max) => GUI.HorizontalSlider(GetNextRect(true), value, min, max);
        private bool[] DrawButtons(params string[] buttonLabels)
        {
            var currentRect = GetNextRect(false);
            var buttonWidth = currentRect.width / buttonLabels.Length;
            var buttonStates = new bool[buttonLabels.Length];

            for (int i = 0; i < buttonLabels.Length; i++)
            {
                var buttonRect = new Rect(currentRect.x + (buttonWidth * i), currentRect.y, buttonWidth, currentRect.height);
                buttonStates[i] = GUI.Button(buttonRect, new GUIContent(buttonLabels[i]));
            }
            return buttonStates;
        }

        private IEnumerator FakeLoadRoutine()
        {
            //gameState.SetCurrentState("Loading");
            for (float i = 0; i < loadTime; i += Time.deltaTime)
            {
                loadProgress = i / loadTime;
                yield return null;
            }
            //gameState.SetCurrentState("Loaded");
            microActivator.SetRootActives();
        }
    }
}