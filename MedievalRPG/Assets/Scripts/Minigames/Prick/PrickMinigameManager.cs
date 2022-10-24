using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrickMinigameManager : MonoBehaviour
{
    public static PrickMinigameManager instance;

    public Animator prickCardAnimator;

    public PrickEnemy prickEnemy;

    public int playerPoints;
    public int enemyPoints;

    public TMP_Text playerPointsTxt;
    public TMP_Text enemyPointsTxt;

    public AnimationClip endRoundAnim;

    [Header("Player")]
    public AnimationClip layPlayerCardAnim;
    public GameObject layedPlayerCardObj;
    public GameObject layerPlayerCardObjMiddle;

    public PrickCardBase layedPlayerCB;

    [Header("Enemy")]
    public AnimationClip layEnemyCardAnim;
    public GameObject layedEnemyCardObj;
    public GameObject layerEnemyCardObjMiddle;

    public PrickCardBase layedEnemyCB;

    public void Awake()
    {
        instance = this;
    }

    public void DisablePrickAnimator()
    {
        prickCardAnimator.enabled = false;

        //layerPlayerCardObjMiddle.SetActive(true);

        if (layedEnemyCB == null || layedPlayerCB == null)
        {
            if (layedEnemyCB == null)
            {
                //layerPlayerCardObjMiddle.SetActive(true);
                TriggerEnemy();
            }
            else if (layedEnemyCB != null)
            {
                layerEnemyCardObjMiddle.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(WaitTillEndRound());
        }
    }

    public void TriggerEnemy()
    {
        StartCoroutine(StartEnemyCardPick());
    }

    IEnumerator WaitTillEndRound()
    {
        yield return new WaitForSeconds(1.5f);

        layerPlayerCardObjMiddle.SetActive(true);
        layerEnemyCardObjMiddle.SetActive(true);

        EndRound();
    }

    public void EndRound()
    {
        prickCardAnimator.enabled = true;
        prickCardAnimator.Play(endRoundAnim.name);

        if (layedPlayerCB.cardNumber > layedEnemyCB.cardNumber)
        {
            playerPoints += 1;
        }
        else
        {
            enemyPoints += 1;
        }

        playerPointsTxt.text = playerPoints.ToString();
        enemyPointsTxt.text = enemyPoints.ToString();

        //layedEnemyCB = null;
        //layedPlayerCB = null;
    }

    public void StartNewRound()
    {
        prickCardAnimator.enabled = false;
    }

    IEnumerator StartEnemyCardPick()
    {
        yield return new WaitForSeconds(Random.Range(1, 2.5f));

        prickEnemy.PlayCard();
    }
}
