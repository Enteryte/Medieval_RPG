using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager instance;

    public Camera minimapCam;

    public GameObject minimapBoarder;

    public List<MinimapIcon> allMinimapIcons = new List<MinimapIcon>();

    public Color32 npcSymbolIsSelectedColor;

    public Color32 hasMissionToAcceptColor;
    public Color32 hasMissionToCompleteColor;

    [Header("Merchants")]
    public Sprite normalMerchantSprite;
    public Sprite normalHealerSprite;

    [Header("TavernKeeper")]
    public Sprite normalTavernKeeperSprite;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var rot = minimapCam.transform.rotation;
        rot.x = 0;
        rot.y = 0;

        minimapBoarder.transform.rotation = rot;
    }

    public void SetHasUnacceptedMissionText(MinimapIcon minimapIcon)
    {
        minimapIcon.exclamationMarkImg.SetActive(false);
        minimapIcon.questionMarkImg.SetActive(true);

        //if (UIManager.missionToDisplay != null && minimapIcon.currCorrMissions.Contains(UIManager.missionToDisplay))
        //{
        //    minimapIcon.exclamationMarkImg.GetComponent<Image>().color = minimapIcon.symbolIsSelectedColor;
        //}
        //else
        //{
        //    minimapIcon.exclamationMarkImg.GetComponent<Image>().color = Color.white;
        //}

        if (minimapIcon.corrNPCMissionSymbol != null)
        {
            minimapIcon.corrNPCMissionSymbol.exclaImg.SetActive(false);
            minimapIcon.corrNPCMissionSymbol.questionImg.SetActive(true);
        }

        if (UIManager.missionToDisplay != null && minimapIcon.currCorrMissions.Contains(UIManager.missionToDisplay))
        {
            minimapIcon.questionMarkImg.GetComponent<Image>().color = minimapIcon.symbolIsSelectedColor;

            if (minimapIcon.corrNPCMissionSymbol != null)
            {
                minimapIcon.corrNPCMissionSymbol.questionImg.GetComponent<Image>().color = MinimapManager.instance.npcSymbolIsSelectedColor;
            }
        }
        else
        {
            minimapIcon.questionMarkImg.GetComponent<Image>().color = Color.white;

            if (minimapIcon.corrNPCMissionSymbol != null)
            {
                minimapIcon.corrNPCMissionSymbol.questionImg.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void SetHasTaskToCompleteMissionText(MinimapIcon minimapIcon)
    {
        minimapIcon.exclamationMarkImg.SetActive(true);
        minimapIcon.questionMarkImg.SetActive(false);

        //if (UIManager.missionToDisplay != null && minimapIcon.currCorrMissions.Contains(UIManager.missionToDisplay))
        //{
        //    minimapIcon.exclamationMarkImg.GetComponent<Image>().color = minimapIcon.symbolIsSelectedColor;
        //}
        //else
        //{
        //    minimapIcon.exclamationMarkImg.GetComponent<Image>().color = Color.white;
        //}

        if (minimapIcon.corrNPCMissionSymbol != null)
        {
            minimapIcon.corrNPCMissionSymbol.exclaImg.SetActive(true);
            minimapIcon.corrNPCMissionSymbol.questionImg.SetActive(false);
        }

        if (UIManager.missionToDisplay != null && minimapIcon.currCorrMissions.Contains(UIManager.missionToDisplay))
        {
            minimapIcon.exclamationMarkImg.GetComponent<Image>().color = minimapIcon.symbolIsSelectedColor;

            if (minimapIcon.corrNPCMissionSymbol != null)
            {
                minimapIcon.corrNPCMissionSymbol.exclaImg.GetComponent<Image>().color = MinimapManager.instance.npcSymbolIsSelectedColor;
            }
        }
        else
        {
            minimapIcon.exclamationMarkImg.GetComponent<Image>().color = Color.white;

            if (minimapIcon.corrNPCMissionSymbol != null)
            {
                minimapIcon.corrNPCMissionSymbol.exclaImg.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void CheckAllMinimapSymbols()
    {
        for (int i = 0; i < MinimapManager.instance.allMinimapIcons.Count; i++)
        {
            for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions.Count; y++)
            {
                if (y == 0)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allCurrAcceptedMissions[y], true, false);
                }
                else if (y == MissionManager.instance.allCurrAcceptedMissions.Count)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allCurrAcceptedMissions[y], false, true);
                }
                else
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allCurrAcceptedMissions[y], false, false);
                }
            }

            for (int y = 0; y < MissionManager.instance.allCurrOpenNotAcceptedMissions.Count; y++)
            {
                if (y == 0)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allCurrOpenNotAcceptedMissions[y], false, false);
                }
                else if (y == MissionManager.instance.allCurrAcceptedMissions.Count)
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allCurrOpenNotAcceptedMissions[y], false, true);
                }
                else
                {
                    MinimapManager.instance.allMinimapIcons[i].CheckIfIsNeededForMission(MissionManager.instance.allCurrOpenNotAcceptedMissions[y], false, false);
                }
            }
        }
    }
}
