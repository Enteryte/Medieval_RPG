using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveImpulse : SkeletonBossActions
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject WaveObject;

    public override void UseAction()
    {
        anim.SetTrigger("ShockWave");
    }

    public void CreateWave()
    {
        GameObject wave = Instantiate(WaveObject);
        wave.transform.parent = null;
        wave.transform.position = transform.position;
    }
}
