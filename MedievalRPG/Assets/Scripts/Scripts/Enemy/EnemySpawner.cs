using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] AssignedSpawnPoints;

    [SerializeField] private MeleeEnemyKi[] MeleeEnemyKisToSpawn;
    [SerializeField] private ArcherEnemyKI[] ArcherEnemyKisToSpawn;

    private void OnTriggerEnter(Collider _other)
    {
        if(AssignedSpawnPoints.Length == 0)
            return;
        SpawnEnemies();
        Destroy(gameObject);
    }

    public void SpawnEnemies()
    {
        if (AssignedSpawnPoints.Length < (MeleeEnemyKisToSpawn.Length + ArcherEnemyKisToSpawn.Length))
            throw new Exception($"Too many Enemies, too few Spawnpoints at {gameObject.name}");
        for (int i = 0, j = 0; i < AssignedSpawnPoints.Length; i++)
        {
            if (i <= MeleeEnemyKisToSpawn.Length)
            {
                BaseEnemyKI myEnemy = Instantiate(MeleeEnemyKisToSpawn[i], AssignedSpawnPoints[i].position,
                    Quaternion.identity);
                myEnemy.Init();
            }
            else
            {
                ArcherEnemyKI myEnemy = Instantiate(ArcherEnemyKisToSpawn[j], AssignedSpawnPoints[i].position,
                    Quaternion.identity);
                myEnemy.Init();
                myEnemy.LinkArrowPool(EnemyInitializer.instance.ArrowPool);
                j++;
            }
        }
    }
}