using System.Collections;

using TMPro;

namespace Game.Core
{
    public class AtomTextMeshLinker : AtomTextLinker<TextMeshProUGUI>
    {
        public override void Refresh() => value.text = GetText();
    }
}