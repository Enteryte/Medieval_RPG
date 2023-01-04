using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float damage = 1; //Damage per Tick
    public int tickTime = 1; //Time between damage ticks
    public float duration = 10; //Time Befor Fire Extinguishes

    private EnemyHealth hp = null;

    private void Start()
    {
        if(transform.parent.gameObject.GetComponent<EnemyHealth>() != null)
        {
            hp = transform.parent.gameObject.GetComponent<EnemyHealth>();

            StartCoroutine(DoDamage());
        }
        StartCoroutine(BUUUURN());
    }

    IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(tickTime);
        hp.LightDamage(damage);

        StartCoroutine(DoDamage());
    }

    IEnumerator BUUUURN()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

}
