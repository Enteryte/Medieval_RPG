using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyHandler : MonoBehaviour
{
    public static DifficultyHandler instance;

    [HideInInspector] public float dmgMultiplier = 1;
    [HideInInspector] public float traglastMultiplier = 1;
    [HideInInspector] public float enemyHpMultiplier = 1;
    [HideInInspector] public float bossHpMultiplier = 1;
    [HideInInspector] public float pricesMultiplier = 1;
    [HideInInspector] public float bleedingMultiplier = 1;

    [HideInInspector]
    public enum Difficulties
    {
        düster,
        blutig,
        illusorisch
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetDifficulty(Difficulties diff)
    {
        if(diff == Difficulties.düster)
        {
            return;
        }
        if(diff == Difficulties.blutig)
        {
            dmgMultiplier = 1.15f;
            traglastMultiplier = 0.85f;
            enemyHpMultiplier = 1;
            bossHpMultiplier = 1;
            pricesMultiplier = 1.2f;
            bleedingMultiplier = 1;
        }
        if (diff == Difficulties.illusorisch)
        {
            dmgMultiplier = 1.15f;
            traglastMultiplier = 0.85f;
            enemyHpMultiplier = 1.2f;
            bossHpMultiplier = 1.5f;
            pricesMultiplier = 1.25f;
            bleedingMultiplier = 2;
        }
    }   
}       
        
        
        