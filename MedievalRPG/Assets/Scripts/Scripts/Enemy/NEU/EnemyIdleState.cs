using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyStateManager TEST;

    public Animator animator;

    public EnemyChaseState chaseState;
    public bool canSeePlayer;

    public override EnemyState RunCurrentState()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, TEST.viewRadius, TEST.targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform interactableObj = targetsInViewRadius[i].transform;

            Vector3 dirToObj = (interactableObj.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToObj) < TEST.viewAngle / 2)
            {
                float distanceToObj = Vector3.Distance(transform.position, interactableObj.position);

                if (!Physics.Raycast(transform.position, dirToObj, distanceToObj, TEST.obstacleMask) && targetsInViewRadius[i].gameObject == TEST.testPlayer)
                {
                    canSeePlayer = true;

                    animator.SetBool("CanSeePlayer", true);

                    break;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
        }

        if (canSeePlayer)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }
}
