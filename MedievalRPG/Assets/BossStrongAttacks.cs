using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStrongAttacks : SkeletonBossActions
{
    [SerializeField] private List<SkeletonBossActions> Attacks;

    public override void UseAction()
    {
        int attack = Random.Range(0, Attacks.Count);
        Attacks[attack].UseAction();
    }
}
