using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    [SerializeField]
    private GameObject flames;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            GameObject Flame = Instantiate(flames);
            Flame.transform.position = this.gameObject.transform.position;
            Flame.transform.localScale = new Vector3(2, 2, 2);
            Flame.transform.parent = other.transform;

            //if (other.gameObject.CompareTag("BossHitbox"))
            //{
            //    if (lightAttack == true)
            //    {
            //        other.gameObject.GetComponent<SkeletonBossStats>().CurrentHP -= lightDamage;
            //    }
            //    else
            //    {
            //        other.gameObject.GetComponent<SkeletonBossStats>().CurrentHP -= heavyDamage;
            //    }
            //}

            //if (other.TryGetComponent(out EnemyHealth eHealth))
            //{
            //    if (lightAttack)
            //        eHealth.LightDamage(lightDamage);
            //    if (heavyAttack)
            //        eHealth.HeavyDamage(heavyDamage);
            //}
        }
    }
}
