using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ArcherEnemyKI : BaseEnemyKI
{
    [SerializeField] private Transform FiringPoint;
    [SerializeField] private float PersonalSpace;
    [SerializeField] private float RunningLength;
    [SerializeField] private float FleeReroutCoolDown;


    private Vector3 FlightTargetPos;

    private ArrowPool ArrowPool;

    private bool IsAttackCoroutineStarted;
    private bool IsFleeCoroutineStarted;
    private bool HasTakenAStep;

    public void LinkArrowPool(ArrowPool _arrowPool)
    {
        ArrowPool = _arrowPool;
    }

    public override void Init()
    {
        base.Init();
        IsInitialized = true;
    }

    protected override void Update()
    {
        base.Update();

        CompareSight();
        if (!IsSeeingPlayer || IsFleeCoroutineStarted)
            return;
        if (IsAttackCoroutineStarted)
        {
            // Vector3 targetHorizontal = new Vector3(Target.position.x, 0.0f, Target.position.z);
            transform.LookAt(Target.position);
        }
        if (Vector3.Distance(transform.position, Target.position) < PersonalSpace)
            StartCoroutine(FleeCheck());
        else
            Animator.SetBool(Animator.StringToHash("IsFleeing"), false);
        if (!IsAttackCoroutineStarted)
        {
            StartCoroutine(AttackTrigger());
        }
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
    }

    private void NoticeEnemy()
    {
        HasSeenPlayer = true;
        Animator.SetTrigger(Animator.StringToHash("NoticedYou"));
        Animator.SetBool(Animator.StringToHash("KnowsAboutYou"), true);
    }

    private void Flee()
    {
        Vector3 flightDir = transform.position - Target.transform.position;
        flightDir = flightDir.normalized;
        flightDir *= RunningLength;
        FlightTargetPos = Target.position + flightDir;
        if (NavMesh.SamplePosition(FlightTargetPos, out NavMeshHit hit, RunningLength, NavMesh.AllAreas))
        {
            Animator.SetBool(Animator.StringToHash("IsFleeing"), true);
            Agent.SetDestination(hit.position);
        }
        else
        {
            bool isHit = false;
            float randomizerValue = 1.0f;
            while (!isHit)
            {
                Vector3 randomMod = new Vector3(Random.Range(-randomizerValue, randomizerValue),
                    0.0f,
                    Random.Range(-randomizerValue, randomizerValue));
                FlightTargetPos += randomMod;
                if (NavMesh.SamplePosition(FlightTargetPos, out hit, RunningLength, NavMesh.AllAreas))
                {
                    isHit = true;
                    Agent.SetDestination(hit.position);
                }

                randomizerValue += 0.2f;
            }
        }
    }

    private IEnumerator AttackTrigger()
    {
        IsAttackCoroutineStarted = true;
        Agent.enabled = false;
        while (!IsFleeCoroutineStarted)
        {
            Animator.SetTrigger(Animator.StringToHash("AttackLaunch"));
            yield return new WaitForSeconds(KiStats.AttackCoolDown);
        }
        IsAttackCoroutineStarted = false;
        Agent.enabled = true;
    }

    private IEnumerator FleeCheck()
    {
        Agent.enabled = true;
        IsFleeCoroutineStarted = true;
        while (Vector3.Distance(transform.position, Target.transform.position) < PersonalSpace)
        {
            Flee();
            yield return new WaitForSeconds(FleeReroutCoolDown);
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
        Gizmos.DrawWireSphere(transform.position, PersonalSpace);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(FlightTargetPos, Vector3.one);
    }
}