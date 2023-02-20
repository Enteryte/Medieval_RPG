using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;

    [Header("Collected Something Message")]
    public GameObject collectedMessageObjPrefab;
    public GameObject collectedMessageParentObj;

    [Header("If Is Money")]
    public Sprite isMoneySprite;
    public AudioClip collectedMoneyAC;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CreateCollectedMessage(ItemBaseProfile collectedItemBP)
    {
        GameObject newCollMessageObj = Instantiate(collectedMessageObjPrefab, collectedMessageParentObj.transform);

        newCollMessageObj.GetComponent<CollectedMessageObject>().itemNameTxt.text = collectedItemBP.itemName;
        newCollMessageObj.GetComponent<CollectedMessageObject>().itemSpriteImg.sprite = collectedItemBP.itemSprite;
    }

    public void CreateCollectedMessage(int moneyAmount)
    {
        GameObject newCollMessageObj = Instantiate(collectedMessageObjPrefab, collectedMessageParentObj.transform);

        newCollMessageObj.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);

        newCollMessageObj.GetComponent<CollectedMessageObject>().itemNameTxt.text = moneyAmount.ToString();
        newCollMessageObj.GetComponent<CollectedMessageObject>().itemSpriteImg.sprite = isMoneySprite;
    }
}
