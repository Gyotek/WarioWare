using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game
{
    public class SliderAudioMixer : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer = default;
        [SerializeField] Slider slider = default;
        [SerializeField] string parameterName = "Master";

        private void Awake() => slider.onValueChanged.AddListener(UpdateAudioMixer);

        private void Start() => InitializeSlider();

        private void OnDestroy() => slider.onValueChanged.RemoveListener(UpdateAudioMixer);

        void InitializeSlider()
        {
            bool parameterExist = audioMixer.GetFloat(parameterName, out float value);
            bool saveExist = PlayerPrefs.HasKey(parameterName);
            if (!parameterExist)
                throw new System.Exception("Parameter doesn't exist in AudioMixer");
            if(saveExist)
               value = PlayerPrefs.GetFloat(parameterName);
            slider.SetValueWithoutNotify(value);
            UpdateAudioMixer(value);
        }

        void UpdateAudioMixer(float value)
        {
            audioMixer.SetFloat(parameterName, value);
            PlayerPrefs.SetFloat(parameterName, value);
            PlayerPrefs.Save();
        }
    }
}