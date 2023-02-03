using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamagerDelegator : MonoBehaviour
{
    [SerializeField] private EnemyDamager EnemyDamager;

    public NavMeshAgent navmeshAgent;

    public void DamageOn()
    {
        //navmeshAgent.isStopped = true;

        EnemyDamager.DamageOn();
    }

    public void DamageOff()
    {
        //navmeshAgent.isStopped = false;

        EnemyDamager.DamageOff();
    }
}
