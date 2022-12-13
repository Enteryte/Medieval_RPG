using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Generic_Enemy_KI : MonoBehaviour
{
    [Header("Includes")] [SerializeField] private SO_KI_Stats KiStats;
    [SerializeField] private EnemyBaseProfile BaseStats;
    [SerializeField] private Animator Animator;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private EnemyHealth Health;
    [SerializeField] private EnemyDamager EnemyDamager;
    [SerializeField] private GameObject HardCodeTarget;

    private bool IsSeeingPlayer;
    private bool HasSeenPlayer;
    private bool IsInAttackRange;

    private bool HasDied;

    private Vector3 RandomTarget;
    private Transform Target;
    private Vector3 StartPos;


    [Header("Detectors")] [SerializeField] private RayDetection RayDetectionPrefab;
    [SerializeField] private Transform SightContainer;

    [SerializeField] private Transform AttackContainer;

    //The Transforms for the Detectors, must be an amount divisible by 2
    private RayDetection[] RayDetectorsSight;

    //The Transforms for the Detectors, must be an amount divisible by 2
    private RayDetection[] RayDetectorsAttack;


    [Header("Dev Variables")]
    //How low the speed is to be considered not moving, just in case Navmesh doesn't do it's job stopping
    [SerializeField]
    private float Tolerance;
    private float SqrTolerance;


    private int CheckValue;
    private bool IsAttackCoroutineStarted;
    private bool IsSearching;

    #region Unity Events

    void Start()
    {
        StartPos = transform.position;
        SqrTolerance = Tolerance * Tolerance;
        Animator.SetBool(Animator.StringToHash("IsKnockDownable"), KiStats.IsKnockDownable);
        RayDetectorsSight = SetDetectors(KiStats.SightDetectorCountHalf, SightContainer, KiStats.DetectionFOV,
            KiStats.DetectionRange, Color.cyan);
        RayDetectorsAttack = SetDetectors(KiStats.AttackDetectorCountHalf, AttackContainer, KiStats.AttackRangeFOV,
            KiStats.AttackRange, Color.red);
        Health.Initialize(BaseStats, Animator, this);
        EnemyDamager.Init(BaseStats.normalDamage);
    }

    void Update()
    {
        if (HasDied)
            return;

        IsSeeingPlayer = DetectorCheck(RayDetectorsSight);
        //TODO: If IsSeeingPlayer went from Positive to negative, put the OnSightLost Event here.
        SightEvent(IsSeeingPlayer);

        // Debug.Log(Agent.velocity.sqrMagnitude);
        Debug.Log(Agent.velocity.sqrMagnitude > SqrTolerance);
        Animator.SetBool(Animator.StringToHash("IsMoving"), (Agent.velocity.sqrMagnitude > SqrTolerance));

        //Putting the Attack Detection into an if so it only checks when it has the player within it's sight for better performance.
        if (!IsSeeingPlayer) return;
        IsSearching = false;
        IsInAttackRange = DetectorCheck(RayDetectorsAttack);
        Animator.SetBool(Animator.StringToHash("IsInsideAttackRange"), IsInAttackRange);
    }

    private bool DetectorCheck(RayDetection[] _detectors)
    {
        CheckValue = 0;
        for (int i = 0; i < _detectors.Length; i++)
            CheckValue += _detectors[i].Sight() ? 1 : 0;
        // Debug.Log(CheckValue);
        return (CheckValue > 0);
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
            case false when _isSeeingPlayer:
                NoticeEnemy();
                break;
            case true when _isSeeingPlayer:
                CheckAttackPossible();
                break;
            case true when !_isSeeingPlayer:
                Search();
                break;
        }
    }

    #region NoticingBehaviour

    private void NoticeEnemy()
    {
        //ToDo: Remove this temporary check when in the game.
        if (!HardCodeTarget)
            Target = GameManager.instance.playerGO.transform;
        else
            Target = HardCodeTarget.transform;
        HasSeenPlayer = true;
        Animator.SetTrigger(Animator.StringToHash("NoticedYou"));
        Animator.SetBool(Animator.StringToHash("KnowsAboutYou"), true);
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
            //Move in the direction of the player
            if (Agent.hasPath || IsInAttackRange) return;
            Agent.isStopped = false;
            Agent.SetDestination(Target.position);
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

    public void Death()
    {
        //TODO: Turn of all other scripts, animators, etc. and turn the enemy into a ragdoll
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
            detectors[i].Initialize(_range, _gizmoColor, HardCodeTarget);
            detectors[i + 1] = Instantiate(RayDetectionPrefab, containerPosition, rRot, _container);
            detectors[i + 1].Initialize(_range, _gizmoColor, HardCodeTarget);
        }

        return detectors;
    }

    #region EditorDepictors

    private void OnDrawGizmos()
    {
        VisualizeDetectors(Color.cyan, KiStats.DetectionRange, SightContainer, RayDetectorsSight);
        VisualizeDetectors(Color.red, KiStats.AttackRange, AttackContainer, RayDetectorsAttack);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(StartPos, new Vector3(KiStats.PatrollingRange * 2f, 0, KiStats.PatrollingRange * 2f));
    }

    private void VisualizeDetectors(Color _lineColor, float _range, Transform _container, RayDetection[] _detectors)
    {
        Gizmos.color = _lineColor;
        Transform t = _container.transform;
        Gizmos.DrawLine(t.position, t.forward * _range + t.position);
    }

    #endregion
}