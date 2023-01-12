using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    public GameObject targetEnemyCanvasObj;

    public GameObject currTargetEnemy;

    //public int CurrArrowAmount
    //{
    //    get
    //    {
    //        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
    //        {
    //            if (InventoryManager.instance.inventory.slots[i].itemBase == FightManager.instance.arrowIB)
    //            {
    //                return InventoryManager.instance.inventory.slots[i].itemAmount;
    //            }
    //        }

    //        return 0;
    //    }
    //    set
    //    {
    //        currArrowCountTxt.text = CurrArrowAmount.ToString();

    //        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
    //        {
    //            if (InventoryManager.instance.inventory.slots[i].itemBase == FightManager.instance.arrowIB)
    //            {
    //                return InventoryManager.instance.inventory.slots[i].itemAmount;
    //            }
    //        }
    //    }
    //}

    [Header("Arrows")]
    public ItemBaseProfile arrowIB;

    public TMP_Text currArrowCountTxt;
    public Image arrowHUDImg;
    public Image arrowHUDBackgroundImg;

    public int currArrowAmount;

    [Header("Tutorials")]
    public TutorialBaseProfile doARollTutorial;
    public TutorialBaseProfile shildBlockTutorial;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currTargetEnemy != null)
        {
            if (Vector3.Distance(currTargetEnemy.transform.position, GameManager.instance.playerGO.transform.position) 
                > GameManager.instance.playerGO.GetComponent<Interacting>().viewRadius)
                             {
                currTargetEnemy = null;

                targetEnemyCanvasObj.SetActive(false);
            }
        }
    }

    public void UpdateArrowHUDDisplay()
    {
        arrowHUDImg.color = Color.red;
        arrowHUDBackgroundImg.color = Color.red;

        currArrowCountTxt.text = "0";

        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            if (InventoryManager.instance.inventory.slots[i].itemBase == arrowIB)
            {
                currArrowCountTxt.text = InventoryManager.instance.inventory.slots[i].itemAmount.ToString();

                arrowHUDImg.color = Color.white;
                arrowHUDBackgroundImg.color = Color.white;
            }
        }
    }

    public void TargetEnemy(GameObject currInteractTarget)
    {
        if (currTargetEnemy == currInteractTarget)
        {
            currTargetEnemy = null;

            targetEnemyCanvasObj.SetActive(false);
        }
        else
        {
            currTargetEnemy = currInteractTarget;

            targetEnemyCanvasObj.SetActive(true);
        }
    }
}
