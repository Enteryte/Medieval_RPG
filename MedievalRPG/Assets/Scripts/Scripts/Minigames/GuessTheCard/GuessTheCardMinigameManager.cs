using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class GuessTheCardMinigameManager : MonoBehaviour
{
    public static GuessTheCardMinigameManager instance;

    public GameObject iOCanvasInteractGO;

    public Camera gTCCamera;

    public GameObject gTCUI;

    public GameObject[] cardGameObjects;
    public Sprite queenOfHeartsSprite;
    public Sprite[] cardSprites;

    public GameObject board;

    public GuessTheCardCard firstFlippedCard;

    public AnimationClip mixCardAnim;

    public TMP_Text currBetAmountTxt;
    public TMP_Text currPlayerMoneyTxt;

    public int currBetAmount = 0;

    public GameObject startGameBtn;
    public GameObject addBetAmountBtn;
    public GameObject reduceBetAmountBtn;

    [Header("Tutorial")]
    public TutorialBaseProfile welcomeTutorial;
    public TutorialBaseProfile chooseACardTutorial;

    public void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        gTCUI.SetActive(true);

        ThirdPersonController.instance.canMove = false;

        for (int i = 0; i < cardGameObjects.Length; i++)
        {
            cardGameObjects[i].transform.localPosition = new Vector3(cardGameObjects[i].transform.localPosition.x, cardGameObjects[i].transform.localPosition.y, -58);
        }

        currBetAmount = 0;

        currBetAmountTxt.text = currBetAmount.ToString();

        currPlayerMoneyTxt.text = PlayerValueManager.instance.money.ToString();

        startGameBtn.GetComponent<Button>().interactable = false;

        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(welcomeTutorial);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && startGameBtn.GetComponent<Button>().interactable)
        {
            gTCUI.SetActive(false);
            gTCCamera.enabled = false;

            ThirdPersonController.instance.canMove = true;

            GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

            this.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PlayerValueManager.instance.money == 0)
        {
            gTCUI.SetActive(false);
            gTCCamera.enabled = false;

            ThirdPersonController.instance.canMove = true;

            GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

            this.enabled = false;
        }
    }

    public void StartGame()
    {
        firstFlippedCard = null;

        for (int i = 0; i < cardGameObjects.Length; i++)
        {
            cardGameObjects[i].GetComponent<Button>().interactable = false;
            cardGameObjects[i].GetComponent<GuessTheCardCard>().isFlipped = false;
        }

        board.GetComponent<Animator>().enabled = true;
        board.GetComponent<Animator>().Play(mixCardAnim.name);

        startGameBtn.GetComponent<Button>().interactable = false;

        addBetAmountBtn.GetComponent<ChangeBetAmountButton>().enabled = false;
        reduceBetAmountBtn.GetComponent<ChangeBetAmountButton>().enabled = false;

        addBetAmountBtn.GetComponent<Button>().interactable = false;
        reduceBetAmountBtn.GetComponent<Button>().interactable = false;
    }

    public void FlipOtherCards(GuessTheCardCard flippedCard)
    {
        for (int i = 0; i < cardGameObjects.Length; i++)
        {
            if (flippedCard != cardGameObjects[i] && !cardGameObjects[i].GetComponent<GuessTheCardCard>().isFlipped)
            {
                cardGameObjects[i].GetComponent<GuessTheCardCard>().StartFlip();
            }
        }
    }

    public void ChangeCardImages()
    {
        var oldCardWQueenSpriteNumber = -1;

        for (int i = 0; i < cardGameObjects.Length; i++)
        {
            if (cardGameObjects[i].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite == queenOfHeartsSprite)
            {
                oldCardWQueenSpriteNumber = i;
            }

            cardGameObjects[i].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite = null;
        }

        var cardNumberWQueenSprite = Random.Range(0, cardGameObjects.Length);

        if (cardNumberWQueenSprite == oldCardWQueenSpriteNumber)
        {
            cardNumberWQueenSprite = Random.Range(0, cardGameObjects.Length);
        }

        cardGameObjects[cardNumberWQueenSprite].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite = queenOfHeartsSprite;

        var secondSpriteNumber = Random.Range(0, cardSprites.Length);
        bool setSecondSprite = false;

        for (int i = 0; i < cardGameObjects.Length; i++)
        {
            if (cardGameObjects[i].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite == null && !setSecondSprite)
            {
                cardGameObjects[i].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite = cardSprites[secondSpriteNumber];

                setSecondSprite = true;
            }
            else if (cardGameObjects[i].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite == null && setSecondSprite)
            {
                if (secondSpriteNumber == 0)
                {
                    cardGameObjects[i].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite = cardSprites[1];
                }
                else
                {
                    cardGameObjects[i].GetComponent<GuessTheCardCard>().cardBack.GetComponent<Image>().sprite = cardSprites[0];
                }
            }
        }
    }

    public void AfterCardMixAnimation()
    {
        ChangeCardImages();

        for (int i = 0; i < cardGameObjects.Length; i++)
        {
            cardGameObjects[i].GetComponent<Button>().interactable = true;
        }

        board.GetComponent<Animator>().enabled = false;

        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(chooseACardTutorial);
    }

    public void AddBetAmount()
    {
        if (currBetAmount + 10 <= PlayerValueManager.instance.money)
        {
            currBetAmount += 10;

            currBetAmountTxt.text = currBetAmount.ToString();
        }

        if (currBetAmount > 0)
        {
            startGameBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void ReduceBetAmount()
    {
        if (currBetAmount > 0)
        {
            currBetAmount -= 10;

            currBetAmountTxt.text = currBetAmount.ToString();
        }

        if (currBetAmount == 0)
        {
            startGameBtn.GetComponent<Button>().interactable = false;
        }
    }

    public void WonGame()
    {
        PlayerValueManager.instance.money += currBetAmount * 2;

        currPlayerMoneyTxt.text = PlayerValueManager.instance.money.ToString();

        startGameBtn.GetComponent<Button>().interactable = true;

        addBetAmountBtn.GetComponent<ChangeBetAmountButton>().enabled = true;
        reduceBetAmountBtn.GetComponent<ChangeBetAmountButton>().enabled = true;
    }

    public void LostGame()
    {
        PlayerValueManager.instance.money -= currBetAmount;

        currPlayerMoneyTxt.text = PlayerValueManager.instance.money.ToString();

        if (currBetAmount > PlayerValueManager.instance.money && PlayerValueManager.instance.money > 10)
        {
            currBetAmount = (int)PlayerValueManager.instance.money;

            currBetAmountTxt.text = currBetAmount.ToString();

            startGameBtn.GetComponent<Button>().interactable = true;
        }
        else if (currBetAmount > PlayerValueManager.instance.money && PlayerValueManager.instance.money < 10)
        {
            currBetAmount = 0;

            currBetAmountTxt.text = currBetAmount.ToString();

            startGameBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            startGameBtn.GetComponent<Button>().interactable = true;
        }

        addBetAmountBtn.GetComponent<ChangeBetAmountButton>().enabled = true;
        reduceBetAmountBtn.GetComponent<ChangeBetAmountButton>().enabled = true;
    }

    public void OnDisable()
    {
        for (int i = 0; i < cardGameObjects.Length; i++)
        {
            cardGameObjects[i].transform.localPosition = new Vector3(cardGameObjects[i].transform.localPosition.x, cardGameObjects[i].transform.localPosition.y, 0);
        }
    }
}
