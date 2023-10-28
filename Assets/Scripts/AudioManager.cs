using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : SingletonPersistent<AudioManager>
{
    public SoundAudioClip[] soundAudioClipsArray;


    //private GameObject[] soundAudioClips;

    [SerializeField]
    private Queue<GameObject> soundAudioClipsQueue;

    [SerializeField]
    private int maxAudioSources = 10;

    private void Start()
    {
        //soundAudioClips = new GameObject[maxAudioSources];

        soundAudioClipsQueue = new Queue<GameObject>(maxAudioSources);
    }

    public enum Sounds
    {
        PlayerDamaged,
        Trap,
        SeafoamExplosion
    }

    //Use this for sounds that may be repeated very quickly Ex: a bunch of towers shooting
    public void PlaySound(Sounds _sound)
    {
        GameObject soundGameObject;
        AudioSource audioSource;
        //Create Audio Source Game Object
        if (soundAudioClipsQueue.Count <= maxAudioSources)
        {
            soundGameObject = new GameObject("Sound");
            soundAudioClipsQueue.Enqueue(soundGameObject);
            audioSource = soundGameObject.AddComponent<AudioSource>();
        }
        else
        {
            soundGameObject = soundAudioClipsQueue.Dequeue();
            soundAudioClipsQueue.Enqueue(soundGameObject);
            audioSource = soundGameObject.GetComponent<AudioSource>();
            audioSource.Stop();
        }
        audioSource.clip = GetAudioClip(_sound).audioClip;
        audioSource.volume = GetAudioClip(_sound).volume;
        audioSource.PlayOneShot(audioSource.clip);
    }

    // Use this for sound effects where you want to hear the entire sound effect ex: Lightning
    // WARNING: using this with sound effects that are repeated often may result in broken audio
    public void PlayEntireSound(Sounds _sound)
    {
        //Create Audio Source Game Object
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(_sound).audioClip;
        audioSource.volume = GetAudioClip(_sound).volume;
        audioSource.priority = 50;
        audioSource.PlayOneShot(audioSource.clip);
        Destroy(soundGameObject, audioSource.clip.length);
    }

    private SoundAudioClip GetAudioClip(Sounds _sound)
    {
        foreach(SoundAudioClip soundAudioClip in soundAudioClipsArray)
        {
            if(soundAudioClip.sound == _sound)
            {
                return soundAudioClip;
            }
        }

        Debug.LogError("Sound " + soundAudioClipsArray + "not found!");
        return null;
    }

    [Serializable]
    public class SoundAudioClip
    {
        public Sounds sound;
        public AudioClip audioClip;

        [SerializeField, Range(0f, 1f)]
        public float volume;
    }

}
