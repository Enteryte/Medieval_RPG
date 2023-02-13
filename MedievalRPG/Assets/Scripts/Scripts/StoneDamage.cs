using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDamage : MonoBehaviour
{
    public int stoneDamage = 10;

    public bool dontDoDmg = false;

    public float timeTillOff = 5;
    public float currTime;

    public void Update()
    {
        if (currTime < timeTillOff)
        {
            currTime += Time.deltaTime;

            if (currTime >= timeTillOff)
            {
                this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!dontDoDmg)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyHealth>().HeavyDamage(stoneDamage);
            }
            if (collision.gameObject.CompareTag("BossHitbox"))
            {
                collision.gameObject.GetComponent<SkeletonBossStats>().CurrentHP -= stoneDamage;
            }
        }

        dontDoDmg = true;
    }
}
