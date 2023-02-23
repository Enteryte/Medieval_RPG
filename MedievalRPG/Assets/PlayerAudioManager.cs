using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip[] atmer;
    [SerializeField] private AudioClip[] rolle;
    [SerializeField] private AudioClip[] blockHit;
    [SerializeField] private AudioClip[] gotHit;

    public void Roll()
    {
        source.clip = rolle[Random.Range(0, rolle.Length - 1)];
        source.Play();
    }
    public void GotHit(bool block)
    {
        if(block)
        {
            source.clip = rolle[Random.Range(0, blockHit.Length - 1)];
        }
        else
        {
            source.clip = rolle[Random.Range(0, gotHit.Length - 1)];
        }
        source.Play();
    }
}
