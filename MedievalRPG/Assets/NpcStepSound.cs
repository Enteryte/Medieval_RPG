using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource source;

    private void PlaySound()
    {
        source.clip = clips[Random.Range(0, clips.Length - 1)];
        source.Play();
    }
}
