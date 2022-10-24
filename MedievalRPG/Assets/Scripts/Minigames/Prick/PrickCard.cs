using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrickCard : MonoBehaviour
{
    public PrickCardBase pCB;

    public GameObject cardBack;

    public bool isInteractableCard = true;

    public Coroutine currCoro;

    public Vector3 cardUpPos;
    public Vector3 cardDownPos;

    public GameObject emptyCard;

    public float speed = 0;

    public void Start()
    {
        if (isInteractableCard)
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(LayCardDown);
        }
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C) && currCoro == null)
        //{
        //    StartCardUp();
        //}

        //if (Input.GetKeyDown(KeyCode.V) && currCoro == null)
        //{
        //    StartCardDown();
        //}
    }

    public void StartCardUp()
    {
        if (isInteractableCard)
        {
            emptyCard.SetActive(true);

            this.gameObject.GetComponent<LayoutElement>().ignoreLayout = true;

            if (currCoro != null)
            {
                StopCoroutine(currCoro);
            }

            currCoro = StartCoroutine(DoCardUp());
        }
    }

    public void StartCardDown()
    {
        if (isInteractableCard)
        {
            emptyCard.SetActive(false);

            this.gameObject.GetComponent<LayoutElement>().ignoreLayout = false;

            if (currCoro != null)
            {
                StopCoroutine(currCoro);
            }

            currCoro = StartCoroutine(DoCardDown());
        }
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

    IEnumerator DoCardUp()
    {
        while (speed < 0.5f)
        {
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, Mathf.Lerp(this.gameObject.transform.localPosition.y, cardUpPos.y, speed), 
                this.gameObject.transform.localPosition.z);

            speed += 0.5f * Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }

        currCoro = null;
        speed = 0;
    }

    IEnumerator DoCardDown()
    {
        while (speed < 0.5f)
        {
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, Mathf.Lerp(this.gameObject.transform.localPosition.y, cardDownPos.y, speed), 
                this.gameObject.transform.localPosition.z);

            speed += 0.5f * Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }

        currCoro = null;
        speed = 0;
    }

    public void LayCardDown()
    {
        for (int i = 0; i < PrickMinigameManager.instance.playerCardGOs.Length; i++)
        {
            PrickMinigameManager.instance.playerCardGOs[i].GetComponent<Button>().interactable = false;
        }

        PrickMinigameManager.instance.layedPlayerCB = pCB;

        emptyCard.SetActive(false);
        this.gameObject.SetActive(false);

        //PrickMinigameManager.instance.layedPlayerCardObj.GetComponent<PrickCard>().cardBack.SetActive(false);

        PrickMinigameManager.instance.layedPlayerCardObj.GetComponent<PrickCard>().pCB = pCB;
        PrickMinigameManager.instance.layedPlayerCardObj.GetComponent<Image>().sprite = pCB.cardSprite;

        PrickMinigameManager.instance.layerPlayerCardObjMiddle.GetComponent<PrickCard>().pCB = pCB;
        PrickMinigameManager.instance.layerPlayerCardObjMiddle.GetComponent<PrickCard>().cardBack.GetComponent<Image>().sprite = pCB.cardSprite;

        PrickMinigameManager.instance.prickCardAnimator.enabled = true;

        if (PrickMinigameManager.instance.playerStartedRound)
        {
            PrickMinigameManager.instance.prickCardAnimator.Play(PrickMinigameManager.instance.layPlayerCardAnim.name);
        }
        else
        {
            PrickMinigameManager.instance.prickCardAnimator.Play(PrickMinigameManager.instance.playerLayCardSecondAnim.name);
        }
    }
}
