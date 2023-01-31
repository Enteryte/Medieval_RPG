using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ArcherEnemyKI : BaseEnemyKI
{
    [SerializeField] private Transform FiringPoint;
    [SerializeField] private float FleeingDistance;
    [SerializeField] private float FleeReroutCoolDown;

    private ArrowPool ArrowPool;

    private bool IsAttackCoroutineStarted;
    private bool IsFleeCoroutineStarted;

    public void LinkArrowPool(ArrowPool _arrowPool)
    {
        ArrowPool = _arrowPool;
    }
    public override void Init()
    {
        base.Init();
        if (HardCodeTarget)
            Target = HardCodeTarget.transform;

        IsInitialized = true;
    }

    protected override void Update()
    {
        base.Update();
        CompareSight();
        if (!IsSeeingPlayer)
            return;
        if (IsFleeCoroutineStarted) return;
        if (Vector3.Distance(transform.position, Target.position) < FleeingDistance)
            StartCoroutine(FleeCheck());
        else
            Animator.SetBool(Animator.StringToHash("IsFleeing"), false);
        if (!IsAttackCoroutineStarted)
            StartCoroutine(AttackTrigger());
    }

    private void CompareSight()
    {
        //Checks if there was a state change
        if (IsSeeingPlayer == WasSeeingPlayer)
            return;

        if (IsSeeingPlayer) //If the current state was Not Seeing -> Seeing
            OnSightGained();
        else //If the current state was Seeing -> Not seeing
            // OnSightLost();
        WasSeeingPlayer = IsSeeingPlayer;
    }

    private void OnSightGained()
    {
        if (!HasSeenPlayer)
            NoticeEnemy();
    }

    private void OnSightLost()
    {
        StartCoroutine(Return());
    }

    private IEnumerator Return()
    {
        float time = 0f;
        while (time < KiStats.Patience)
        {
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
            if (IsSeeingPlayer)
                break;
        }

        if (!IsSeeingPlayer)
            Agent.SetDestination(StartPos);
        transform.LookAt(Target.transform.position);
    }

    private void NoticeEnemy()
    {
        HasSeenPlayer = true;
        Animator.SetTrigger(Animator.StringToHash("NoticedYou"));
        Animator.SetBool(Animator.StringToHash("KnowsAboutYou"), true);
    }

    private void Flee()
    {
        Vector3 flightPos = transform.position - Target.transform.position;
        // StartPos = Vector3.Lerp(flightPos, Target.transform.position, 0.1f);

        Animator.SetBool(Animator.StringToHash("IsFleeing"), true);
        Agent.SetDestination(flightPos);
    }

    private IEnumerator AttackTrigger()
    {
        IsAttackCoroutineStarted = true;
        while (IsSeeingPlayer)
        {
            Animator.SetTrigger(Animator.StringToHash("AttackLaunch"));
            yield return new WaitForSeconds(KiStats.AttackCoolDown);
        }

        IsAttackCoroutineStarted = false;
    }

    private IEnumerator FleeCheck()
    {
        IsFleeCoroutineStarted = true;
        while (Vector3.Distance(transform.position, Target.transform.position) < FleeingDistance)
        {
            Flee();
            yield return new WaitForSeconds(KiStats.AttackCoolDown);
        }

        IsFleeCoroutineStarted = false;
    }



    public void FireArrow()
    {
        ArrowPool.SpawnAndFireArrow(FiringPoint);
    }

    public override void Death()
    {
        base.Death();
        
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, FleeingDistance);
        
    }
    
}