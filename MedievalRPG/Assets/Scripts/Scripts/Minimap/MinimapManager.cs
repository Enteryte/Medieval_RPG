using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager instance;

    public Camera minimapCam;

    public GameObject minimapBoarder;

    public List<MinimapIcon> allMinimapIcons = new List<MinimapIcon>();

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
        minimapIcon.mMIconMissionTxt.text = "!";
    }

    public void SetHasTaskToCompleteMissionText(MinimapIcon minimapIcon)
    {
        minimapIcon.mMIconMissionTxt.text = "?";
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
