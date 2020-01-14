using System.Linq;
using UnityEngine;

namespace Game.Core
{
    public class BossSequenceGenerator : MonoBehaviour
    {
        [SerializeField] private AssetGroup gameGroup;
        [SerializeField] private StorySequence bossSequence;
 
        public void Generate()
        {
            var games = gameGroup.GetAssets<GameID>().ToList();
            while (games.Count != 30)
            {
                var randomIndex = Random.Range(0, games.Count);
                games.RemoveAt(randomIndex);
            }
            
            bossSequence.SetSequence(games.ToArray());
        }
    }
}