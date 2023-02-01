using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossKI : MonoBehaviour
{
    public Transform[] RoomEdges;
    public Transform player;
    public bool phase1 = true;
    
    [SerializeField] private List<SkeletonBossActions> BossActions;
    [SerializeField] private float secondsToWait = 0.5f;
    [SerializeField] private float closeCombatDistance = 1;
    [SerializeField] private int maxArrowsHit = 0;

    [SerializeField] private int weakAttackChance = 80;
    [SerializeField] private int strongAttackChance = 10;
    [SerializeField] private int meteorChance = 5;
    [SerializeField] private int moveChance = 10;
    [SerializeField] private int interactChance = 5;
    [SerializeField] private float choiceChanceMultiplier = 2;

    private float playerDistance = 100;
    private int arrowsHit = 0;
    private bool countArrows = false;

    private bool testBool = false;

    public void PickAction()
    {
        //Wenn der Spieler zu nah an den Boss herankommt
        if (playerDistance < closeCombatDistance)
        {
            Activate("SK_SkeletonMage_Green Variant (MoveBoss)");
        }
        else
        {
            if (phase1)
            {
                if (testBool || PlayerValueManager.instance.CurrHP < PlayerValueManager.instance.normalHP / 2)
                {
                    float wA = (weakAttackChance * choiceChanceMultiplier);
                    float sA = (strongAttackChance / choiceChanceMultiplier);
                    float mA = (moveChance * choiceChanceMultiplier);

                    int choice = Random.Range(0, (int)(wA + sA + mA + interactChance));
                    if (choice < wA)
                    {
                        Activate("SkeletonMageBossGreen (BossWeakAttacks)");
                        return;
                    }
                    if (choice <= wA + sA && choice > wA)
                    {
                        Activate("SkeletonMageBossGreen (BossStrongAttacks)");
                        return;
                    }
                    if (choice <= wA + sA + mA && choice > wA + sA)
                    {
                        Activate("SK_SkeletonMage_Green Variant (MoveBoss)");
                        return;
                    }
                    if (choice <= wA + sA + mA + interactChance && choice > wA + sA + mA)
                    {
                        Activate("Interact");
                        return;
                    }
                }
                else
                {
                    int choice = Random.Range(0, (int)(weakAttackChance + strongAttackChance + moveChance + interactChance));
                    if (choice < weakAttackChance)
                    {
                        Activate("SkeletonMageBossGreen (BossWeakAttacks)");
                        return;
                    }
                    if (choice <= weakAttackChance + strongAttackChance && choice > weakAttackChance)
                    {
                        Activate("SkeletonMageBossGreen (BossStrongAttacks)");
                        return;
                    }
                    if (choice <= weakAttackChance + strongAttackChance + moveChance && choice > weakAttackChance + strongAttackChance)
                    {
                        Activate("SK_SkeletonMage_Green Variant (MoveBoss)");
                        return;
                    }
                    if (choice <= weakAttackChance + strongAttackChance + moveChance + interactChance && choice > weakAttackChance + strongAttackChance + moveChance)
                    {
                        Activate("Interact");
                        return;
                    }
                }
            }

            if (!phase1)
            {
                float wA = (weakAttackChance / choiceChanceMultiplier);
                float sA = (strongAttackChance * choiceChanceMultiplier);

                int choice = Random.Range(0, (int)(wA + sA + moveChance + interactChance + meteorChance));
                if (choice < wA)
                {
                    Activate("SkeletonMageBossGreen (BossWeakAttacks)");
                    return;
                }
                if (choice <= wA + sA && choice > wA)
                {
                    Activate("SkeletonMageBossGreen (BossStrongAttacks)");
                    return;
                }
                if (choice <= wA + sA + moveChance && choice > wA + sA)
                {
                    Activate("SK_SkeletonMage_Green Variant (MoveBoss)");
                    return;
                }
                if (choice <= wA + sA + moveChance + interactChance && choice > wA + sA + moveChance)
                {
                    Activate("Interact");
                    return;
                }
                if (choice <= wA + sA + moveChance + interactChance + meteorChance && choice <= wA + sA + moveChance + interactChance)
                {
                    Activate("SkeletonMageBossGreen (MeteorShield)");
                    return;
                }
            }
        }
    }

    private void Activate(string action)
    {
        foreach (SkeletonBossActions ability in BossActions)
        {
            if (ability.ToString().Equals(action))
            {
                ability.UseAction();
            }
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
    }

    public IEnumerator CountArrows()
    {
        if(countArrows == true)
        {
            int aH = arrowsHit;
            countArrows = false;
            yield return new WaitForSeconds(secondsToWait);
            if (arrowsHit > aH + maxArrowsHit)
            {
                Activate("SkeletonMageBossGreen (MeteorShield)");
            }
            countArrows = true;
        }
    }
}
