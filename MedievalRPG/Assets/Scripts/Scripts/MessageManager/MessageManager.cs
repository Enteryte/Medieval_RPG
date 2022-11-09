using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;

    [Header("Collected Something Message")]
    public GameObject collectedMessageObjPrefab;
    public GameObject collectedMessageParentObj;

    public void Awake()
    {
        instance = this;
    }

    public void CreateCollectedMessage(ItemBaseProfile collectedItemBP)
    {
        GameObject newCollMessageObj = Instantiate(collectedMessageObjPrefab, collectedMessageParentObj.transform);

        newCollMessageObj.GetComponent<CollectedMessageObject>().itemNameTxt.text = collectedItemBP.itemName;
        newCollMessageObj.GetComponent<CollectedMessageObject>().itemSpriteImg.sprite = collectedItemBP.itemSprite;
    }
}
