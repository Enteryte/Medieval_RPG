using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionLogTypeButton : MonoBehaviour
{
    public GameObject arrowLookingDown;
    public GameObject arrowLookingUp;

    public GameObject buttonParentObj;

    public void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(DisplayAllButtonOfType);
    }

    public void DisplayAllButtonOfType()
    {
        arrowLookingDown.SetActive(!arrowLookingDown.activeSelf);
        arrowLookingUp.SetActive(!arrowLookingUp.activeSelf);

        if (buttonParentObj != null)
        {
            for (int i = 0; i < buttonParentObj.transform.childCount; i++)
            {
                buttonParentObj.transform.GetChild(i).gameObject.SetActive(arrowLookingDown.activeSelf);
            }
        }
        else
        {
            if (MissionLogScreenHandler.instance.mainMissionSlot.corrMBP != null)
            {
                MissionLogScreenHandler.instance.mainMissionSlot.gameObject.SetActive(arrowLookingDown.activeSelf);
            }
            else
            {
                MissionLogScreenHandler.instance.mainMissionSlot.gameObject.SetActive(false);
            }
        }
    }
}
