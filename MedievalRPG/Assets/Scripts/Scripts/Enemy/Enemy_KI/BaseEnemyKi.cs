using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

    protected Transform Target;
    protected Vector3 StartPos;

    public bool wasntSpawned = false;

    [Header("Detectors")] [SerializeField] protected RayDetection RayDetectionPrefab;
    [SerializeField] protected Transform SightContainer;

    //The Transforms for the Detectors, must be an amount divisible by 2
    private RayDetection[] RayDetectorsSight;

    [Header("Dev Variables")]
    //How low the speed is to be considered not moving, just in case Navmesh doesn't do it's job stopping
    [SerializeField]
    protected float Tolerance;
    [SerializeField]
    private float PlayerTooFarAwayDst = 50f;		

    protected float SqrTolerance;

    private int CheckValue;

    private Collider[] colls;

    #region Unity Events

    public void Start()
    {
        if (wasntSpawned)
        {
            Init();

            if (this.gameObject.TryGetComponent(out ArcherEnemyKI archerEKI))
            {
                archerEKI.LinkArrowPool(FightManager.instance.enemyArrowPool);
            }

            colls = GetComponentsInChildren<Collider>();
        }
    }

    /// <summary>
    /// The Method for Initialization
    /// </summary>
    public virtual void Init()
    {
        StartPos = transform.position;
        SqrTolerance = Tolerance * Tolerance;
        Animator.SetBool(Animator.StringToHash("IsKnockDownable"), KiStats.IsKnockDownable);
        RayDetectorsSight = SetDetectors(KiStats.SightDetectorCountHalf, SightContainer, KiStats.DetectionFOV,
            KiStats.DetectionRange, Color.cyan);
        Health.Initialize(BaseStats, Animator, this);
    }

    protected virtual void Update()
    {
        if (HasDied || !IsInitialized)
            return;

        if (!IsSeeingPlayer|| Vector3.Distance(StartPos, Target.position) > PlayerTooFarAwayDst)
            IsSeeingPlayer = DetectorCheck(RayDetectorsSight);
    }

    /// <summary>
    /// Sets the Speed of the Animator
    /// </summary>
    /// <param name="_speed">The New Value</param>
    public void SetAnimatorSpeed(float _speed)
    {
        Animator.speed = _speed;
    }

    protected bool DetectorCheck(RayDetection[] _detectors) => _detectors.Any(_rayDetection => _rayDetection.Sight());

    public void GotHitReaction()
    {
        IsSeeingPlayer = true;
    }
    #endregion

    public void RestartAgent()
    {
        Agent.isStopped = false;
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Death()
    {
        //TODO: Turn of all other scripts, animators, etc
        HasDied = true;
        Animator.SetTrigger(Animator.StringToHash("Dies"));
        Animator.SetBool(Animator.StringToHash("IsDead"),true);
        Agent.enabled = false;
        GetComponentInChildren<LocomotionAgent>().enabled = false;
        Destroy(Health.gameObject);

        for (int i = 0; i < colls.Length; i++)
        {
            colls[i].enabled = false;
        }

        enabled = false;
    }

    public void DisableAnimator()
    {
        Animator.enabled = false;
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

    protected virtual void OnDrawGizmos()
    {
        VisualizeDetectors(Color.cyan, KiStats.DetectionRange, SightContainer);
    }

    protected void VisualizeDetectors(Color _lineColor, float _range, Transform _container)
    {
        Gizmos.color = _lineColor;
        Transform t = _container.transform;
        Vector3 position = t.position;
        Gizmos.DrawLine(position, t.forward * _range + position);
    }

    #endregion
}