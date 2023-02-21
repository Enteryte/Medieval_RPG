using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDamager : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float debuffTime;
    [SerializeField] private bool resetDebuffTImer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerValueManager.instance.CurrHP -= damage;
            DebuffManager.instance.SlowPlayer(debuffTime, resetDebuffTImer);
        }
    }
}
