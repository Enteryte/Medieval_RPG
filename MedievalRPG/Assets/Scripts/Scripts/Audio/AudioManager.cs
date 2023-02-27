using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<AudioSource> environment = new List<AudioSource>();
    private List<AudioSource> voice = new List<AudioSource>();
    private List<AudioSource> music = new List<AudioSource>();
    private List<AudioSource> effects = new List<AudioSource>();

    public float MasterVolume = 1.0f;
    public float EnvironmentalVolume = 0.5f;
    public float VoiceVolume = 0.5f;
    public float MusicVolume = 0.5f;
    public float EffectsVolume = 0.5f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        SortLists();
        SetMasterVolume();
    }

    private void OnLevelWasLoaded(int level)
    {
        ClearLists();
        SortLists();
        SetMasterVolume();
    }

    private void ClearLists()
    {
        environment.Clear();
        voice.Clear();
        music.Clear();
        effects.Clear();
    }

    private void SortLists()
    {
        AudioSource[] sources = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in sources)
        {
            AudioVolumeHandler audio = source.gameObject.GetComponent<AudioVolumeHandler>();
            if (audio.type == AudioVolumeHandler.AudioType.Environment)
            {
                environment.Add(source);
            }
            if (audio.type == AudioVolumeHandler.AudioType.Voices)
            {
                voice.Add(source);
            }
            if (audio.type == AudioVolumeHandler.AudioType.Music)
            {
                music.Add(source);
            }
            if (audio.type == AudioVolumeHandler.AudioType.Effects)
            {
                effects.Add(source);
            }
        }
    }

    public void SetMasterVolume()
    {
        SetEnvironmentalVolume();
        SetMusicVolume();
        SetVoiceVolume();
        SetEffectsVolume();
    }
    public void SetEnvironmentalVolume()
    {
        if (environment.Count == 0)
        {
            return;
        }

        foreach (AudioSource source in environment)
        {
            source.volume = EnvironmentalVolume * MasterVolume;
        }
    }
    public void SetVoiceVolume()
    {
        if (voice.Count == 0)
        {
            return;
        }

        foreach (AudioSource source in voice)
        {
            source.volume = VoiceVolume * MasterVolume * 2;
        }
    }
    public void SetMusicVolume()
    {
        if(music.Count == 0)
        {
            return;
        }

        foreach (AudioSource source in music)
        {
            source.volume = MusicVolume * MasterVolume;
        }
    }
    public void SetEffectsVolume()
    {
        if (effects.Count == 0)
        {
            return;
        }

        foreach (AudioSource source in effects)
        {
            source.volume = EffectsVolume * MasterVolume;
        }
    }

}