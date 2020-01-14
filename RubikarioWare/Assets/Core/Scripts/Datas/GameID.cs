using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Game;
using Game.Core.Serialization;

using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "GameID", menuName = "CleanUp/GameID")]
    public class GameID : ScriptableObject, ICheatAccessible, Iinitializable, ISavable
    {
        void Iinitializable.Intialize(params object[] values)
        {
            titles = values[0] as string[];
            descriptions = values[1] as string[];
            actionVerbs = values[2] as string[];
            
            scene = values[3] as SceneField;
            thumbnail = values[4] as Sprite;
            
            designer = values[5] as string[];
            developer = values[6] as string[];

            inputs = (Inputs)values[7];
            rythmConstraints = (Rythm)values[8];
            associatedRivals = (Rivals)values[9];
            theme = (Theme)values[10];
        }
        
        [SerializeField] private string[] titles;
        [SerializeField] private string[] descriptions;
        [SerializeField] private string[] actionVerbs;
        
        [Space]
        
        [SerializeField] private SceneField scene;
        public SceneField Scene => scene;
        
        [SerializeField, PreviewField] private Sprite thumbnail = default;
        public Sprite Thumbnail => thumbnail;

        [Space] [SerializeField] private string[] designer = new string[2];
        public string Designer => designer[0] + designer[1];
        
        [SerializeField] private string[] developer = new string[2];
        public string Developer => developer[0] + developer[1];
        
        [Space]
        
        [SerializeField] private Inputs inputs = default;
        public Inputs Inputs => inputs;
        
        [SerializeField] private Rythm rythmConstraints;
        public Rythm RythmConstraints => rythmConstraints;

        [SerializeField] private Rivals associatedRivals = Rivals.Melo | Rivals.Theo;
        public IEnumerable<Rivals> AssociatedRivals => associatedRivals.Split();

        [SerializeField] private Theme theme;
        public Theme Theme => theme;
        
        private int[] playCounts = new int[] {0, 0, 0};
        private int[] winCounts = new int[] {0, 0, 0};
 
        object[] ICheatAccessible.objects => new object[] { playCounts, winCounts };

        public string[] GetLanguageSpecificInfo(Language language)
        {
            var index = (int)language;
            return new string[]
            {
                titles[index],
                descriptions[index],
                actionVerbs[index]
            };
        }
        
        public void IncrementPlayCount(Difficulty difficulty, bool wasWon)
        {
            var index = (int) difficulty;
            
            playCounts[index]++;
            for (var i = index - 1; i >= 0; i--)
            {
                if (playCounts[i] == 0) playCounts[i] = 1;
            }

            if (!wasWon) return;
            
            winCounts[index]++;
            for (var i = index - 1; i >= 0; i--)
            {
                if (winCounts[i] == 0) winCounts[i] = 1;
            }
        }

        public int GetPlayCount() => playCounts[0] + playCounts[1] + playCounts[2];
        public int GetPlayCount(Difficulty difficulty) => playCounts[(int)difficulty];
        
        public int GetWinCount() => winCounts[0] + winCounts[1] + winCounts[2];
        public int GetWinCount(Difficulty difficulty) => winCounts[(int)difficulty];

        string[] ISavable.Serialize()
        {
            return new string[]
            {
                playCounts[0].ToString(),
                playCounts[1].ToString(),
                playCounts[2].ToString(),

                winCounts[0].ToString(),
                winCounts[1].ToString(),
                winCounts[2].ToString(),
            };
        }

        void ISavable.Deserialize(string[] data)
        {
            for (var i = 0; i < 3; i++) playCounts[i] = int.Parse(data[i]);
            for (var i = 3; i < 6; i++) winCounts[i - 3] = int.Parse(data[i]);
        }
    }
}