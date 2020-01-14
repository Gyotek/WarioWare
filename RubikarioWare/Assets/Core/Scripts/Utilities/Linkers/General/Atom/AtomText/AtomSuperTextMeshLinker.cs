namespace Game.Core
{
    public class AtomSuperTextMeshLinker : AtomTextLinker<SuperTextMesh>
    {
        public override void Refresh() => value._text = GetText();
    }
}