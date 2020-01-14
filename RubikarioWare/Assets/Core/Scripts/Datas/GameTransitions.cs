using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "GameTransitions", menuName = "CleanUp/Game Transitions")]
    public class GameTransitions : SerializedScriptableObject
    {
        [SerializeField] private VoidEvent openingEvent = default;
        public VoidEvent OpeningEvent => openingEvent;
        
        [SerializeField] private VoidEvent closingEvent = default;
        public VoidEvent ClosingEvent => closingEvent;

        [SerializeField] private (VoidEvent transitionEvent, int[] advancements)[] transitionEventInfos;

        public void TryCallTransitionEvent(int advancement)
        {
            foreach (var transitionEventInfo in transitionEventInfos)
            {
                if (transitionEventInfo.advancements.Contains(advancement)) transitionEventInfo.transitionEvent.Raise();
            }
        }
        
        /*[DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        [SerializeField] private SortedList<int, VoidEvent[]> events = new SortedList<int, VoidEvent[]>();
        private Queue<(int threshold, VoidEvent[] transitionEvents)> runtimeEvents = new Queue<(int, VoidEvent[])>();
 
        
        public void Initialize()
        {
            runtimeEvents.Clear();
            foreach (var keyValuePair in events) runtimeEvents.Enqueue((keyValuePair.Key, keyValuePair.Value));
        }
        
        public void TryCallTransitionEvent(int advancement)
        {
            if (advancement < runtimeEvents.Peek().threshold || runtimeEvents.Count == 0) return;
            foreach (var transitionEvent in  runtimeEvents.Dequeue().transitionEvents) transitionEvent.Raise();
        }*/
    }
}
