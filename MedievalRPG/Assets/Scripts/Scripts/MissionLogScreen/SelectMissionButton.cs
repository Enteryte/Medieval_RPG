using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMissionButton : MonoBehaviour
{
    public Image selectedImg;

    // Start is called before the first frame update
    void Start()
    {
        if (selectedImg != null)
        {
            MissionLogScreenHandler.instance.sMButton = this;
        }
    }

    public void SelectMissionToDisplay()
    {
        UIManager.missionToDisplay = MissionLogScreenHandler.currClickedMS.corrMBP;

        selectedImg.gameObject.SetActive(true);
        MissionLogScreenHandler.currClickedMS.isSelectedImg.gameObject.SetActive(true);

        MissionLogScreenHandler.instance.mainMissionSlot.SetMissionInfo(MissionLogScreenHandler.instance.mainMissionSlot.corrMBP);

        for (int i = 0; i < MissionLogScreenHandler.instance.sideMissionSlotParentObj.transform.childCount; i++)
        {
            MissionLogScreenHandler.instance.sideMissionSlotParentObj.transform.GetChild(i).GetComponent<MissionSlot>().SetMissionInfo(
                MissionLogScreenHandler.instance.sideMissionSlotParentObj.transform.GetChild(i).GetComponent<MissionSlot>().corrMBP);
        }

        UIManager.instance.CreateMissionDisplay();

        for (int i = 0; i < MinimapManager.instance.allMinimapIcons.Count; i++)
        {
            for (int y = 0; y < MissionManager.instance.allMissions.Count; y++)
            {
                if (y == 0)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allMissions[y], true, false);
                }
                else if (y == MissionManager.instance.allMissions.Count)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allMissions[y], false, true);
                }
                else
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allMissions[y], false, false);
                }
            }
        }
    }

    public void DeselectMissionToDisplay()
    {
        UIManager.missionToDisplay = null;

        selectedImg.gameObject.SetActive(false);
        MissionLogScreenHandler.currClickedMS.isSelectedImg.gameObject.SetActive(false);

        MissionLogScreenHandler.instance.mainMissionSlot.SetMissionInfo(MissionLogScreenHandler.instance.mainMissionSlot.corrMBP);

        for (int i = 0; i < MissionLogScreenHandler.instance.sideMissionSlotParentObj.transform.childCount; i++)
        {
            MissionLogScreenHandler.instance.sideMissionSlotParentObj.transform.GetChild(i).GetComponent<MissionSlot>().SetMissionInfo(
                MissionLogScreenHandler.instance.sideMissionSlotParentObj.transform.GetChild(i).GetComponent<MissionSlot>().corrMBP);
        }

        UIManager.instance.CreateMissionDisplay();

        for (int i = 0; i < MinimapManager.instance.allMinimapIcons.Count; i++)
        {
            for (int y = 0; y < MissionManager.instance.allMissions.Count; y++)
            {
                if (y == 0)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allMissions[y], true, false);
                }
                else if (y == MissionManager.instance.allMissions.Count)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allMissions[y], false, true);
                }
                else
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allMissions[y], false, false);
                }
            }
        }
    }
}
