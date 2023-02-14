using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioData data;

    public float SetMasterVolume { private get; set; } = 1.0f;
    private float EnvironmentalVolume = 0.5f;
    public float GetEnvironmentalVolume => EnvironmentalVolume * SetMasterVolume;

    public float SetEnvironmentalVolume
    {
        set => EnvironmentalVolume = Mathf.Min(1.0f, value);
        //data.
    }

    private float VoiceVolume = 0.5f;
    public float GetVoiceVolume => VoiceVolume * SetMasterVolume;

    public float SetVoiceVolume
    {
        set => VoiceVolume = Mathf.Min(1.0f, value);
    }

    private float MusicVolume = 0.5f;
    public float GetMusicVolume => MusicVolume * SetMasterVolume;

    public float SetMusicVolume
    {
        set => MusicVolume = Mathf.Min(1.0f, value);
    }

    private float SFXVolume = 0.5f;
    public float GetSFXVolume => SFXVolume * SetMasterVolume;

    public float SetSFXVolume
    {
        set => SFXVolume = Mathf.Min(1.0f, value);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
}