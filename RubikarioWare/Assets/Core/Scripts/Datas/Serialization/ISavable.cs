namespace Game.Core.Serialization
{
    public interface ISavable
    {
        string[] Serialize();
        void Deserialize(string[] data);
    }
}