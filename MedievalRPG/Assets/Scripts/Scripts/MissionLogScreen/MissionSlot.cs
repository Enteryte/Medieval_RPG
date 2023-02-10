using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionSlot : MonoBehaviour
{
    public TMP_Text missionNameTxt;
    public TMP_Text whereToDoMissionTxt;

    public Image sideColorImg;
    public Image hoverGradientImg;

    public Image missionSymbolImg;

    public Image boarder;

    public MissionBaseProfile corrMBP;

    public Image isSelectedImg;

    public void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(DisplayInformation);
    }

    public void SetMissionInfo(MissionBaseProfile mBP)
    {
        corrMBP = mBP;

        missionNameTxt.text = mBP.missionName;
        whereToDoMissionTxt.text = "";

        if (mBP.missionType == MissionBaseProfile.MissionType.main)
        {
            //sideColorImg.color = MissionLogScreenHandler.instance.mainMissionColor;
            //sideColorImg.gameObject.GetComponent<Outline>().effectColor = MissionLogScreenHandler.instance.mainMissionOutlineColor;

            //hoverGradientImg.color = MissionLogScreenHandler.instance.mainMissionColor;
            //hoverGradientImg.gameObject.GetComponent<Outline>().effectColor = MissionLogScreenHandler.instance.mainMissionOutlineColor;

            missionSymbolImg.sprite = MissionLogScreenHandler.instance.mainMissionSprite;
        }
        else
        {
            //sideColorImg.color = MissionLogScreenHandler.instance.sideMissionColor;
            //sideColorImg.gameObject.GetComponent<Outline>().effectColor = MissionLogScreenHandler.instance.sideMissionOutlineColor;

            //hoverGradientImg.color = MissionLogScreenHandler.instance.sideMissionColor;
            //hoverGradientImg.gameObject.GetComponent<Outline>().effectColor = MissionLogScreenHandler.instance.sideMissionOutlineColor;

            missionSymbolImg.sprite = MissionLogScreenHandler.instance.sideMissionSprite;
        }

        if (UIManager.missionToDisplay == corrMBP)
        {
            isSelectedImg.gameObject.SetActive(true);
        }
        else
        {
            isSelectedImg.gameObject.SetActive(false);
        }
    }

    public void DisplayInformation()
    {
        MissionLogScreenHandler.currClickedMS = this;

        MissionLogScreenHandler.instance.missionNameTxt.text = corrMBP.missionName;
        MissionLogScreenHandler.instance.whereToDoMissionTxt.text = "";

        MissionLogScreenHandler.instance.missionSymbolImg.sprite = missionSymbolImg.sprite;

        MissionLogScreenHandler.instance.missionSymbolImg.gameObject.GetComponent<Outline>().effectColor = sideColorImg.gameObject.GetComponent<Outline>().effectColor;
        MissionLogScreenHandler.instance.backgroundGradientImg.color = hoverGradientImg.color;
        MissionLogScreenHandler.instance.boarderImg.color = sideColorImg.gameObject.GetComponent<Outline>().effectColor;

        MissionLogScreenHandler.instance.missionDescriptionTxt.text = corrMBP.missionDescription;

        for (int i = 0; i < MissionLogScreenHandler.instance.missionTaskParentObj.transform.childCount; i++)
        {
            Destroy(MissionLogScreenHandler.instance.missionTaskParentObj.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < corrMBP.allMissionTasks.Length; i++)
        {
            if (corrMBP.allMissionTasks[i].mTB.canBeDisplayed || corrMBP.allMissionTasks[i].mTB.missionTaskCompleted)
            {
                var newMissionTaskSlot = Instantiate(MissionLogScreenHandler.instance.missionTaskSlotPrefab, MissionLogScreenHandler.instance.missionTaskParentObj.transform);

                newMissionTaskSlot.GetComponent<MissionTaskSlot>().DisplayTaskInformation(corrMBP.allMissionTasks[i]);
            }
        }

        if (UIManager.missionToDisplay == corrMBP)
        {
            Debug.Log(MissionLogScreenHandler.instance.sMButton);
            MissionLogScreenHandler.instance.sMButton.selectedImg.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(MissionLogScreenHandler.instance.sMButton);
            MissionLogScreenHandler.instance.sMButton.selectedImg.gameObject.SetActive(false);
        }
    }
}
