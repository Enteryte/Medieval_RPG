using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Generic_Archer_Enemy_KI : MonoBehaviour
{
    [SerializeField] private SO_KI_Stats KiStats;
    [SerializeField] private Animator Animator;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private EnemyHealth Health;
    [SerializeField] private Transform FiringPoint;

    private ArrowPool ArrowPool;
    private bool isInitialized;
    private bool IsSeeingPlayer;
    private bool HasSeenPlayer;
    private bool IsInAttackRange;

    private bool HasDied;

    public void Initialize(ArrowPool _arrowPool)
    {
        ArrowPool = _arrowPool;

        isInitialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasDied || !isInitialized)
            return;
    }

    
    private void FireArrow()
    {
        ArrowPool.SpawnAndFireArrow(FiringPoint);
    }
}