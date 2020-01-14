using Game.Core.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Game.Core
{
    public class VisualNovelHandler : MonoBehaviour, ISavable
    {
        [SerializeField] private TimelineAsset timelineAsset = default;
        [SerializeField] private PlayableDirector director = default;
        [FormerlySerializedAs("eventHandler")] [SerializeField] private EventWrapper eventWrapper = default;
        
        [ShowInInspector, ReadOnly] private bool hasBeenUsed = false;

        void Start() => SaveSystem.LoadData(GetInstanceID(), "VisualNovels");

        public void Launch()
        {
            if (!hasBeenUsed)
            {
                director.Play(timelineAsset);
                hasBeenUsed = true;
                SaveSystem.SaveData(GetInstanceID(), "VisualNovels");
            }
            else eventWrapper.Execute();
        }

        string[] ISavable.Serialize() => new string[] {hasBeenUsed.ToString()};
        void ISavable.Deserialize(string[] data) => hasBeenUsed = bool.Parse(data[0]);
    }
}