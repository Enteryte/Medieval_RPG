using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string Tutorial = "Diese Funktion benötigt entweder einen Collider oder kann auch als Manuell ausgeführt werden um Gegner zu Spawnen.\n Welche Art Collider ist Irellevant, er muss allerdings auf IsTrigger gesetzt sein.";
    [SerializeField] private Transform[] AssignedSpawnPoints;

    [SerializeField] private MeleeEnemyKi[] MeleeEnemyKisToSpawn;
    [SerializeField] private ArcherEnemyKI[] ArcherEnemyKisToSpawn;
    [SerializeField] private ArrowPool ArrowPool;
    private void OnTriggerEnter(Collider _other)
    {
        if(AssignedSpawnPoints.Length == 0)
            return;
        SpawnEnemies();
        Destroy(gameObject);
    }

    public void SpawnEnemies()
    {
        for (int i = 0, j = 0; i < AssignedSpawnPoints.Length; i++)
        {
            if(i > (MeleeEnemyKisToSpawn.Length + ArcherEnemyKisToSpawn.Length))
               return;
            if (i <= MeleeEnemyKisToSpawn.Length && MeleeEnemyKisToSpawn.Length > 0)
            {
                BaseEnemyKI myEnemy = Instantiate(MeleeEnemyKisToSpawn[i], AssignedSpawnPoints[i].position, Quaternion.identity);
                myEnemy.Init();
            }
            else
            {
                ArcherEnemyKI myEnemy = Instantiate(ArcherEnemyKisToSpawn[j], AssignedSpawnPoints[i].position, Quaternion.identity);
                myEnemy.Init();
                myEnemy.LinkArrowPool(ArrowPool);
                // Debug.Log("Archer Spawned");
                j++;
            }
        }
    }
}