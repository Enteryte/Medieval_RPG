using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShootDelegator : MonoBehaviour
{
    [SerializeField] private ArcherEnemyKI AI;
    public void ReleaseArrow() => AI.FireArrow();
}
