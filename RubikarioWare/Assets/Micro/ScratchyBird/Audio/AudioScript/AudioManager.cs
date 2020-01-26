using UnityEngine.Audio;
using System;
using UnityEngine;

namespace Game.ScratchyBird
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public AudioMixerGroup mixerGroupMusic;
        public AudioMixerGroup mixerGroupSound;

        public Sound[] musics;
        public Sound[] sounds;

        public enum MUSIC
        {
            //Music,
        };

        public enum SFX
        {
            Applauses, 
            Loose, 
            LooseTube, 
            Pong, 
            Win, 
            WingFlap,
        }

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            foreach (Sound s in musics)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;

                if (s.source.outputAudioMixerGroup == null)
                {
                    s.source.outputAudioMixerGroup = mixerGroupMusic;
                }
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;

                if (s.source.outputAudioMixerGroup == null)
                {
                    s.source.outputAudioMixerGroup = mixerGroupSound;
                }
            }
        }

        private void Start()
        {
            //PlayMusic(MUSIC.Exemple);
        }


        public void PlaySFX(SFX sfx)
        {
            switch (sfx)
            {
                case SFX.Applauses:
                    Play(sounds, "Applauses", true);
                    break;
                case SFX.Loose:
                    Play(sounds, "Loose", true);
                    break;
                case SFX.LooseTube:
                    Play(sounds, "LooseTube", true);
                    break;
                case SFX.Pong:
                    Play(sounds, "Pong", true);
                    break;
                case SFX.Win:
                    Play(sounds, "Win", true);
                    break;
                case SFX.WingFlap:
                    Play(sounds, "WingFlap", true);
                    break;
            }
        }

        public void PlayMusic(MUSIC musicsEnum)
        {
            switch (musicsEnum)
            {
                /*
                case MUSIC.Exemple:
                    Play(musics, "Exemple");
                    break;
                */
            }
        }

        public void StopMusic(MUSIC musicsEnum)
        {
            switch (musicsEnum)
            {
                /*
                case MUSIC.Music:
                    StopPlaying(musics, "Exemple");
                    break;
                */
            }
        }

        public void StopSfx(SFX sfxEnum) //Only for looping SFX
        {
            switch (sfxEnum)
            {
                /* 
                case SFX.Laser:
                    StopPlaying(sounds, "Exemple");
                    break;
                */
            }
        }

        private void Play(Sound[] sounds, string sound, bool SFX = false, bool doNotLoop = false)
        {
            Sound s = Array.Find(sounds, item => item.name == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            if (doNotLoop)
            {
                s.source.loop = false;
            }

            if (!SFX)
            {
                s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
                s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));


                s.source.Play();
            }
            else
            {
                s.source.PlayOneShot(s.clip);
            }
        }

        private void StopPlaying(Sound[] sounds, string sound)
        {
            Sound s = Array.Find(sounds, item => item.name == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

            s.source.Stop();
        }
    }
}
