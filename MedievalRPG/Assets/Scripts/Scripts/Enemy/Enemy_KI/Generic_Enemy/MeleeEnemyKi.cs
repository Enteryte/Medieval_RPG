using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class MeleeEnemyKi : BaseEnemyKI
{
    // ReSharper disable once IdentifierTypo
    [SerializeField] private EnemyDamager EnemyDamager;

    private bool IsInAttackRange;


    private Vector3 RandomTarget;

    [SerializeField] private Transform AttackContainer;

    //The Transforms for the Detectors, must be an amount divisible by 2
    private RayDetection[] RayDetectorsAttack;

    [Header("Dev Variables")] private bool IsAttackCoroutineStarted;
    private bool IsSearching;
    [SerializeField] private float RepathCoolDownLength = 0.2f;
    private float RepathCoolDown = 0.2f;

    [SerializeField] private float MinDistance = 1.0f;

    #region Unity Events

    public override void Init()
    {
        base.Init();
        EnemyDamager.Init(BaseStats.normalDamage);
        RayDetectorsAttack = SetDetectors(KiStats.AttackDetectorCountHalf, AttackContainer, KiStats.AttackRangeFOV,
            KiStats.AttackRange, Color.red);
        if (GameManager.instance)
            GameManager.instance.allMeleeEnemies.Add(this);
        IsInitialized = true;
    }

    protected override void Update()
    {
        base.Update();
        SightEvent(IsSeeingPlayer);
        if (Vector3.Distance(transform.position, Target.position) < MinDistance)
        {
            
            if (Agent.enabled)
            {
                Agent.destination = transform.position;
                Agent.enabled = false;
            }

            transform.LookAt(Target);
        }
        else if (!Agent.enabled)
            Agent.enabled = true;

        //Putting the Attack Detection into an if so it only checks when it has the player within it's sight for better performance.
        if (!IsSeeingPlayer)
        {
            Animator.SetBool(Animator.StringToHash("IsInsideAttackRange"), false);

            if (FightManager.instance.enemiesInFight.Contains(this))
            {
                FightManager.instance.enemiesInFight.Remove(this);

                if (GameManager.instance.musicAudioSource.clip == FightManager.instance.fightMusic && FightManager.instance.enemiesInFight.Count <= 0)
                {
                    FightManager.instance.isInFight = false;

                    FightManager.instance.FadeOldMusicOut();
                }
            }

            return;
        }

        IsSearching = false;

        IsInAttackRange = DetectorCheck(RayDetectorsAttack);
        Animator.SetBool(Animator.StringToHash("IsInsideAttackRange"), IsInAttackRange);
    }

    #endregion

    private void SightEvent(bool _isSeeingPlayer)
    {
        switch (HasSeenPlayer)
        {
            case false when !_isSeeingPlayer:
                if (!KiStats.PatrolsWhileVibing || Agent.velocity.sqrMagnitude >= SqrTolerance)
                    break;
                StartCoroutine(TimeToLookAtNewRandomTarget());
                break;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            case false when _isSeeingPlayer:
                NoticeEnemy();
                break;
            case true when _isSeeingPlayer:
                CheckAttackPossible();
                break;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            case true when !_isSeeingPlayer:
                // Search();
                break;
        }
    }

    #region NoticingBehaviour

    private void NoticeEnemy()
    {
        //ToDo: Remove this temporary check when in the game.
        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        if (!HardCodeTarget)
            Target = GameManager.instance.playerGO.transform;
        else
            Target = HardCodeTarget.transform;
        HasSeenPlayer = true;
        Animator.SetTrigger(Animator.StringToHash("NoticedYou"));
        Animator.SetBool(Animator.StringToHash("KnowsAboutYou"), true);

        if (!FightManager.instance.enemiesInFight.Contains(this))
        {
            FightManager.instance.enemiesInFight.Add(this);

            if (GameManager.instance.musicAudioSource.clip != FightManager.instance.fightMusic)
            {
                FightManager.instance.isInFight = true;

                FightManager.instance.FadeOldMusicOut();
            }
        }
    }

    #endregion

    #region AttackingBehaviour

    private IEnumerator AttackTrigger()
    {
        while (IsInAttackRange)
        {
            IsAttackCoroutineStarted = true;
            Animator.SetTrigger(Animator.StringToHash("AttackLaunch"));
            yield return new WaitForSeconds(KiStats.AttackCoolDown);

            IsAttackCoroutineStarted = false;
        }
    }

    private void CheckAttackPossible()
    {
        //TODO: Maybe make the AI more complex by having it skirt around when on cooldown
        if (IsInAttackRange)
        {
            RepathCoolDown = 0f;
            if (Agent.enabled)
                if (!Agent.isStopped)
                {
                    Agent.isStopped = true;
                    Agent.ResetPath();
                }

            //Attack
            if (!IsAttackCoroutineStarted)
                StartCoroutine(AttackTrigger());
        }
        else
        {
            RepathCoolDown += Time.deltaTime;
            //Move in the direction of the player
            if (IsInAttackRange || RepathCoolDown < RepathCoolDownLength) return;
            RepathCoolDown = 0f;
            Agent.isStopped = false;
            var targetPos = Target.position;
            Agent.destination = targetPos;
        }
    }

    #endregion

    #region SearchingBehaviour

    private void Search()
    {
        if (Vector3.Distance(transform.position, StartPos) <= Tolerance && IsSearching)
        {
            if (!KiStats.PatrolsWhileVibing || Agent.velocity.sqrMagnitude != 0)
                return;
            StartCoroutine(TimeToLookAtNewRandomTarget());

            return;
        }
        //TODO: Do this over a OnLoseSight event instead of a continuous event 

        if (!IsSearching)
        {
            IsSearching = true;
            Vector3 placeToGo = Target.position;
            Agent.SetDestination(placeToGo);

            if (!FightManager.instance.enemiesInFight.Contains(this))
            {
                FightManager.instance.enemiesInFight.Add(this);

                if (GameManager.instance.musicAudioSource.clip != FightManager.instance.fightMusic)
                {
                    FightManager.instance.isInFight = true;

                    FightManager.instance.FadeOldMusicOut();
                }
            }
        }

        if (IsSearching && Agent.velocity.sqrMagnitude <= Tolerance)
            StartCoroutine(WaitUntilGivingUp());
    }

    private IEnumerator WaitUntilGivingUp()
    {
        float a = 0f;
        while (a <= KiStats.AttackCoolDown * 10f)
        {
            a += 1f;
            if (IsSeeingPlayer)
            {
                Agent.SetDestination(StartPos);
                break;
            }

            yield return new WaitForSeconds(0.1f);
            if (!IsSeeingPlayer) continue;
            Agent.SetDestination(StartPos);
            break;
        }
    }

    #endregion

    #region IdleBehaviour

    /// <summary>
    /// A vector3 that generates a random position on the map that the NPC can use as a target while he isn't chasing anything. Also checks the position on the mesh for improved navigation.
    /// </summary>
    /// <returns>A random Vector3 on the Mesh</returns>
    private Vector3 GenerateRandomTarget()
    {
        Vector3 myTarget = new Vector3(
            Random.Range(StartPos.x - KiStats.PatrollingRange, StartPos.x + KiStats.PatrollingRange), 0,
            Random.Range(StartPos.z - KiStats.PatrollingRange, StartPos.z + KiStats.PatrollingRange));
        if (NavMesh.SamplePosition(myTarget, out NavMeshHit hit, KiStats.PatrollingRange, NavMesh.AllAreas))
            myTarget = hit.position;
        else
            throw new Exception($"Couldn't Hit NavMesh at Target: {myTarget.x}, {myTarget.y}, {myTarget.z}");
        return myTarget;
    }

    /// <summary>
    /// A coroutine that just waits for how long the Patience of the NPC lasts before requesting a new random Target.
    /// </summary>
    private IEnumerator TimeToLookAtNewRandomTarget()
    {
        yield return new WaitForSeconds(KiStats.Patience);
        RandomTarget = GenerateRandomTarget();
        Agent.SetDestination(RandomTarget);
    }

    #endregion

    public override void Death()
    {
        base.Death();
    }

    #region EditorDepictors

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        VisualizeDetectors(Color.red, KiStats.AttackRange, AttackContainer);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(StartPos, new Vector3(KiStats.PatrollingRange * 2f, 0, KiStats.PatrollingRange * 2f));
    }

    #endregion
}