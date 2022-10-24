using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrickEnemy : MonoBehaviour
{
    public List<PrickCardBase> currentPlayableCards/* = new List<PrickCard>()*/;

    public void PlayCard()
    {
        for (int i = 0; i < currentPlayableCards.Count; i++)
        {
            if (currentPlayableCards[i].cardNumber < PrickMinigameManager.instance.layedPlayerCB.cardNumber)
            {
                LayCardDown(currentPlayableCards[i]);

                currentPlayableCards.Remove(currentPlayableCards[i]);
            }
        }
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
        PrickMinigameManager.instance.prickCardAnimator.Play(PrickMinigameManager.instance.layEnemyCardAnim.name);
    }
}
