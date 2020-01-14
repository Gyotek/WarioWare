using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Game.Core.Serialization;

using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Utilities;

using Random = UnityEngine.Random;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "StorySequence", menuName = "CleanUp/Story Sequence")]
    public class StorySequence : GameSequence, ISavable, IAudioSequence
    {
        [SerializeField, Range(1,3)] private int difficulty;
        public int Difficulty => difficulty;
        
        [SerializeField, MinValue(1)] private int bpm96Threshold;
        [SerializeField, MinValue(2)] private int bpm120Threshold;
        
        [SerializeField] public AudioClip[] bpmAudioClips;
        public AudioClip[] Clips => bpmAudioClips;
        public AudioClip[] TransitionClips => null;

        [Space] 
        
        [SerializeField] private Rivals rival;
        public string RivalName => rival.ToString();
        
        [SerializeField] private string rivalNickname;
        public string RivalNickname => rivalNickname;

        [SerializeField] private Enum sequenceType;
        public string SequenceName => sequenceType.ToString();

        [SerializeField, PreviewField] private Sprite rivalThumbnail;
        public Sprite RivalThumbnail => rivalThumbnail;

        [Space] 
        
        [ReadOnly] public bool isAccessible;
        [ReadOnly] public bool hasBeenPerfectlyCompleted;
        
        [Button]
        protected override void GenerateSequence()
        {
            var bpm160Threshold = sequence.Length;

            var thresholds = new Dictionary<Rythm, int>()
            {
                {Rythm.Bpm96, bpm96Threshold},
                {Rythm.Bpm120, bpm120Threshold},
                {Rythm.Bpm160, bpm160Threshold}
            };
            var groups = new Dictionary<Rythm, List<GameID>>()
            {
                {Rythm.Bpm96, new List<GameID>()},
                {Rythm.Bpm120, new List<GameID>()},
                {Rythm.Bpm160, new List<GameID>()}
            };
            var remainder = new List<GameID>();
            
            foreach (var game in sequence)
            {
                var rythmConstraint = GetGameRythmConstraint(game);
                
                if(rythmConstraint == Rythm.None) remainder.Add(game);
                else
                {
                    if(groups[rythmConstraint].Count < thresholds[rythmConstraint]) groups[rythmConstraint].Add(game);
                    else remainder.Add(game);
                }
            }

            AddToSequence(Rythm.Bpm96);
            AddToSequence(Rythm.Bpm120);
            AddToSequence(Rythm.Bpm160);
            foreach (var game in remainder) currentSequence.Insert(Random.Range(0, currentSequence.Count), game);
            
            Rythm GetGameRythmConstraint(GameID game)
            {
                var constraints = game.RythmConstraints.Split().ToList();

                if (constraints.Count == 0) return Rythm.None;
                if (constraints.Count > 1)
                {
                    while (constraints.Count > 0)
                    {
                        var rand = constraints[Random.Range(0, constraints.Count)];

                        if (groups[rand].Count < thresholds[rand]) return rand;
                        else constraints.Remove(rand);
                    }
                    return Rythm.None;
                }
                else return constraints.First();
            }
            void AddToSequence(Rythm rythmConstraint)
            {
                var group = groups[rythmConstraint];
                if(group.Count > 1) group.Shuffle();
                
                currentSequence.AddRange(group);
            }
        }

        string[] ISavable.Serialize()
        {
            return new string[]
            {
                isAccessible.ToString(),
                hasBeenPerfectlyCompleted.ToString()
            };
        }

        void ISavable.Deserialize(string[] data)
        {
            isAccessible = bool.Parse(data[0]);
            hasBeenPerfectlyCompleted = bool.Parse(data[1]);
        }
    }
}

