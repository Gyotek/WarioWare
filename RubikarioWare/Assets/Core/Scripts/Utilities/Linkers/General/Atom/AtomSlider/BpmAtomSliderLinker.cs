using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class BpmAtomSliderLinker : AtomSliderLinker
    {
        [SerializeField] private AudioSource macroSoundtrackSource;
        [SerializeField] private AudioClip[] macroClips;
        
        private Dictionary<int, int> bpmValues = new Dictionary<int, int>()
        {
            {1,96},
            {2,120},
            {3,160}
        };

        protected override void RefreshAtomValue(float value)
        {
            base.RefreshAtomValue(value);
            macroSoundtrackSource.clip = macroClips[(int) value];
        }

        protected override float GetSliderValue(float value)
        {
            return bpmValues.TryGetValue((int) value, out var result) ? result : 96;
        }
        protected override string GetNumericText(float value)
        {
            var traduction = GetSliderValue(value);
            return value == 1 ? $"<c=A29999>0</c>{traduction}" : traduction.ToString();
        }
    }
}