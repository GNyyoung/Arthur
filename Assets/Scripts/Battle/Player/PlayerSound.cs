using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerSound : MonoBehaviour
    {
        public enum SoundType
        {
            None,
            Slash,
            UpperSlash,
            Stab,
            Defence,
            Damage
        }
        
        public AudioClip slashSound;
        public AudioClip upperSlashSound;
        public AudioClip stabSound;
        public AudioClip defenceSound;
        public AudioClip damageSound;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void OutputSound(SoundType soundType)
        {
            AudioClip audioClip = null;
            switch (soundType)
            {
                case SoundType.Slash:
                    audioClip = slashSound;
                    break;
                case SoundType.UpperSlash:
                    audioClip = upperSlashSound;
                    break;
                case SoundType.Stab:
                    audioClip = stabSound;
                    break;
                case SoundType.Defence:
                    audioClip = defenceSound;
                    break;
                case SoundType.Damage:
                    audioClip = damageSound;
                    break;
            }

            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}