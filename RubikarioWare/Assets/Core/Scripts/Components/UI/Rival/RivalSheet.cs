using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class RivalSheet : MonoBehaviour
    {
        [SerializeField] private IntVariable difficultyAtom;
        [SerializeField] private IntVariable BPMAtom;

        [Space]
        
        [SerializeField] private StorySequence storySequence;
        [SerializeField] private GameSequenceHandler gameSequenceHandler;

        [Space] 
        
        [SerializeField] private SuperTextMesh rivalNameTextMesh;
        [SerializeField] private SuperTextMesh rivalNicknameTextMesh;
        [SerializeField] private SuperTextMesh sequenceNameTextMesh;
        
        [Space]
        
        [SerializeField] private RawImage rivalImage;
        [SerializeField] private Image starImage;
        [SerializeField] private Color enabledColor, disabledColor;

        private void Start() => SetSequence(storySequence);
        
        public void SetSequence(StorySequence storySequence)
        { 
            this.storySequence = storySequence;
            Refresh();
        }

        public void Refresh()
        {
            difficultyAtom.SetValue(storySequence.Difficulty);
            
            rivalNameTextMesh.text = storySequence.RivalName;
            rivalNicknameTextMesh.text = storySequence.RivalNickname;

            sequenceNameTextMesh.text = storySequence.SequenceName;
            
            rivalImage.texture = storySequence.RivalThumbnail.texture;
            starImage.color = this.storySequence.hasBeenPerfectlyCompleted ? enabledColor : disabledColor;
        }
        
        public void LaunchSequence() => gameSequenceHandler.StartNewSequence(storySequence as IGameSequence);

        public void ChangeSpeedUpSound(AudioSource source)
        {
            if (storySequence.bpmAudioClips.Length != 3) return;
            
            switch (BPMAtom.Value)
            {
                case 96:
                    source.clip = storySequence.bpmAudioClips[0];
                    source.Play();
                    return;
                case 120:
                    source.clip = storySequence.bpmAudioClips[1];
                    source.Play();
                    return;
                case 160:
                    source.clip = storySequence.bpmAudioClips[2];
                    source.Play();
                    return;
            }
           
        }
    }
}
