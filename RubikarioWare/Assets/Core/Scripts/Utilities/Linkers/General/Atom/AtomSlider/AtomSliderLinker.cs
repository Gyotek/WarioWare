using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class AtomSliderLinker : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        [Space(15)] 
        
        [SerializeField] private bool hasNumericDisplay;
        [SerializeField, ShowIf("hasNumericDisplay")] private SuperTextMesh numericText;

        [Space(15)]
        
        [SerializeField] protected AtomBaseVariable atom;
        [SerializeField] private bool isInteger;

        void Start() => slider.onValueChanged.AddListener(RefreshAtomValue);

        protected virtual void RefreshAtomValue(float value)
        {
            if (isInteger) ((IntVariable)atom).SetValue(Mathf.RoundToInt(GetSliderValue(value)));
            else ((FloatVariable)atom).SetValue(GetSliderValue(value));

            if (hasNumericDisplay) numericText.text = GetNumericText(value);
        }

        protected virtual float GetSliderValue(float value) => value;
        protected virtual string GetNumericText(float value) => value.ToString();
    }
}