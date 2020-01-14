using System;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class RivalCollection : MonoBehaviour
    {
        #region EncapsuledTypes

        [Serializable]
        private struct ButtonSequencePair
        {            
            public ButtonSequencePair(StorySequence sequence, Button button)
            {
                this.sequence = sequence;
                this.button = button;
            }
            
            [SerializeField] private StorySequence sequence;
            public StorySequence Sequence => sequence;
            
            [SerializeField] private Button button;
            public Button Button => button;
        }

        #endregion

        [SerializeField] private RivalSheet sheet;
        
        [Space]
        
        [SerializeField] private FloatVariable healthAtom;
        [SerializeField] private ButtonSequencePair[] pairs;
        
        private int currentPairIndex = 0;

        
        public void Refresh()
        {
            foreach (var pair in pairs) pair.Button.interactable = pair.Sequence.isAccessible;
        }
        public void SetCurrentGameIndex(int index) => currentPairIndex = index - 1;
        public void TryUnlock()
        {
            if (healthAtom.Value == 3)
            {
                pairs[currentPairIndex].Sequence.hasBeenPerfectlyCompleted = true;
                sheet.Refresh();
            }
            if (healthAtom.Value > 0 && currentPairIndex + 1 < pairs.Length) pairs[currentPairIndex + 1].Sequence.isAccessible = true;
            Refresh();
        }
    }
}