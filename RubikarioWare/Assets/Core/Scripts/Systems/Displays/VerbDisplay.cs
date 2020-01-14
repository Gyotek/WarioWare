using System.Collections;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Core
{
    public class VerbDisplay : MonoBehaviour
    {
        [SerializeField] private float displayDuration;
        [SerializeField] private GameSequenceHandler gameSequenceHandler = default;
        
        [SerializeField] private StringUnityEvent onShow = default;
        [SerializeField] private UnityEvent onHide = default;
        
        public string CurrentVerb { get; private set; } = default;
        public bool IsDisplayOnGoing => timer.IsComplete;
        
        private Timer timer = new Timer(0f);
        
        void Awake() => timer.Assign(onHide.Invoke);
        void Start() => displayDuration = Macro.AffectTimeByBpm(displayDuration);
        void Update() => timer.Tick(Time.deltaTime);
        
        public void BeginDisplay()
        {
            CurrentVerb = gameSequenceHandler.CurrentGame.GetLanguageSpecificInfo(Language.English)[2];
            StartDisplay();
        }
        public void BeginDisplay(string verb)
        {
            CurrentVerb = verb;
            StartDisplay();
        }
        public void BeginDisplay(string verb, float displayDuration)
        {
            CurrentVerb = verb;
            StartDisplay(displayDuration);
        }

        private void StartDisplay(float customDisplayDuration = 0)
        {
            onShow?.Invoke(CurrentVerb);
            timer.Start(customDisplayDuration == 0 ? displayDuration : customDisplayDuration);
        }
    }
}