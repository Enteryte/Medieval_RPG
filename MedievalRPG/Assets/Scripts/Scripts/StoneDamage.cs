using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDamage : MonoBehaviour
{
    public int stoneDamage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().HeavyDamage(stoneDamage);
        }
        Destroy(this);
    }
}
