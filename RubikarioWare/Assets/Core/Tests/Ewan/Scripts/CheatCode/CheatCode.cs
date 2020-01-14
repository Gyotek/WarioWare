using System;
using UnityEngine.Events;

namespace Game.Core
{
	[Serializable]
    public struct CheatCode
    {
        public string code;
        public UnityEvent OnInput;
    }
}