using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessTheCardCard : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    public GameObject cardBack;

    public bool isFlipped = false;

    //public bool cardBackIsActive = false;

    public float timer;

    public void Update()
    {

    }

    public void StartFlip()
    {
        isFlipped = true;

        StartCoroutine(CalculateFlip());
    }

    //public void FlipCard()
    //{
    //    if (!cardBackIsActive)
    //    {
    //        cardBack.gameObject.SetActive(false);
    //        cardBackIsActive = false;
    //    }
    //    else
    //    {
    //        cardBack.gameObject.SetActive(true);
    //        cardBackIsActive = true;
    //    }
    //}

    IEnumerator CalculateFlip()
    {
        for (int i = 0; i < GuessTheCardMinigameManager.instance.cardGameObjects.Length; i++)
        {
            GuessTheCardMinigameManager.instance.cardGameObjects[i].GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.00001f);

            transform.Rotate(new Vector3(x, y, z));

            timer += 1;

            if (timer >= 12.5/* || timer == -12.5*/)
            {
                cardBack.gameObject.SetActive(true);
                //cardBackIsActive = true;
            }
        }

        timer = 0;

        if (GuessTheCardMinigameManager.instance.firstFlippedCard == null)
        {
            GuessTheCardMinigameManager.instance.firstFlippedCard = this;

            GuessTheCardMinigameManager.instance.FlipOtherCards(this);

            if (GuessTheCardMinigameManager.instance.firstFlippedCard.cardBack.GetComponent<Image>().sprite == GuessTheCardMinigameManager.instance.queenOfHeartsSprite)
            {
                GuessTheCardMinigameManager.instance.WonGame();
            }
            else
            {
                GuessTheCardMinigameManager.instance.LostGame();
            }
        }
        else
        {
            var notAllFlipped = false;

            for (int i = 0; i < GuessTheCardMinigameManager.instance.cardGameObjects.Length; i++)
            {
                if (!GuessTheCardMinigameManager.instance.cardGameObjects[i].GetComponent<GuessTheCardCard>().isFlipped)
                {
                    notAllFlipped = true;
                }
            }

            if (!notAllFlipped)
            {
                GuessTheCardMinigameManager.instance.startGameBtn.SetActive(true);
            }
        }
    }
}
