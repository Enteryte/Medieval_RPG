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
        if (isActive != true) return;

        if(other.gameObject.CompareTag("Destructable"))
        {
            other.gameObject.GetComponent<Animator>().enabled = true;
            other.gameObject.GetComponent<AudioSource>().Play();
        }

        if(other.gameObject.CompareTag("BossHitbox"))
        {
            if(lightAttack == true)
            {
                other.gameObject.GetComponent<SkeletonBossStats>().CurrentHP -= lightDamage;
            }
            else
            {
                other.gameObject.GetComponent<SkeletonBossStats>().CurrentHP -= heavyDamage;
            }
        }

        if (other.gameObject.CompareTag("Enemy") && other.TryGetComponent(out EnemyHealth eHealth))
        {
            if (lightAttack)
                eHealth.LightDamage(lightDamage);
            if (heavyAttack)
                eHealth.HeavyDamage(heavyDamage);
        }

    }
}
