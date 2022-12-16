using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArrowPool))]
public class EnemyInitializer : MonoBehaviour
{
    //TODO: Turn this into a proper Initializer and also make it activate enemies only when the player is near
    [SerializeField] private MeleeEnemyKi[] MeleeEnemies;
    [SerializeField] private ArcherEnemyKI[] ArcherEnemies;
    [SerializeField] private ArrowPool ArrowPool;

    
    // Start is called before the first frame update
    void Start()
    {
        //Todo: Replace those magic numbers when this can be set via difficulty options
        ArrowPool.InitializeArrows(1f, 2f, 3f);
        for (int i = 0; i < ArcherEnemies.Length; i++)
        {
            ArcherEnemies[i].Initialize(ArrowPool);
        }
    }
}