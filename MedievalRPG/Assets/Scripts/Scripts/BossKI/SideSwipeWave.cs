using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSwipeWave : SkeletonBossActions
{
    [SerializeField] private GameObject Wave;
    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private Animator anim;

    public override void UseAction()
    {
        anim.SetTrigger("SideSwipe");
    }

    public void CreateSwipe()
    {
        GameObject wave = Instantiate(Wave);
        wave.transform.parent = null;
        wave.transform.position = KI.RoomEdges[0].transform.position;
    }
}
