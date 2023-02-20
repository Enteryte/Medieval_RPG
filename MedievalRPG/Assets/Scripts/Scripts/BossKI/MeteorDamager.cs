using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDamager : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerValueManager.instance.CurrHP -= damage;
        }
    }
}
