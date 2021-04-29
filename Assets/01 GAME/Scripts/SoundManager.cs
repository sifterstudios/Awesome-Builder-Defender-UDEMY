using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }


    AudioSource _audioSource;
    Dictionary<Sound, AudioClip> _soundAudioClipDictionary;
    float volume = .5f;

    void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in Enum.GetValues(typeof(Sound)))
        {
            _soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(Sound.BuildingPlaced.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        _audioSource.PlayOneShot(_soundAudioClipDictionary[sound], volume);
    }

    public void IncVol()
    {
        volume += .1f;
        volume = Mathf.Clamp01(volume);
    }

    public void DecVol()
    {
        volume -= .1f;
        volume = Mathf.Clamp01(volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}