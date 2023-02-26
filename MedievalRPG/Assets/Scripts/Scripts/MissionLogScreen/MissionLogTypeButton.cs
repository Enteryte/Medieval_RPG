using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionLogTypeButton : MonoBehaviour /*IPointerClickHandler, IPointerEnterHandler*/
{
    public GameObject arrowLookingDown;
    public GameObject arrowLookingUp;

    public GameObject buttonParentObj;

    public AudioClip showMissionsAC;
    public AudioClip dontShowMissionsAC;

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

        if (arrowLookingDown.activeSelf)
        {
            GameManager.instance.SetUIAudioOneShot(showMissionsAC);
        }
        else
        {
            GameManager.instance.SetUIAudioOneShot(dontShowMissionsAC);
        }
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if ()
    //}
}
