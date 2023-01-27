using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int damage = 10;

    private void Start()
    {
        StartCoroutine(TriggerEvents());
    }

    IEnumerator TriggerEvents()
    {
        this.gameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(TriggerEvents());
            Destroy(this.GetComponent<Rigidbody>());
            this.transform.SetParent(other.gameObject.transform);

            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyHealth>().LightDamage(damage);
            }
            if(other.gameObject.CompareTag("SkeletonBoss"))
            {
                StartCoroutine(other.gameObject.GetComponent<SkeletonBossKI>().CountArrows());
            }
        }
    }
}
