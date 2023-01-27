using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private TeleportShot Teleport;

    private void TeleportBoss()
    {
        Teleport.TeleportToTargetLocation();
    }
    private void ShootProjectiles()
    {
        Teleport.ShootProjectiles();
    }
}
