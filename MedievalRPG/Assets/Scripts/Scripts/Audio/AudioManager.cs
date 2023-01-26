using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public float SetMasterVolume { private get; set; }
    private float EnvironmentalVolume;
    public float GetEnvironmentalVolume => EnvironmentalVolume * SetMasterVolume;
    public float SetEnvironmentalVolume
    {
        set => EnvironmentalVolume =(value >= 1.0f) ? 1.0f : value;
    }
    private float VoiceVolume;
    public float GetVoiceVolume => VoiceVolume * SetMasterVolume;
    public float SetVoiceVolume
    {
        set => VoiceVolume = (value >= 1.0f) ? 1.0f : value;
    }
    
    private float MusicVolume;
    public float GetMusicVolume => MusicVolume * SetMasterVolume;
    public float SetMusicVolume
    {
        set => MusicVolume = (value >= 1.0f) ? 1.0f : value;
    }
    private float SFXVolume;
    public float GetSFXVolume => SFXVolume * SetMasterVolume;
    public float SetSFXVolume
    {
        set => SFXVolume = (value >= 1.0f) ? 1.0f : value;
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
}
