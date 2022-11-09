using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrickEnemy : MonoBehaviour
{
    public List<PrickCardBase> currentPlayableCards/* = new List<PrickCard>()*/;

    public void StartRound()
    {
        StartCoroutine(WaitTillPlayFirstCard());
    }

    IEnumerator WaitTillPlayFirstCard()
    {
        yield return new WaitForSeconds(1.5f);

        var newPlayedCardNumber = Random.Range(0, currentPlayableCards.Count);

        LayCardDown(currentPlayableCards[newPlayedCardNumber]);

        currentPlayableCards.Remove(currentPlayableCards[newPlayedCardNumber]);
    }

    public void PlayCard()
    {
        for (int i = 0; i < currentPlayableCards.Count; i++)
        {
            if (currentPlayableCards[i].cardColor == PrickMinigameManager.instance.layedPlayerCB.cardColor)
            {
                if (currentPlayableCards[i].cardNumber < PrickMinigameManager.instance.layedPlayerCB.cardNumber)
                {
                    LayCardDown(currentPlayableCards[i]);

                    currentPlayableCards.Remove(currentPlayableCards[i]);

                    return;
                }
            }
        }

        for (int i = 0; i < currentPlayableCards.Count; i++)
        {
            if (currentPlayableCards[i].cardColor == PrickMinigameManager.instance.layedPlayerCB.cardColor)
            {
                if (currentPlayableCards[i].cardNumber > PrickMinigameManager.instance.layedPlayerCB.cardNumber)
                {
                    LayCardDown(currentPlayableCards[i]);

                    currentPlayableCards.Remove(currentPlayableCards[i]);

                    return;
                }
            }
        }

        for (int i = 0; i < currentPlayableCards.Count; i++)
        {
            if (currentPlayableCards[i].cardColor != PrickMinigameManager.instance.layedPlayerCB.cardColor)
            {
                if (currentPlayableCards[i].cardNumber >= PrickMinigameManager.instance.layedPlayerCB.cardNumber)
                {
                    LayCardDown(currentPlayableCards[i]);

                    currentPlayableCards.Remove(currentPlayableCards[i]);

                    return;
                }
            }
        }

        for (int i = 0; i < currentPlayableCards.Count; i++)
        {
            if (currentPlayableCards[i].cardColor != PrickMinigameManager.instance.layedPlayerCB.cardColor)
            {
                if (currentPlayableCards[i].cardNumber <= PrickMinigameManager.instance.layedPlayerCB.cardNumber)
                {
                    LayCardDown(currentPlayableCards[i]);

                    currentPlayableCards.Remove(currentPlayableCards[i]);

                    return;
                }
            }
        }

        //PrickMinigameManager.instance.EndRound();
    }

    public void LayCardDown(PrickCardBase pCB)
    {
        //PrickMinigameManager.instance.layedEnemyCardObj.GetComponent<PrickCard>().cardBack.SetActive(false);
        PrickMinigameManager.instance.layedEnemyCB = pCB;

        PrickMinigameManager.instance.layedEnemyCardObj.GetComponent<PrickCard>().pCB = pCB;
        PrickMinigameManager.instance.layedEnemyCardObj.GetComponent<Image>().sprite = pCB.cardSprite;

        PrickMinigameManager.instance.layerEnemyCardObjMiddle.GetComponent<PrickCard>().pCB = pCB;
        PrickMinigameManager.instance.layerEnemyCardObjMiddle.GetComponent<PrickCard>().cardBack.GetComponent<Image>().sprite = pCB.cardSprite;

        PrickMinigameManager.instance.prickCardAnimator.enabled = true;

        if (PrickMinigameManager.instance.playerStartedRound)
        {
            PrickMinigameManager.instance.prickCardAnimator.Play(PrickMinigameManager.instance.layEnemyCardAnim.name);

            Debug.Log(PrickMinigameManager.instance.layEnemyCardAnim.name);
        }
        else
        {
            PrickMinigameManager.instance.prickCardAnimator.Play(PrickMinigameManager.instance.enemyLayCardFirstAnim.name);

            Debug.Log(PrickMinigameManager.instance.enemyLayCardFirstAnim.name);
        }

        PrickMinigameManager.instance.enemyCardGOs[currentPlayableCards.Count - 1].SetActive(false);
    }

    public void ActivatePlayerCards()
    {
        for (int i = 0; i < PrickMinigameManager.instance.playerCardGOs.Length; i++)
        {
            PrickMinigameManager.instance.playerCardGOs[i].GetComponent<Button>().interactable = true;
        }
    }
}
