using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public abstract class BaseEnemyKI : MonoBehaviour
{
    //TODO: Cut this down and have the basic functions remain here, then let the melee and archer KI inherit from it.
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

    protected bool HasDied;
    
    protected Transform Target;
    protected Vector3 StartPos;


    [Header("Detectors")] [SerializeField] protected RayDetection RayDetectionPrefab;
    [SerializeField] protected Transform SightContainer;

    //The Transforms for the Detectors, must be an amount divisible by 2
    protected RayDetection[] RayDetectorsSight;

    [Header("Dev Variables")]
    //How low the speed is to be considered not moving, just in case Navmesh doesn't do it's job stopping
    [SerializeField]
    protected float Tolerance;
    protected float SqrTolerance;

    private int CheckValue;

    #region Unity Events

    public void Start()
    {
        if (this.gameObject.TryGetComponent(out MeleeEnemyKi mEKI))
        {
            mEKI.Init();
        }
        else if (this.gameObject.TryGetComponent(out ArcherEnemyKI aEKI))
        {
            aEKI.Init();
            aEKI.LinkArrowPool(EnemyInitializer.instance.ArrowPool);
        }
    }

    /// <summary>
    /// The Method for Initialization
    /// </summary>
    public virtual void Init()
    {
        StartPos = transform.position;
        SqrTolerance = Tolerance * Tolerance;
        // ReSharper disable once StringLiteralTypo
        Animator.SetBool(Animator.StringToHash("IsKnockDownable"), KiStats.IsKnockDownable);
        RayDetectorsSight = SetDetectors(KiStats.SightDetectorCountHalf, SightContainer, KiStats.DetectionFOV,
            KiStats.DetectionRange, Color.cyan);
        Health.Initialize(BaseStats, Animator, this);
    }

    protected virtual void Update()
    {
        if (HasDied && !IsInitialized)
            return;

        if(!IsSeeingPlayer)
        IsSeeingPlayer = DetectorCheck(RayDetectorsSight);

        Animator.SetBool(Animator.StringToHash("IsMoving"), (Agent.velocity.sqrMagnitude > SqrTolerance));

    }
/// <summary>
/// Sets the Speed of the Animator
/// </summary>
/// <param name="_speed">The New Value</param>
    public void SetAnimatorSpeed(float _speed)
    {
        Animator.speed = _speed;
    }
    protected bool DetectorCheck(RayDetection[] _detectors)
    {
        CheckValue = 0;
        for (int i = 0; i < _detectors.Length; i++)
            CheckValue += _detectors[i].Sight() ? 1 : 0;
        // Debug.Log(CheckValue);
        return (CheckValue > 0);
    }

    #endregion
    public virtual void Death()
    {
        //TODO: Turn of all other scripts, animators, etc. and turn the enemy into a ragdoll
        HasDied = true;
        Animator.SetBool(Animator.StringToHash("IsDead"), true);
        Agent.enabled = false;
        
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