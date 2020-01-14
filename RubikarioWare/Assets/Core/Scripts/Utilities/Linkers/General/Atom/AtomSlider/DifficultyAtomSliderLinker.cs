using UnityEngine;

namespace Game.Core
{
    public class DifficultyAtomSliderLinker : AtomSliderLinker
    {
        protected override string GetNumericText(float value) =>  $"<c=A29999>00</c>{value}";
    }
}