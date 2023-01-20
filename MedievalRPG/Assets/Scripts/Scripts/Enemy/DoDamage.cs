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
        if (isActive != true || !other.gameObject.CompareTag("Enemy")) return;
        if (other.TryGetComponent(out EnemyHealth eHealth))
        {
            if (lightAttack)
                eHealth.LightDamage(lightDamage);
            if (heavyAttack)
                eHealth.HeavyDamage(heavyDamage);
        }

    }
}
