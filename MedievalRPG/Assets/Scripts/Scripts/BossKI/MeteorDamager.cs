using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDamager : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float debuffTime;
    [SerializeField] private bool resetDebuffTImer;
    [SerializeField] private AudioSource source;

    private void Start()
    {
        source.volume = AudioManager.Instance.EffectsVolume * AudioManager.Instance.MasterVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerValueManager.instance.CurrHP -= damage;

            if (DebuffManager.instance.slowPlayerCoro == null)
            {
                DebuffManager.instance.SlowPlayer(debuffTime, resetDebuffTImer);
            }
        }
    }
}
