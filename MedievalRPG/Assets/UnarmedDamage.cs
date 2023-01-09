using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnarmedDamage : MonoBehaviour
{
    public int lightDamage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out EnemyHealth eHealth))
            {
                eHealth.LightDamage(lightDamage);
            }
        }
    }
}
