namespace Game.Core
{
    [System.Serializable]
    public struct PlayData
    {
        public int playCount { get; private set; }
        public int winCount { get; private set; }

        public PlayData(int _playCount, int _winCount)
        {
            playCount = _playCount;
            winCount = _winCount;
        }

        public void IncrementPlayCount()
        {
            playCount++;
        }

        public void IncrementWinCount()
        {
            winCount++;
        }

        public void SetPlayCount(int _value)
        {
            playCount = _value;
        }

        public void SetWinCount(int _value)
        {
            winCount = _value;
        }
    }
}