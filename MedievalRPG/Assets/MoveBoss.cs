using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveBoss : SkeletonBossActions
{
    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private Transform[] RoomEdges;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject head;
    [SerializeField] private Animator anim;
    [SerializeField] private float acceptableDistancefromTargetLocation = 3;
    [SerializeField] private float bodyRotationSpeed = 0;
    [SerializeField] private float headRotationSpeed = 0;
    [SerializeField] private float blendSpeed = 0;
    [SerializeField] private bool ikActive = true;

    private Vector3 targetLocation;
    private Transform player;
    private bool mayMove = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHeadObject").transform;
        if (RoomEdges.Length > 4)
        {
            for (int i = 0; i < RoomEdges.Length; i++)
            {
                if (i > 3)
                {
                    RoomEdges[i] = null;
                }
            }
        }
    }

    public override void UseAction()
    {
        PickNewTargetLocation();
    }

    private void PickNewTargetLocation()
    {
        float minX = 0;
        float maxX = 0;
        float minZ = 0;
        float maxZ = 0;

        for (int x = 0; x < RoomEdges.Length; x++)
        {
            if (RoomEdges[x].position.x < minX)
            {
                minX = RoomEdges[x].position.x;
            }
            if (RoomEdges[x].position.x > maxX)
            {
                maxX = RoomEdges[x].position.x;
            }

            if (RoomEdges[x].position.z < minZ)
            {
                minZ = RoomEdges[x].position.z;
            }
            if (RoomEdges[x].position.z > maxZ)
            {
                maxZ = RoomEdges[x].position.z;
            }
        }

        targetLocation = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        agent.SetDestination(targetLocation);
        mayMove = true;
    }

    private void CheckDitsanceFromTargetLocation()
    {
        if (Vector3.Distance(transform.position, targetLocation) <= acceptableDistancefromTargetLocation)
        {
            mayMove = false;
            agent.SetDestination(transform.position);
            KI.PickAction();
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction;
        Quaternion lookRotation;
        
        direction = (player.position - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        lookRotation.x = 0;
        lookRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * bodyRotationSpeed);
    }

    private void OnAnimatorIK()
    {
        if (anim)
        {
            if (ikActive)
            {
                if (mayMove == false && player != null)
                {
                    anim.SetLookAtWeight(1);
                    anim.SetLookAtPosition(player.position);
                }
                if (mayMove == true)
                {
                    anim.SetLookAtWeight(1);
                    anim.SetLookAtPosition(new Vector3(targetLocation.x, targetLocation.y + 2, targetLocation.z));
                }
            }
        }
    }

    private void Update()
    {
        if (mayMove == true)
        {
            anim.SetFloat("Speed", anim.GetFloat("Speed") + blendSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, 0);
            CheckDitsanceFromTargetLocation();
        }
        else
        {
            anim.SetFloat("Speed", 0);
            RotateTowardsPlayer();
        }
    }
}
