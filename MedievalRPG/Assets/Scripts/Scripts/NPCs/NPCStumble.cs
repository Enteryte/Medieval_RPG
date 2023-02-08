using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCStumble : MonoBehaviour
{
    private Animator Anim;
    private AudioSource AudioSource;
    [SerializeField] private SO_NPCProfanityDex MyProfanities;
    private int LayerIndex;
    private bool IsRockBottom;

    private void OnTriggerEnter(Collider _other)
    {
        if(IsRockBottom)
            return;
        Vector3 relativePlayerPos = transform.InverseTransformPoint(_other.transform.position);
        Vector3 playerVelocity = _other.GetComponent<CharacterController>().velocity;
        
        Vector3 stumbleForce = relativePlayerPos * playerVelocity.magnitude;
        Debug.Log(stumbleForce);
        
        SetWeightZero();
        Anim.SetFloat(Animator.StringToHash("ColX"), stumbleForce.x);
        Anim.SetFloat(Animator.StringToHash("ColY"), stumbleForce.z);
        Anim.SetTrigger(Animator.StringToHash("Stumble"));

        AudioSource.clip = MyProfanities.Insults[Random.Range(0, MyProfanities.Insults.Length-1)];
        AudioSource.Play();

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        LayerIndex = Anim.GetLayerIndex("Bottom Layer");
        AudioSource.volume = AudioManager.Instance.GetVoiceVolume;
    }

    private void SetWeightZero()
    {
        Anim.SetLayerWeight(LayerIndex, 0.0f);
        IsRockBottom = true;
    }
    
    private void SetWeightOne()
    {
        Anim.SetLayerWeight(LayerIndex, 1.0f);
        IsRockBottom = false;
    }
    
}
