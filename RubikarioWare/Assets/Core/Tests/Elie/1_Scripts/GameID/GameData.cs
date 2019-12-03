namespace Game.Core
{
    [System.Serializable]
    public struct GameData
    {
        public PlayData easyDifficulty;
        public PlayData mediumDifficulty;
        public PlayData hardDifficulty;

        public PlayData general
        {
            get
            {
                return new PlayData(easyDifficulty.playCount + mediumDifficulty.playCount + hardDifficulty.playCount, easyDifficulty.winCount + mediumDifficulty.winCount + hardDifficulty.winCount);
            }
        }
    }
}