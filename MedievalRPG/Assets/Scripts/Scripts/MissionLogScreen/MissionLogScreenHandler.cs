using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionLogScreenHandler : MonoBehaviour
{
    public static MissionLogScreenHandler instance;

    public MissionManager missionManager;

    public GameObject missionSlotPrefab;

    public static MissionSlot currClickedMS;

    [Header("Mission Task Display")]
    public GameObject missionTaskParentObj;
    public GameObject missionTaskSlotPrefab;

    public TMP_Text missionNameTxt;
    public TMP_Text whereToDoMissionTxt;

    public Image missionSymbolImg;

    public Image backgroundGradientImg;
    public Image boarderImg;

    [Header("Mission Description Display")]
    public TMP_Text missionDescriptionTxt;

    [Header("MainQuest")]
    public MissionSlot mainMissionSlot;

    public Color32 mainMissionColor;
    public Color32 mainMissionOutlineColor;

    public Color32 mainMissionHoverColor;

    public Sprite mainMissionSprite;

    public MissionLogTypeButton mainMLTBtn;

    [Header("SideQuests")]
    public GameObject sideMissionSlotParentObj;
    public List<MissionSlot> allSideQuestSlots = new List<MissionSlot>();

    public Color32 sideMissionColor;
    public Color32 sideMissionOutlineColor;

    public Color32 sideMissionHoverColor;

    public Sprite sideMissionSprite;

    public MissionLogTypeButton sideMLTButton;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        missionManager = MissionManager.instance;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    DisplayMissions();
        //}
    }

    public void DisplayMissions()
    {
        currClickedMS = null;

        mainMissionSlot.gameObject.SetActive(false);

        for (int i = 0; i < sideMissionSlotParentObj.transform.childCount; i++)
        {
            Destroy(sideMissionSlotParentObj.transform.GetChild(i).gameObject);
        }

        if (missionManager == null)
        {
            missionManager = MissionManager.instance;
        }

        for (int i = 0; i < missionManager.allCurrAcceptedMissions.Count; i++)
        {
            if (missionManager.allCurrAcceptedMissions[i].missionType == MissionBaseProfile.MissionType.main)
            {
                mainMissionSlot.SetMissionInfo(missionManager.allCurrAcceptedMissions[i]);

                if (mainMissionSlot.corrMBP != null)
                {
                    mainMissionSlot.gameObject.SetActive(mainMLTBtn.arrowLookingDown.activeSelf);

                    mainMissionSlot.hoverGradientImg.gameObject.SetActive(true);
                    mainMissionSlot.DisplayInformation();
                }
                else
                {
                    mainMissionSlot.gameObject.SetActive(false);
                }
            }
            else
            {
                var newSideMissionSlot = Instantiate(missionSlotPrefab, sideMissionSlotParentObj.transform);

                newSideMissionSlot.GetComponent<MissionSlot>().SetMissionInfo(missionManager.allCurrAcceptedMissions[i]);

                newSideMissionSlot.gameObject.SetActive(sideMLTButton.arrowLookingDown.activeSelf);

                if (currClickedMS == null)
                {
                    newSideMissionSlot.GetComponent<MissionSlot>().hoverGradientImg.gameObject.SetActive(true);
                    newSideMissionSlot.GetComponent<MissionSlot>().DisplayInformation();
                }

                Debug.Log("drfgvhbjnm");
            }
        }
    }
}
