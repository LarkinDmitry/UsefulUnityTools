using System.Collections.Generic;
using UnityEngine;

namespace KAKuBCE.UsefulUnityTools
{
    public class SoundPlayer : MonoBehaviour
    {
        private Dictionary<string, AudioSource> sounds = new();

        public void AddSound(string soundName, string path, bool playOnAwake = false, bool isLooped = false, float volume = 1)
        {
            if(sounds.ContainsKey(soundName))
            {
                Debug.LogError($"sound - \"{soundName}\" is already in the library");
            }
            else
            {
                AudioClip clip = Resources.Load<AudioClip>(path);
                
                if(clip == null )
                {
                    Debug.LogError($"file - \"{path}\" not found in Resources folder");
                }
                else
                {
                    AudioSource sound = gameObject.AddComponent<AudioSource>();
                    sound.clip = clip;
                    sound.playOnAwake = playOnAwake;
                    sound.loop = isLooped;
                    sound.volume = volume;
                    sounds.Add(soundName, sound);
                }
            }
        }

        public void Remote(string soundName)
        {
            if (sounds.TryGetValue(soundName, out var audioSource))
            {
                Destroy(audioSource);
            }
            else
            {
                Debug.LogError($"sound - \"{soundName}\" not found");
            }
        }

        public void Play(string soundName)
        {
            if(sounds.TryGetValue(soundName, out var audioSource))
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError($"sound - \"{soundName}\" not found");
            }
        }

        public void Pause(string soundName)
        {
            if (sounds.TryGetValue(soundName, out var audioSource))
            {
                audioSource.Pause();
            }
            else
            {
                Debug.LogError($"sound - \"{soundName}\" not found");
            }
        }

        public void Stop(string soundName)
        {
            if (sounds.TryGetValue(soundName, out var audioSource))
            {
                audioSource.Stop();
            }
            else
            {
                Debug.LogError($"sound - \"{soundName}\" not found");
            }
        }

        public void SetVolume(string soundName, float volume)
        {
            if (sounds.TryGetValue(soundName, out var audioSource))
            {
                audioSource.volume = ConvertToDecibel(volume);
            }
            else
            {
                Debug.LogError($"sound - \"{soundName}\" not found");
            }

            float ConvertToDecibel(float value)
            {
                float vol = Mathf.Clamp(value, 0.0001f, 1);
                return Mathf.Log10(vol) * 20;
            }
        }

        public void SetPitch(string soundName, float pitch)
        {
            if (sounds.TryGetValue(soundName, out var audioSource))
            {
                audioSource.pitch = pitch;
            }
            else
            {
                Debug.LogError($"sound - \"{soundName}\" not found");
            }
        }
    }
}