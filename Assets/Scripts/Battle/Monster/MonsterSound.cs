using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    public enum SoundType
    {
        Defence,
        Damage,
        Die,
        SkillAlarm,
    }
    
    private AudioSource audioSource;
    
    public AudioClip defenceSound;
    public AudioClip damageSound;
    public AudioClip dieSound;
    public AudioClip skillAlarmSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OutputSound(MonsterSound.SoundType soundType)
    {
        AudioClip audioClip = null;
        switch (soundType)
        {
            case SoundType.Defence:
                audioClip = defenceSound;
                break;
            case SoundType.Damage:
                audioClip = damageSound;
                break;
            case SoundType.Die:
                audioClip = dieSound;
                break;
            case SoundType.SkillAlarm:
                audioClip = skillAlarmSound;
                break;
        }

        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
