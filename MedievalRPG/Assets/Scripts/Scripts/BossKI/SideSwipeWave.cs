using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSwipeWave : SkeletonBossActions
{
    [SerializeField] private GameObject Wave;
    [SerializeField] private GameObject Wave2;
    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private Animator anim;

    public override void UseAction()
    {
        if (KI.phase1)
        {
            anim.SetTrigger("SideSwipe");
        }
        else
        {
            anim.SetTrigger("SideSwipeP2");
        }
    }

    public void CreateSwipe()
    {
        if (KI.phase1 == false)
        {
            GameObject wave;
            wave = Instantiate(Wave2);
            wave.transform.parent = null;
            wave.transform.position = KI.RoomEdges[0].transform.position;

            GameObject wave2;
            wave2 = Instantiate(Wave2);
            wave2.transform.parent = null;
            wave2.transform.Rotate(new Vector3(0, 180, 0), Space.World);
            wave2.transform.position = KI.RoomEdges[2].transform.position;
        }
        else
        {
            GameObject wave;
            wave = Instantiate(Wave);
            wave.transform.parent = null;
            wave.transform.position = KI.RoomEdges[0].transform.position;
        }

    }
}
