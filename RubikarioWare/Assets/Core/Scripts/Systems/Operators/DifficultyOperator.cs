using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public class DifficultyOperator : MonoBehaviour
    {
        [SerializeField] private IntVariable difficultyAtom = default;
        [SerializeField] private AtomTextMeshLinker difficultyTextMeshLinker;
        
        private int currentAdvancement;
        private Difficulty[] difficulties = new[]
        {
            Difficulty.Medium,

            Difficulty.Easy,
            Difficulty.Medium,
            Difficulty.Hard,

            Difficulty.Easy,
            Difficulty.Medium,

            Difficulty.Easy,
            Difficulty.Medium,
            Difficulty.Hard,

            Difficulty.Easy,
            Difficulty.Medium,
            Difficulty.Hard,

            Difficulty.Medium,
            Difficulty.Hard,

            Difficulty.Medium,
            Difficulty.Hard,
            
            Difficulty.Medium,
            Difficulty.Hard
        };

        public void Initialize() => currentAdvancement = 0;
        public void ChangeDifficulty()
        {
            var value = (int)difficulties[currentAdvancement];
            difficultyAtom.SetValue(value + 1);
            currentAdvancement++;
            
            difficultyTextMeshLinker.Refresh();
        }
    }
}