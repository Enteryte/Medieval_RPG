using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance;

    [Header("Audio")] public Slider masterSlider;
    public Slider environmentSlider;
    public Slider voiceSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public TMP_Text masterSliderTxt;
    public TMP_Text environmentSliderTxt;
    public TMP_Text voiceSliderTxt;
    public TMP_Text musicSliderTxt;
    public TMP_Text sfxSliderTxt;

    [Header("Video")] public Toggle windowModeToggle;
    public TMP_Dropdown resolutionDropdown;
    public Toggle subtitleToggle;

    [Header("Controls")] public Slider cameraSensiSlider;
    public Slider mouseSensiSlider;
    public Toggle controllerToggle;

    public TMP_Text cameraSensiSliderTxt;
    public TMP_Text mouseSensiSliderTxt;

    public TMP_Text[] keyTxts;

    public void Awake()
    {
        instance = this;
    }

    #region Slider: OnValueChange()

    public void MasterSliderOnValueChange()
    {
        masterSliderTxt.text = ((int)(masterSlider.value * 100)).ToString() + " / 100";
        AudioManager.Instance.SetMasterVolume = masterSlider.value;
    }

    public void EnvironmentSliderOnValueChange()
    {
        environmentSliderTxt.text = ((int)(environmentSlider.value * 100)).ToString() + " / 100";
        AudioManager.Instance.SetEnvironmentalVolume = environmentSlider.value;
    }

    public void VoiceSliderOnValueChange()
    {
        voiceSliderTxt.text = ((int)(voiceSlider.value * 100)).ToString() + " / 100";
        AudioManager.Instance.SetVoiceVolume = voiceSlider.value;
    }

    public void MusicSliderOnValueChange()
    {
        musicSliderTxt.text = ((int)(musicSlider.value * 100)).ToString() + " / 100";
        AudioManager.Instance.SetMusicVolume = musicSlider.value;
    }

    public void SFXSliderOnValueChange()
    {
        sfxSliderTxt.text = ((int)(sfxSlider.value * 100)).ToString() + " / 100";
        AudioManager.Instance.SetSFXVolume = sfxSlider.value;
    }

    public void CameraSensiSliderOnValueChange()
    {
        cameraSensiSliderTxt.text = ((int)(cameraSensiSlider.value * 100)).ToString() + " / 100";
    }

    public void MouseSensiSliderOnValueChange()
    {
        mouseSensiSliderTxt.text = ((int)(mouseSensiSlider.value * 100)).ToString() + " / 100";
    }

    public void ResolutionUpdate()
    {
        int screenWidth = 0;
        int screenHeight = 0;
        switch (resolutionDropdown.value)
        {
            case 0:
                screenWidth = 1900;
                screenHeight = 1080;
                break;
            case 1:
                screenWidth = 1680;
                screenHeight = 1050;
                break;
            case 2:
                screenWidth = 1280;
                screenHeight = 1024;
                break;
            case 3:
                screenWidth = 1280;
                screenHeight = 960;
                break;

        }

        Screen.SetResolution(screenWidth, screenHeight, windowModeToggle.isOn);
    }

    #endregion
}