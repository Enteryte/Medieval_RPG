using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] AssignedSpawnPoints;

    [SerializeField] private BaseEnemyKI[] EnemyKisToSpawn;
    [SerializeField] private ArrowPool ArrowPool;

    private void OnTriggerEnter(Collider _other)
    {
        if (AssignedSpawnPoints.Length == 0 || EnemyKisToSpawn.Length == 0)
            return;
        SpawnEnemies();
        Destroy(gameObject);//Maybe turn this into a dst check if the others later want the player to be able to escape after all and allow it to just despawn it's spawned enemies.
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < AssignedSpawnPoints.Length; i++)
        {
            if (i > EnemyKisToSpawn.Length)
                return;
            BaseEnemyKI myEnemy = Instantiate(EnemyKisToSpawn[i], AssignedSpawnPoints[i].position, AssignedSpawnPoints[i].rotation);
            myEnemy.Init();
            if (myEnemy is ArcherEnemyKI myArcherEnemy)
                myArcherEnemy.LinkArrowPool(ArrowPool);
        }
    }
}