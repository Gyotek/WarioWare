using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class MicroSceneLauncher : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public bool IsStarted { get; set; } = true;

        [Space] 
        
        [SerializeField, Range(1, 3)] private int desiredDifficulty;
        [SerializeField] private IntVariable difficultyAtom;
        
        [Space]
        
        [SerializeField, Range(1, 3)] private int desiredBpm;
        [SerializeField] private IntVariable bpmAtom;

        private Dictionary<int, int> bpmTraductions = new Dictionary<int, int>() {{1, 96}, {2, 120}, {3, 160}};

        [Space] 
        
        [SerializeField] private BeatSystem beatSystem;
        [SerializeField] private GameContextHandler gameContextHandler;
        [SerializeField] private GameInfoContextLinker gameInfoContextLinker;
        [SerializeField] private CallbackManager callbackManager;
        
        [Space]
        
        [SerializeField] private Vector2 anchor = new Vector2(10f, 10f);
        [SerializeField] private float margin = 10f;
        [SerializeField] private float width = 200f;
        [SerializeField] private float lineHeight = 25f;
        [SerializeField] private float adjusment = 0f;

        private int buildIndex = -1;
        private int GUICount = 0;
        
        private void Awake()
        {
            beatSystem.Play();
            RefreshAtomValues();
        }

        private void RefreshAtomValues()
        {
            difficultyAtom.SetValue(desiredDifficulty);
            bpmAtom.SetValue(bpmTraductions[desiredBpm]);
        }
        
        private void OnGUI()
        {
            if (IsStarted) return;
            
            DrawLabel($"Difficulty : {desiredDifficulty}");
            desiredDifficulty = (int)DrawSlider(desiredDifficulty, 1, 3);
            
            DrawLabel($"Bpm : {bpmTraductions[desiredBpm]}");
            desiredBpm = (int)DrawSlider(desiredBpm, 1, 3);

            if (DrawButtons("Restart")[0])
            {
                var microScene = SceneManager.GetSceneAt(1);

                if (buildIndex == -1) buildIndex = microScene.buildIndex;
                
                var unloadOperation = SceneManager.UnloadSceneAsync(microScene);
                unloadOperation.completed += op =>
                {
                    callbackManager.AllowCallbacks = true;
                    SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
                };

                IsStarted = true;
                RefreshAtomValues();
                
                gameInfoContextLinker.ResetWinIndicator();
                gameContextHandler.SetCurrentState(GameStates.Loaded);
            }
            
            GUI.Box(new Rect(anchor.x, anchor.y, width + (margin * 2), (GUICount * lineHeight) + (margin * 2)), GUIContent.none);
            GUICount = 0;
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
    }
}