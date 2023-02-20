using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeHandler : MonoBehaviour
{
    private AudioSource source;

    public AudioType type;
    
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    [HideInInspector]
    public enum AudioType
    {
        Environment,
        Voices,
        Music,
        Effects,
    }
}
