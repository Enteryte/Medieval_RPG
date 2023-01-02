using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ArcherEnemyKI : BaseEnemyKI
{
    [SerializeField] private SO_KI_Stats KiStats;
    [SerializeField] private EnemyBaseProfile BaseStats;
    [SerializeField] private Animator Animator;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private EnemyHealth Health;
    [SerializeField] private Transform FiringPoint;
    [SerializeField] private float FleeingDistance;
    [SerializeField] private float FleeReroutCoolDown;
    [SerializeField] private Transform SightContainer;
    [SerializeField] private float Tolerance;

    private ArrowPool ArrowPool;
    private bool IsInitialized;
    private bool IsSeeingPlayer;
    private bool WasSeeingPlayer;
    private bool HasSeenPlayer;
    private bool IsAttackCoroutineStarted;
    private bool IsFleeCoroutineStarted;

    private bool HasDied;
    private Vector3 StartPos;
    private float SqrTolerance;
    private Transform Target;

    //The Transforms for the Detectors, must be an amount divisible by 2
    private RayDetection[] RayDetectorsSight;
    [SerializeField] private RayDetection RayDetectionPrefab;
    [SerializeField] private Transform HardCodeTarget;


    public void Initialize(ArrowPool _arrowPool)
    {
        ArrowPool = _arrowPool;
        StartPos = transform.position;
        SqrTolerance = (float) Math.Pow(Tolerance, 2f);
        // ReSharper disable once StringLiteralTypo
        Animator.SetBool(Animator.StringToHash("IsKnockDownable"), KiStats.IsKnockDownable);
        RayDetectorsSight = SetDetectors(KiStats.SightDetectorCountHalf, SightContainer, KiStats.DetectionFOV,
            KiStats.DetectionRange, Color.cyan);
        Health.InitializeArcher(BaseStats, Animator, this);
        if (HardCodeTarget)
            Target = HardCodeTarget;

        IsInitialized = true;
    }

    void Update()
    {
        if (HasDied || !IsInitialized)
            return;
        Animator.SetBool(Animator.StringToHash("IsMoving"), (Agent.velocity.sqrMagnitude > SqrTolerance));

        IsSeeingPlayer = DetectorCheck(RayDetectorsSight);
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
            OnSightLost();
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
        while (IsSeeingPlayer)
        {
            Flee();
            yield return new WaitForSeconds(KiStats.AttackCoolDown);
        }

        IsFleeCoroutineStarted = false;
    }


    private bool DetectorCheck(RayDetection[] _detectors)
    {
        int checkValue = 0;
        for (int i = 0; i < _detectors.Length; i++)
            checkValue += _detectors[i].Sight() ? 1 : 0;
        // Debug.Log(CheckValue);
        return (checkValue > 0);
    }

    public void FireArrow()
    {
        ArrowPool.SpawnAndFireArrow(FiringPoint);
    }

    public void Death()
    {
        HasDied = true;
        Animator.SetBool(Animator.StringToHash("IsDead"), true);
    }

    /// <summary>
    /// The Function that appoints the Detectors into their Position according to the Field of View
    /// </summary>
    /// <param name="_halvedCount">50% of the amount of Detectors you want the enemy to have. Needs to be doubled.</param>
    /// <param name="_container">What parent element the Detectors are subordinate to in the scene.</param>
    /// <param name="_fov">The Field of View for the set of Detectors</param>
    /// <param name="_range"></param>
    /// <param name="_gizmoColor"></param>
    /// <returns>A ray Detection array, as otherwise it doesn't actually store the detectors.</returns>
    private RayDetection[] SetDetectors(int _halvedCount, Transform _container, float _fov, float _range,
        Color _gizmoColor)
    {
        float detectorCount = _halvedCount * 2f;
        float angleSteps = (_fov / detectorCount) / 2f;
        RayDetection[] detectors = new RayDetection[(int) detectorCount];
        for (int i = 0; i < detectors.Length; i += 2)
        {
            Quaternion lRot = Quaternion.AngleAxis(-angleSteps * (i + 1), Vector3.up);
            Quaternion rRot = Quaternion.AngleAxis(angleSteps * (i + 1), Vector3.up);
            Vector3 containerPosition = _container.position;
            detectors[i] = Instantiate(RayDetectionPrefab, containerPosition, lRot, _container);
            detectors[i].Initialize(_range, _gizmoColor, HardCodeTarget.gameObject);
            detectors[i + 1] = Instantiate(RayDetectionPrefab, containerPosition, rRot, _container);
            detectors[i + 1].Initialize(_range, _gizmoColor, HardCodeTarget.gameObject);
        }

        return detectors;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, FleeingDistance);
        
        VisualizeDetectors(Color.cyan, KiStats.DetectionRange, SightContainer);
    }
    
    private void VisualizeDetectors(Color _lineColor, float _range, Transform _container)
    {
        Gizmos.color = _lineColor;
        Transform t = _container.transform;
        Vector3 position = t.position;
        Gizmos.DrawLine(position, t.forward * _range + position);
    }
}