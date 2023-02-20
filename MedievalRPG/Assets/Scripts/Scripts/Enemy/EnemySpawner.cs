using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] AssignedSpawnPoints;
    [SerializeField] private BaseEnemyKI[] EnemyKisToSpawn;
    [SerializeField] private ArrowPool ArrowPool;
    [SerializeField] private float DespawnDistance;

    private BaseEnemyKI[] AssignedEnemyKIs;
    private bool WasTriggered;
    private bool EnemiesAreAlert;
    private Transform Player;
    
    

    private void OnTriggerEnter(Collider _other)
    {
        if (!_other.CompareTag("Player")) return;
        if (WasTriggered)
            return;
        WasTriggered = true;
        if (AssignedSpawnPoints.Length == 0 || EnemyKisToSpawn.Length == 0)
            return;
        SpawnEnemies();
    }

    private void Start()
    {
        if (GameManager.instance)
            Player = GameManager.instance.playerGO.transform;
    }

    private void Update()
    {
        if (!(Vector3.Distance(transform.position, Player.position) > DespawnDistance) || !WasTriggered) return;
        
        for (int i = AssignedEnemyKIs.Length - 1; i >= 0; i--) 
            Destroy(AssignedEnemyKIs[i].gameObject);
        AssignedEnemyKIs = Array.Empty<BaseEnemyKI>();
        WasTriggered = false;
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

        if (!EnemiesAreAlert) return;
        EnemiesAreAlert = false;
        AlertAllAssignedEnemies();
    }

    public void AlertAllAssignedEnemies()
    {
        if(EnemiesAreAlert)return;
        EnemiesAreAlert = true;
        foreach (BaseEnemyKI ki in AssignedEnemyKIs)
            ki.UnusualNoticePlayerReaction(true);
        Destroy(gameObject);
    }
}