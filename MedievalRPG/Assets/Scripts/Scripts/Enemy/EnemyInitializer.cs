using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArrowPool))]
public class EnemyInitializer : MonoBehaviour
{
    public static EnemyInitializer instance;

    //TODO: Turn this into a proper Initializer and also make it activate enemies only when the player is near
    [SerializeField] private MeleeEnemyKi[] MeleeEnemies;
    [SerializeField] private ArcherEnemyKI[] ArcherEnemies;
    [SerializeField] public ArrowPool ArrowPool;
    
    // Start is called before the first frame update
    public void Awake()
    {
        //Debug.Log("ISHEREMFGZHJNKM;");

        ////Todo: Replace those magic numbers when this can be set via difficulty options
        //for (int i = 0; i < MeleeEnemies.Length; i++)
        //{
        //    MeleeEnemies[i].Init();
        //    Debug.Log(MeleeEnemies[i].name + "-----------------------------------------------------------------");
        //}

        for (int i = 0; i < ArcherEnemies.Length; i++)
        {
            ArcherEnemies[i].Init();
            ArcherEnemies[i].LinkArrowPool(ArrowPool);
        }
    }

    public void Start()
    {
        instance = this;
    }
}