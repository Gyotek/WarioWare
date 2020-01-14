using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "Cheat Codes", menuName = "Wario Ware/Macro/Cheat Codes")]
    public class CheatCodes : ScriptableObject
    {
        [SerializeField] CheatCode[] cheatCodes = default;

        public void InputCheat(string code)
        {
            foreach (var cheatCode in cheatCodes)
                if (cheatCode.code.Equals(code))
                    cheatCode.OnInput?.Invoke();
        }
    }
}