using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyHandler : MonoBehaviour
{
    public static DifficultyHandler instance;

    [HideInInspector] public float dmgMultiplier = 1;
    [HideInInspector] public float traglastMultiplier = 1;
    [HideInInspector] public float enemyHpMultiplier = 1;
    [HideInInspector] public float bossHpMultiplier = 1;
    [HideInInspector] public float pricesMultiplier = 1;
    [HideInInspector] public float bleedingMultiplier = 1;
    [HideInInspector] public int diffStage = 0;

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

    private void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            for (int i = 0; i < InventoryManager.instance.inventory.database.items.Length; i++)
            {
                InventoryManager.instance.inventory.database.items[i].highBuyPrice = InventoryManager.instance.inventory.database.items[i].buyPrice;
                InventoryManager.instance.inventory.database.items[i].highBuyPrice  = Mathf.RoundToInt(InventoryManager.instance.inventory.database.items[i].highBuyPrice * pricesMultiplier);
            }
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
            diffStage = 1;
        }
        if (diff == Difficulties.illusorisch)
        {
            dmgMultiplier = 1.15f;
            traglastMultiplier = 0.85f;
            enemyHpMultiplier = 1.2f;
            bossHpMultiplier = 1.5f;
            pricesMultiplier = 1.25f;
            bleedingMultiplier = 2;
            diffStage = 2;
        }
    }   
}       
        
        
        