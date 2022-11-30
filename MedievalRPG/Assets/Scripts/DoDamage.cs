using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField]
    private float lightDamage;
    [SerializeField]
    private float heavyDamage;

    public bool isActive = false;
    public bool lightAttack = false;
    public bool heavyAttack = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isActive == true && other.gameObject.CompareTag("Enemy"))
        {
            if(lightAttack)
            { 
                //reference script and do damage
            }
            if (heavyAttack)
            {
                //reference script and do damage
            }
        }
    }
}
