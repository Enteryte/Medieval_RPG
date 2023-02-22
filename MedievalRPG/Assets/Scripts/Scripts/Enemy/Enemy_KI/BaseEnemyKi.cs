using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class BaseEnemyKI : MonoBehaviour
{
    [Header("Includes")] [SerializeField] protected SO_KI_Stats KiStats;
    [SerializeField] protected EnemyBaseProfile BaseStats;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected NavMeshAgent Agent;
    [SerializeField] protected EnemyHealth Health;

    // ReSharper disable once IdentifierTypo
    [SerializeField] protected GameObject HardCodeTarget;

    protected bool IsInitialized;
    protected bool IsSeeingPlayer;
    protected bool WasSeeingPlayer;
    protected bool HasSeenPlayer;

    private bool HasDied;
    [SerializeField] protected Enemy Enemy;

    protected Transform Target;
    protected Vector3 StartPos;
    [SerializeField] private bool WasNotSpawned;

    [Header("Detectors")] [SerializeField] protected RayDetection RayDetectionPrefab;
    [SerializeField] protected Transform SightContainer;

    //The Transforms for the Detectors, must be an amount divisible by 2
    private RayDetection[] RayDetectorsSight;

    [Header("Dev Variables")]
    //How low the speed is to be considered not moving, just in case Navmesh doesn't do it's job stopping
    [SerializeField]
    protected float Tolerance;

    [SerializeField] private float PlayerTooFarAwayDst = 50f;
    [SerializeField] private float PlayerDetectionDistance;
    protected float SqrTolerance;

    private int CheckValue;

    private Collider[] Colls;

    private EnemySpawner MySpawner;
    private bool WasAlerted;

    #region Unity Events

    public void Start()
    {
        if (WasNotSpawned)
        {
            Init();

            if (gameObject.TryGetComponent(out ArcherEnemyKI archerEKI))
                archerEKI.LinkArrowPool(FightManager.instance.enemyArrowPool);

            Colls = GetComponentsInChildren<Collider>();
        }
    }

    /// <summary>
    /// The Method for Initialization
    /// </summary>
    public virtual void Init(EnemySpawner _mySpawner = null)
    {
        StartPos = transform.position;
        SqrTolerance = Tolerance * Tolerance;
        Animator.SetBool(Animator.StringToHash("IsKnockDownable"), KiStats.IsKnockDownable);
        RayDetectorsSight = SetDetectors(KiStats.SightDetectorCountHalf, SightContainer, KiStats.DetectionFOV,
            KiStats.DetectionRange, Color.cyan);
        Health.Initialize(BaseStats, Animator, this);
        Target = GameManager.instance.playerGO.transform;
        MySpawner = _mySpawner;
    }

    protected virtual void Update()
    {
        if (HasDied || !IsInitialized)
            return;

        //Debug.Log("--------------------------------------------------------------r43tgrgv");

        if (!IsSeeingPlayer || Vector3.Distance(StartPos, Target.position) > PlayerTooFarAwayDst)
        {
            if (Vector3.Distance(transform.position, Target.position) <= PlayerDetectionDistance)
            {
                IsSeeingPlayer = true;
                return;
            }

            IsSeeingPlayer = DetectorCheck(RayDetectorsSight);
        }
    }

    /// <summary>
    /// Sets the Speed of the Animator
    /// </summary>
    /// <param name="_speed">The New Value</param>
    public void SetAnimatorSpeed(float _speed) => Animator.speed = _speed;

    protected bool DetectorCheck(RayDetection[] _detectors) => _detectors.Any(_rayDetection => _rayDetection.Sight());

    public void UnusualNoticePlayerReaction(bool _wasAlerted = false)
    {
        IsSeeingPlayer = true;
        WasAlerted = true;
    }

    public void RestartAgent() => Agent.isStopped = false;

    #endregion

    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Death()
    {
        HasDied = true;
        Animator.SetTrigger(Animator.StringToHash("Dies"));
        Animator.SetBool(Animator.StringToHash("IsDead"), true);
        Agent.enabled = false;
        GetComponentInChildren<LocomotionAgent>().enabled = false;
        Destroy(Health.gameObject);
        FightManagerRemoveEnemy();
        if (Enemy.despawnAfterTime)
            foreach (Collider col in Colls)
                col.enabled = false;

        Enemy.EnemyDie();

        enabled = false;
    }

    public void DisableAnimator() => Animator.enabled = false;


    protected void NoticeEnemy()
    {
        if (!GameManager.instance)
            throw new Exception("No Game Manager Found");
        HasSeenPlayer = true;
        Animator.SetTrigger(Animator.StringToHash("NoticedYou"));
        Animator.SetBool(Animator.StringToHash("KnowsAboutYou"), true);

        if (MySpawner && !WasAlerted)
            MySpawner.AlertAllAssignedEnemies();
    }

    protected void FightManagerAddEnemy()
    {
        if (!FightManager.instance) return;
        if (FightManager.instance.enemiesInFight.Contains(this)) return;

        FightManager.instance.enemiesInFight.Add(this);

        if (GameManager.instance.musicAudioSource.clip == FightManager.instance.fightMusic) return;

        FightManager.instance.isInFight = true;
        FightManager.instance.StartCoroutine(FightManager.instance.FadeOldMusicOut());
    }
    public void FightManagerRemoveEnemy()
    {
        if (!FightManager.instance) return;
        if (!FightManager.instance.enemiesInFight.Contains(this)) return;
        FightManager.instance.enemiesInFight.Remove(this);

        if (GameManager.instance.musicAudioSource.clip != FightManager.instance.fightMusic || FightManager.instance.enemiesInFight.Count > 0) return;
        FightManager.instance.isInFight = false;

        FightManager.instance.StartCoroutine(FightManager.instance.FadeOldMusicOut());
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
    protected RayDetection[] SetDetectors(int _halvedCount, Transform _container, float _fov, float _range,
        Color _gizmoColor)
    {
        float detectorCount = _halvedCount * 2f;
        float angleSteps = (_fov / detectorCount) / 2f;
        RayDetection[] detectors = new RayDetection[(int)detectorCount];
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

    protected virtual void OnDrawGizmos() => VisualizeDetectors(Color.cyan, KiStats.DetectionRange, SightContainer);

    protected void VisualizeDetectors(Color _lineColor, float _range, Transform _container)
    {
        Gizmos.color = _lineColor;
        Transform t = _container.transform;
        Vector3 position = t.position;
        Gizmos.DrawLine(position, t.forward * _range + position);
    }

    #endregion
}