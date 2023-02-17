using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Base Profile", menuName = "Scriptable Objects/Enemies/Enemy Base Profile", order = 0)]
public class EnemyBaseProfile : ScriptableObject
{
    
    public string enemyName;
    public float normalHealth;
    public float normalDamage;

    [Header("Possible Loot")]
    public ItemBaseProfile[] possibleLootItems;

    public int minLootMoney;
    public int maxLootMoney;
}
