using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossKI : MonoBehaviour
{
    [SerializeField] private List<SkeletonBossActions> FirstPhaseActions = new List<SkeletonBossActions>();
    [SerializeField] private List<SkeletonBossActions> SecondPhaseActions = new List<SkeletonBossActions>();
    [SerializeField] private Transform player;
    [SerializeField] private bool phase1 = true;
    [SerializeField] private float secondsToWait = 0.5f;
    [SerializeField] private float closeCombatDistance = 1;
    [SerializeField] private int arrowsHit = 0;
    [SerializeField] private int maxArrowsHit = 0;

    [SerializeField] private int weakAttackChance = 80; 
    [SerializeField] private int strongAttackChance = 10; 
    [SerializeField] private int moveChance = 10;
    [SerializeField] private int interactChance = 5;
    [SerializeField] private float choiceChanceMultiplier = 2;

    private List<string> Actions;
    private float playerDistance = 100;

    public void PickAction()
    {
        //Wenn der Spieler zu nah an den Boss herankommt
        if(playerDistance < closeCombatDistance)
        {
            Move();
        }
        else
        {
            if(phase1)
            {
                if(PlayerValueManager.instance.CurrHP < PlayerValueManager.instance.normalHP / 2)
                {
                    float wA = (weakAttackChance * choiceChanceMultiplier);
                    float sA = (strongAttackChance / choiceChanceMultiplier);

                    int choice = Random.Range(0, (int)(weakAttackChance + strongAttackChance + moveChance + interactChance));
                    if(choice <= wA )
                    {
                        WeakAttack();
                        return;
                    }
                    if (choice <= wA + sA && choice > wA)
                    {
                        StrongAttack();
                        return;
                    }
                    if (choice <= wA + sA + moveChance && choice > wA + sA)
                    {
                        Move();
                        return;
                    }
                    if (choice <= wA + sA + moveChance + interactChance && choice > wA + sA + moveChance)
                    {
                        Interact();
                        return;
                    }
                }
                else
                {
                    int choice = Random.Range(0, (int)(weakAttackChance + strongAttackChance + moveChance + interactChance));
                    if (choice <= weakAttackChance)
                    {
                        WeakAttack();
                        return;
                    }
                    if (choice <= weakAttackChance + strongAttackChance && choice > weakAttackChance)
                    {
                        StrongAttack();
                        return;
                    }
                    if (choice <= weakAttackChance + strongAttackChance + moveChance && choice > weakAttackChance + strongAttackChance)
                    {
                        Move();
                        return;
                    }
                    if (choice <= weakAttackChance + strongAttackChance + moveChance + interactChance && choice > weakAttackChance + strongAttackChance + moveChance)
                    {
                        Interact();
                        return;
                    }
                }
            }

            if (!phase1)
            {
                float wA;
                float sA;
                float fR;

                int choice = Random.Range(0, (int)(weakAttackChance + strongAttackChance + moveChance + interactChance));
                if (choice <= weakAttackChance)
                {
                    WeakAttack();
                    return;
                }
                if (choice <= weakAttackChance + strongAttackChance && choice > weakAttackChance)
                {
                    StrongAttack();
                    return;
                }
                if (choice <= weakAttackChance + strongAttackChance + moveChance && choice > weakAttackChance + strongAttackChance)
                {
                    Move();
                    return;
                }
                if (choice <= weakAttackChance + strongAttackChance + moveChance + interactChance && choice > weakAttackChance + strongAttackChance + moveChance)
                {
                    Interact();
                    return;
                }
                
            }
        }
    }

    private void WeakAttack()
    {

    }

    private void StrongAttack()
    {

    }

    private void Interact()
    {

    }

    private void Move()
    {
        foreach (SkeletonBossActions ability in FirstPhaseActions)
        {
            if (ability.name.Equals("MoveBoss"))
            {
                ability.UseAction();
            }
        }
    }

    private void RainOfFire()
    {

    }

    private void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
    }

    public IEnumerator CountArrows()
    {
        int aH = arrowsHit;
        yield return new WaitForSeconds(secondsToWait);
        if(arrowsHit > aH + maxArrowsHit)
        {
            RainOfFire();
        }
    }
}
