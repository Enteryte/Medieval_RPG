using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportShot : SkeletonBossActions
{
    [SerializeField] private Animator anim;
    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float shotSpeed = 3;
    [SerializeField] private float shotCount = 3;

    private Transform[] RoomEdges;
    private Vector3 targetLocation;

    private void Start()
    {
        RoomEdges = KI.RoomEdges;
    }

    public override void UseAction()
    {
        PickTargetLocation();
        CreatePortals();
        anim.SetTrigger("TeleportShot");
    }

    private void PickTargetLocation()
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
    }

    private void CreatePortals()
    {
        GameObject portal_1 = Instantiate(portal);
        GameObject portal_2 = Instantiate(portal);
    
        portal_1.transform.parent = null;
        portal_2.transform.parent = null;

        portal_1.transform.position = KI.gameObject.transform.position;
        portal_2.transform.position = targetLocation;
    }

    public void ShootProjectiles()
    {
        for (int i = 0; i < shotCount; i++)
        {
            GameObject proj = Instantiate(projectile);
            proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward * shotSpeed, ForceMode.Acceleration);
        }
    }

    public void TeleportToTargetLocation()
    {
        KI.transform.position = targetLocation;
    }
}
