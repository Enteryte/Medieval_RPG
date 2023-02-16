using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] AssignedSpawnPoints;
    [SerializeField] private BaseEnemyKI[] EnemyKisToSpawn;
    [SerializeField] private ArrowPool ArrowPool;

    private BaseEnemyKI[] AssignedEnemyKIs;
    private bool WasTriggered;
    
    private void OnTriggerEnter(Collider _other)
    {
        if(!_other.CompareTag("Player"))return;
        if(WasTriggered)
            return;
        WasTriggered = true;
        if (AssignedSpawnPoints.Length == 0 || EnemyKisToSpawn.Length == 0)
            return;
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        AssignedEnemyKIs = new BaseEnemyKI[EnemyKisToSpawn.Length];
        for (int i = 0; i < AssignedSpawnPoints.Length; i++)
        {
            if (i > EnemyKisToSpawn.Length)
                return;
            BaseEnemyKI myEnemy = Instantiate(EnemyKisToSpawn[i], AssignedSpawnPoints[i].position, Quaternion.identity);
            myEnemy.Init();
            if (myEnemy is ArcherEnemyKI myArcherEnemy)
                myArcherEnemy.LinkArrowPool(ArrowPool);
            myEnemy.transform.rotation = AssignedSpawnPoints[i].rotation;
            AssignedEnemyKIs[i] = myEnemy;
        }
    }

    public void AlertAllAssignedEnemies()
    {
        foreach (BaseEnemyKI ki in AssignedEnemyKIs) 
            ki.UnusualNoticePlayerReaction(true);
        Destroy(gameObject);
    }
}