using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamagerDelegator : MonoBehaviour
{
    [SerializeField] private EnemyDamager EnemyDamager;

    private MeleeEnemyKi ParentKI;
    public NavMeshAgent navmeshAgent;

    private void Start()
    {
        ParentKI = GetComponentInParent<MeleeEnemyKi>();
    }

    private void LeaveRecoil()
    {
        ParentKI.RestartAgent();
    }
    private void DisableAnimator()
    {
        ParentKI.DisableAnimator();
        StartCoroutine(DespawnEnemy());
    }

    private IEnumerator DespawnEnemy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(transform.parent.gameObject);
    }
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
