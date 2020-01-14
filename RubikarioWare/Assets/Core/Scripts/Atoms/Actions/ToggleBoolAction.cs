using UnityEngine;

namespace UnityAtoms
{
    using static AtomMenu;

    [CreateAssetMenu(fileName = "Toggle", menuName = Actions + "Toggle", order = Order)]
    public class ToggleBoolAction : VoidAction
    {
        [SerializeField] BoolVariable boolVariable = default;

        public override void Do()
        {
            boolVariable.SetValue(!boolVariable.Value);
        }
    }
}
