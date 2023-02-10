using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SideSwiperSpawner : MonoBehaviour
{
    [SerializeField] private Transform SpawnPointL;
    [SerializeField] private Transform SpawnPointR;
    [SerializeField] private EnemyDamager SideSwipePrefab;
    private float SideSwipeDamage;

    private void Start()
    {
        SpawnSwipers();
    }

    public void Init(float _sideswipeDamage)
    {
        SideSwipeDamage = _sideswipeDamage;
    }
/// <summary>
/// This is the function the boss can use to spawn the Swipers
/// </summary>
    public void SpawnSwipers()
    {
        EnemyDamager sideSwipe = Instantiate(SideSwipePrefab, SpawnPointL.position, quaternion.identity, SpawnPointL);
        sideSwipe.Init(SideSwipeDamage);
        sideSwipe = Instantiate(SideSwipePrefab, SpawnPointR.position, quaternion.identity, SpawnPointR);
        sideSwipe.Init(SideSwipeDamage);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color= Color.yellow;
        Gizmos.DrawSphere(SpawnPointL.position, 0.1f);
        Gizmos.DrawSphere(SpawnPointR.position, 0.1f);
        Gizmos.color= Color.cyan;
        //So you can see which direction is forward.
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y,transform.position.z +0.5f ), new Vector3(0.1f, 0.1f,1.0f));
    }
}
