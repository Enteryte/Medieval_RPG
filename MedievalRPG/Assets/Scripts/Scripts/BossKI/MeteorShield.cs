using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShield : SkeletonBossActions
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject meteor;
    [SerializeField] private GameObject shield;
    [SerializeField] private SkeletonBossKI KI;

    private ShieldScript shieldScript;

    public override void UseAction()
    {
        anim.SetTrigger("MeteorShield");
    }

    private Vector3 PickTargetLocation()
    {
        float minX = 0;
        float maxX = 0;
        float minZ = 0;
        float maxZ = 0;

        for (int x = 0; x < KI.RoomEdges.Length; x++)
        {
            if (KI.RoomEdges[x].position.x < minX)
            {
                minX = KI.RoomEdges[x].position.x;
            }
            if (KI.RoomEdges[x].position.x > maxX)
            {
                maxX = KI.RoomEdges[x].position.x;
            }

            if (KI.RoomEdges[x].position.z < minZ)
            {
                minZ = KI.RoomEdges[x].position.z;
            }
            if (KI.RoomEdges[x].position.z > maxZ)
            {
                maxZ = KI.RoomEdges[x].position.z;
            }
        }

        return new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
    }

    public void CreateMeteor()
    {
        GameObject Meteor = Instantiate(meteor);
        Meteor.transform.parent = null;
        Meteor.transform.position = PickTargetLocation();
    }

    public void CreateShield()
    {
        GameObject Shield = Instantiate(shield);
        Shield.transform.position = transform.position;
        shieldScript = Shield.GetComponent<ShieldScript>();
    }

    public void DeactivateShield()
    {
        shieldScript.Deactivate();
    }
}
