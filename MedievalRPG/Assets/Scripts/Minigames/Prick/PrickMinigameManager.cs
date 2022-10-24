using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public AnimationClip startNewMatchAnim;

    public bool playerStartedRound = false;

    [Header("Cards")]
    public List<PrickCardBase> allCards;
    public List<PrickCardBase> allUnusedCards = new List<PrickCardBase>();

    public int currGivenCardNumber = 0;

    [Header("Player")]
    public AnimationClip layPlayerCardAnim;
    public GameObject layedPlayerCardObj;
    public GameObject layerPlayerCardObjMiddle;

    public PrickCardBase layedPlayerCB;

    public GameObject[] playerCardGOs;

    [Header("Enemy")]
    public AnimationClip layEnemyCardAnim;
    public GameObject layedEnemyCardObj;
    public GameObject layerEnemyCardObjMiddle;

    public PrickCardBase layedEnemyCB;

    public GameObject[] enemyCardGOs;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        StartNewMatch();
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

        if (layedEnemyCB != null && layedPlayerCB != null)
        {
            if (layedEnemyCB.cardColor != layedPlayerCB.cardColor)
            {
                if (playerStartedRound)
                {
                    playerPoints += 1;
                }
                else
                {
                    enemyPoints += 1;
                }
                //    if (layedPlayerCB.cardNumber > layedEnemyCB.cardNumber)
                //    {
                //        playerPoints += 1;
                //    }
                //    else
                //    {
                //        enemyPoints += 1;
                //    }
            }
            else
            {
                if (layedPlayerCB.cardNumber < layedEnemyCB.cardNumber)
                {
                    playerPoints += 1;
                }
                else
                {
                    enemyPoints += 1;
                }
            }
        }
        else if (layedEnemyCB == null)
        {
            playerPoints += 1;
        }
        else if (layedPlayerCB == null)
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

        layedEnemyCB = null;
        layedPlayerCB = null;

        for (int i = 0; i < playerCardGOs.Length; i++)
        {
            playerCardGOs[i].GetComponent<Button>().interactable = true;
        }
    }

    IEnumerator StartEnemyCardPick()
    {
        yield return new WaitForSeconds(Random.Range(1, 2.5f));

        prickEnemy.PlayCard();
    }

    public void StartNewMatch()
    {
        for (int i = 0; i < PrickMinigameManager.instance.playerCardGOs.Length; i++)
        {
            PrickMinigameManager.instance.playerCardGOs[i].GetComponent<Button>().interactable = false;
        }

        allUnusedCards.Clear();

        currGivenCardNumber = 0;

        allUnusedCards = allCards;

        prickCardAnimator.enabled = true;
        prickCardAnimator.Play(startNewMatchAnim.name);
    }

    public void ActivateCards()
    {
        var newCardNumber = Random.Range(0, allUnusedCards.Count);
        prickEnemy.currentPlayableCards.Add(allUnusedCards[newCardNumber]);

        allUnusedCards.Remove(allUnusedCards[newCardNumber]);

        enemyCardGOs[currGivenCardNumber].SetActive(true);

        newCardNumber = Random.Range(0, allUnusedCards.Count);
        playerCardGOs[currGivenCardNumber].GetComponent<PrickCard>().pCB = allUnusedCards[newCardNumber];
        playerCardGOs[currGivenCardNumber].GetComponent<Image>().sprite = allUnusedCards[newCardNumber].cardSprite;

        allUnusedCards.Remove(allUnusedCards[newCardNumber]);

        playerCardGOs[currGivenCardNumber].SetActive(true);

        if (currGivenCardNumber >= 9)
        {
            for (int i = 0; i < PrickMinigameManager.instance.playerCardGOs.Length; i++)
            {
                PrickMinigameManager.instance.playerCardGOs[i].GetComponent<Button>().interactable = true;
            }

            prickCardAnimator.enabled = false;

            playerStartedRound = !playerStartedRound;

            if (playerStartedRound)
            {

            }
        }

        currGivenCardNumber += 1;
    }
}
