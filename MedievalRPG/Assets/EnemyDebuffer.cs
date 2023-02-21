using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuffer : MonoBehaviour
{
    [SerializeField] private int debuffType;
    [SerializeField] private float amount;
    [SerializeField] private float debuffTime;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(debuffType == 0)
            {
                DebuffManager.instance.LowerStrength(amount, debuffTime, true);
            }
            if (debuffType == 1)
            {
                DebuffManager.instance.Bleeding(debuffTime, Mathf.RoundToInt(amount), true);
            }
        }
    }
}
