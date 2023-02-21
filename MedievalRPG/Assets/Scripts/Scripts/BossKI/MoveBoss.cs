using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveBoss : SkeletonBossActions
{
    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject head;
    [SerializeField] private Animator anim;
    [SerializeField] private float acceptableDistancefromTargetLocation = 3;
    [SerializeField] private float bodyRotationSpeed = 0;
    [SerializeField] private float headRotationSpeed = 0;
    [SerializeField] private float blendSpeed = 0;
    [SerializeField] private bool ikActive = true;

    private Vector3 targetLocation;
    private Transform[] RoomEdges;
    private Transform player;
    private bool mayMove = false;

    private void Start()
    {
        RoomEdges = KI.RoomEdges;
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
        if (RoomEdges == null)
        {
            RoomEdges = KI.RoomEdges;
        }

        float minX = Mathf.Min(RoomEdges[0].position.x, RoomEdges[1].position.x, RoomEdges[2].position.x, RoomEdges[3].position.x);
        float maxX = Mathf.Max(RoomEdges[0].position.x, RoomEdges[1].position.x, RoomEdges[2].position.x, RoomEdges[3].position.x);
        float minZ = Mathf.Min(RoomEdges[0].position.z, RoomEdges[1].position.z, RoomEdges[2].position.z, RoomEdges[3].position.z);
        float maxZ = Mathf.Max(RoomEdges[0].position.z, RoomEdges[1].position.z, RoomEdges[2].position.z, RoomEdges[3].position.z);

        targetLocation = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

        agent.SetDestination(targetLocation);
        mayMove = true;
    }

    private void CheckDitsanceFromTargetLocation()
    {
        if (Vector3.Distance(KI.gameObject.transform.position, targetLocation) <= acceptableDistancefromTargetLocation)
        {
            mayMove = false;
            agent.SetDestination(KI.gameObject.transform.position);
            KI.PickAction();
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction;
        Quaternion lookRotation;
        
        direction = (player.position - KI.gameObject.transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        lookRotation.x = 0;
        lookRotation.z = 0;
        KI.gameObject.transform.rotation = Quaternion.Slerp(KI.gameObject.transform.rotation, lookRotation, Time.deltaTime * bodyRotationSpeed);
    }

    private void ResetTransform()
    {
        transform.position = KI.gameObject.transform.position;
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
            transform.localEulerAngles = new Vector3(0, 0, 0);
            CheckDitsanceFromTargetLocation();
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

        RotateTowardsPlayer();
        ResetTransform();
    }
}
