using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Explosives : MonoBehaviour
{
    public ItemBaseProfile iBP;

    public void OnTriggerEnter(Collider other)
    {
        // WIP: Hier muss noch die HP des gestroffenen Gegners rein.

        //DealDamage();

        Destroy(this.gameObject);
    }

    //public void DealDamage(float valueToDealDamageTo)
    //{
    //    valueToDealDamageTo -= iBP.normalDamage;
    //}
}
