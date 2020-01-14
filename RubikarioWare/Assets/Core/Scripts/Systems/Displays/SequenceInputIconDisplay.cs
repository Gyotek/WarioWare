using System.Linq;
using UnityEngine;

namespace Game.Core
{
    public class SequenceInputIconDisplay : InputIconsDisplay
    {
        [Space, SerializeField] private GameSequenceHandler gameSequenceHandler;

        protected override Inputs[] FetchInputs() => gameSequenceHandler.CurrentGame.Inputs.Split().ToArray();
    }
}