using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private TeleportShot Teleport;
    [SerializeField] private SideSwipeWave SideSwipe;
    [SerializeField] private ShockWaveImpulse ShockWave;
    [SerializeField] private MeteorShield MeteorShield;
    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private SkeletonBossStats stats;

    private void TeleportBoss()
    {
        Teleport.TeleportToTargetLocation();
    }
    private void ShootProjectiles()
    {
        Teleport.ShootProjectiles();
    }
    private void CreateSwipe()
    {
        SideSwipe.CreateSwipe();
    }
    private void PickAction()
    {
        KI.PickAction();
    }
    private void CreateWave()
    {
        ShockWave.CreateWave();
    }
    private void CreateMeteor()
    {
        MeteorShield.CreateMeteor();
    }
    private void CreateShield()
    {
        MeteorShield.CreateShield();
    }
    private void DeactivateShield()
    {
        MeteorShield.DeactivateShield();
    }
    private void HealBoss()
    {
        stats.HealBoss();
    }
}
