using UnityAtoms;

namespace Game.Core
{
    public interface IGameSequence
    {
        void Initialize();
        bool TryGetNextGame(out GameID nextGame);
        void TryCallTransitionEvent();
        
        GameID FirstGame { get; }
        
        GameID[] Games { get; }
        int Advancement { get; }
        
        VoidEvent OpeningTransitionEvent { get; }
        VoidEvent ClosingTransitionEvent { get; }
    }
}