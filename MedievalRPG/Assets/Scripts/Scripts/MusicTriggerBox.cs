using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerBox : MonoBehaviour
{
    public AudioClip afterEnteredMusicClip; // New Music
    public AudioClip beforeEnteredMusicClip; // Old Music

    public AudioSource musicAudioSource;

    public IEnumerator FadeOldMusicOut()
    {
        float currentTime = 0;
        float start = musicAudioSource.volume;

        while (currentTime < 1.5f)
        {
            currentTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(start, 0, currentTime / 1.5f);

            yield return null;
        }

        if (musicAudioSource.clip == afterEnteredMusicClip)
        {
            musicAudioSource.clip = beforeEnteredMusicClip;
            musicAudioSource.Play();
        }
        else
        {
            musicAudioSource.clip = afterEnteredMusicClip;
            musicAudioSource.Play();
        }

        StartCoroutine(FadeNewMusicIn());

        yield break;
    }

    public IEnumerator FadeNewMusicIn()
    {
        float currentTime = 0;
        float start = 0;

        while (currentTime < 1.5f)
        {
            currentTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(start, OptionManager.instance.musicSlider.value, currentTime / 1.5f);

            yield return null;
        }

        yield break;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO)
        {
            if (!FightManager.instance.isInFight)
            {
                StartCoroutine(FadeOldMusicOut());
            }
            else
            {
                if (musicAudioSource.clip == afterEnteredMusicClip)
                {
                    FightManager.lastMusicClip = beforeEnteredMusicClip;
                }
                else
                {
                    FightManager.lastMusicClip = afterEnteredMusicClip;
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO)
        {
            if (!FightManager.instance.isInFight)
            {
                StartCoroutine(FadeNewMusicIn());
            }
            else
            {
                if (musicAudioSource.clip == afterEnteredMusicClip)
                {
                    FightManager.lastMusicClip = beforeEnteredMusicClip;
                }
                else
                {
                    FightManager.lastMusicClip = afterEnteredMusicClip;
                }
            }
        }
    }
}
