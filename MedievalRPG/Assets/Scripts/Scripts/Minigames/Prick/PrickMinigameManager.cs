using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class PrickMinigameManager : MonoBehaviour
{
    public static PrickMinigameManager instance;

    public Camera prickCamera;

    public Animator prickCardAnimator;

    public PrickEnemy prickEnemy;

    public int playerPoints;
    public int enemyPoints;

    public TMP_Text playerPointsTxt;
    public TMP_Text enemyPointsTxt;
    public TMP_Text playerMoneyTxt;

    public AnimationClip endRoundAnim;
    public AnimationClip startNewMatchAnim;

    public bool playerStartedRound = false;

    public float winMoneyAmount;

    public Button startNewMatchBtn;

    public GameObject prickUI;

    [Header("Cards")]
    public List<PrickCardBase> allCards;
    public List<PrickCardBase> allUnusedCards = new List<PrickCardBase>();

    public int currGivenCardNumber = 0;

    [Header("Player")]
    public AnimationClip layPlayerCardAnim;
    public AnimationClip playerLayCardSecondAnim;
    public GameObject layedPlayerCardObj;
    public GameObject layerPlayerCardObjMiddle;

    public PrickCardBase layedPlayerCB;

    public GameObject[] playerCardGOs;

    [Header("Enemy")]
    public AnimationClip layEnemyCardAnim;
    public AnimationClip enemyLayCardFirstAnim;
    public GameObject layedEnemyCardObj;
    public GameObject layerEnemyCardObjMiddle;

    public PrickCardBase layedEnemyCB;

    public GameObject[] enemyCardGOs;

    [Header("Tutorial")]
    public TutorialBaseProfile welcomeTutorial;
    public TutorialBaseProfile chooseACardTutorial;
    public TutorialBaseProfile pointsAndGoldTutorial;

    public void OnEnable()
    {
        //if (instance != this || instance != null)
        //{
        //    //instance.prickUI = this.prickUI;

        //    return;
        //}

        prickUI = GameManager.instance.prickMGUI;

        playerPoints = 0;
        enemyPoints = 0;

        playerMoneyTxt.text = PlayerValueManager.instance.money.ToString();

        playerPointsTxt.text = playerPoints.ToString();
        enemyPointsTxt.text = enemyPoints.ToString();

        if (PlayerValueManager.instance.money >= winMoneyAmount)
        {
            startNewMatchBtn.interactable = true;
        }
        else
        {
            startNewMatchBtn.interactable = false;
        }

        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(welcomeTutorial);
    }

    public void Awake()
    {
        //if (instance == null)
        //{
            instance = this;

        prickUI = GameManager.instance.prickMGUI;
        //}
        //else
        //{
        //    instance.prickUI = this.prickUI;
        //}
    }

    public void Start()
    {
        startNewMatchBtn.onClick.AddListener(StartNewMatch);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && startNewMatchBtn.GetComponent<Button>().interactable && prickEnemy.currentPlayableCards.Count == 0)
        {
            prickUI.SetActive(false);
            prickCamera.enabled = false;

            ThirdPersonController.instance.canMove = true;

            GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

            this.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PlayerValueManager.instance.money == 0)
        {
            prickUI.SetActive(false);
            prickCamera.enabled = false;

            ThirdPersonController.instance.canMove = true;

            GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

            this.enabled = false;
        }
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

        if (prickEnemy.currentPlayableCards.Count == 0)
        {
            if (playerPoints > enemyPoints)
            {
                PlayerValueManager.instance.money += winMoneyAmount;
            }
            else if (playerPoints < enemyPoints)
            {
                PlayerValueManager.instance.money -= winMoneyAmount;
            }

            playerMoneyTxt.text = PlayerValueManager.instance.money.ToString();

            startNewMatchBtn.interactable = true;
        }

        //layedEnemyCB = null;
        //layedPlayerCB = null;

        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(pointsAndGoldTutorial);
    }

    public void StartNewRound()
    {
        prickCardAnimator.enabled = false;

        layedEnemyCB = null;
        layedPlayerCB = null;

        if (!playerStartedRound)
        {
            for (int i = 0; i < playerCardGOs.Length; i++)
            {
                playerCardGOs[i].GetComponent<Button>().interactable = true;
            }

            playerStartedRound = true;
        }
        else
        {
            if (prickEnemy.currentPlayableCards.Count > 0)
            {
                playerStartedRound = false;

                prickEnemy.StartRound();
            }
        }
    }

    IEnumerator StartEnemyCardPick()
    {
        if (prickEnemy.currentPlayableCards.Count > 1)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2.5f));
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        prickEnemy.PlayCard();
    }

    public void StartNewMatch()
    {
        for (int i = 0; i < PrickMinigameManager.instance.playerCardGOs.Length; i++)
        {
            playerCardGOs[i].GetComponent<Button>().interactable = false;
            playerCardGOs[i].GetComponent<LayoutElement>().ignoreLayout = false;
        }

        allUnusedCards.Clear();

        currGivenCardNumber = 0;

        playerPoints = 0;
        enemyPoints = 0;

        playerPointsTxt.text = playerPoints.ToString();
        enemyPointsTxt.text = enemyPoints.ToString();

        for (int i = 0; i < allCards.Count; i++)
        {
            allUnusedCards.Add(allCards[i]);
        }

        startNewMatchBtn.interactable = false;

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
            //if (playerStartedRound)
            //{
            //    for (int i = 0; i < PrickMinigameManager.instance.playerCardGOs.Length; i++)
            //    {
            //        PrickMinigameManager.instance.playerCardGOs[i].GetComponent<Button>().interactable = true;
            //    }
            //}

            prickCardAnimator.enabled = false;

            playerStartedRound = !playerStartedRound;

            if (!playerStartedRound)
            {
                prickEnemy.StartRound();
            }
        }

        currGivenCardNumber += 1;

        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(chooseACardTutorial);
    }
}
