using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinimapIcon : MonoBehaviour
{
    public Camera minimapCam;

    public NPCMissionSymbol corrNPCMissionSymbol;

    public TMP_Text mMIconMissionTxt;

    public Merchant corrMerchant;
    public TavernKeeper corrTavernKeeper;
    public NPC corrNPC;
    public Item corrItem;

    public List<MissionBaseProfile> currCorrMissions = new List<MissionBaseProfile>();

    public bool isDoorOrGoToIcon = false;
    public List<MissionTaskBase> corrMissionTasks;

    //// Start is called before the first frame update
    void Start()
    {
        MinimapManager.instance.allMinimapIcons.Add(this);

        minimapCam = MinimapManager.instance.minimapCam;

        mMIconMissionTxt = GetComponentInChildren<TextMeshProUGUI>();

        corrMerchant = this.gameObject.GetComponentInParent<Merchant>();
        corrTavernKeeper = this.gameObject.GetComponentInParent<TavernKeeper>();
        corrItem = this.gameObject.GetComponentInParent<Item>();
        corrNPC = this.gameObject.GetComponentInParent<NPC>();

        mMIconMissionTxt.text = "";

        if (!isDoorOrGoToIcon)
        {
            corrNPCMissionSymbol = this.gameObject.transform.parent.GetComponentInChildren<NPCMissionSymbol>();
        }

        //for (int i = 0; i < MinimapManager.instance.allMinimapIcons.Count; i++)
        ////{
        //    for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions.Count; y++)
        //    {
        //        CheckIfIsNeededForMission(MissionManager.instance.allCurrAcceptedMissions[y]);
        //    }

        //    for (int y = 0; y < MissionManager.instance.allCurrOpenNotAcceptedMissions.Count; y++)
        //    {
        //        CheckIfIsNeededForMission(MissionManager.instance.allCurrOpenNotAcceptedMissions[y]);
        //    }
        //}

        //MinimapManager.instance.CheckAllMinimapSymbols();
    }

    // Update is called once per frame
    void Update()
    {
        var rot = minimapCam.transform.rotation;
        rot.x = 90;

        transform.rotation = (minimapCam.transform.rotation);
    }

    public void CheckIfIsNeededForMission(MissionBaseProfile mBToCheck, bool isFirst, bool isLast)
    {
        if (isFirst)
        {
            mMIconMissionTxt.text = "";

            if (!isDoorOrGoToIcon)
            {
                currCorrMissions.Clear();
            }
        }
        //else
        //{
        //    if (currCorrMissions.Contains(mBToCheck))
        //    {
        //        currCorrMissions.Remove(mBToCheck);
        //    }
        //}

        if (corrNPC == null)
        {
            if (!mBToCheck.missionCompleted/* && mBToCheck.isActive*/)
            {
                for (int i = 0; i < mBToCheck.allMissionTasks.Length; i++)
                {
                    if (mBToCheck.allMissionTasks[i].mTB.canBeDisplayed && !mBToCheck.allMissionTasks[i].mTB.missionTaskCompleted)
                    {
                        if (mBToCheck.allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
                        {
                            if (mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile != null)
                            {
                                if (corrMerchant != null && mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile == corrMerchant.nPCBP)
                                {
                                    currCorrMissions.Add(mBToCheck);

                                    SetMissionSymbolToHasSomethingToComplete();
                                }
                                else if (corrTavernKeeper != null && mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile == corrTavernKeeper.nPCBP)
                                {
                                    currCorrMissions.Add(mBToCheck);

                                    SetMissionSymbolToHasSomethingToComplete();
                                }
                                else if (corrNPC != null && mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile == corrNPC.nPCBP)
                                {
                                    currCorrMissions.Add(mBToCheck);

                                    SetMissionSymbolToHasSomethingToComplete();
                                }
                            }
                        }
                        else if (isDoorOrGoToIcon && currCorrMissions.Contains(mBToCheck) && corrMissionTasks.Contains(mBToCheck.allMissionTasks[i].mTB))
                        {
                            //currCorrMissions.Add(mBToCheck);

                            SetMissionSymbolToHasSomethingToComplete();
                        }
                        //else if (mBToCheck.allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.)
                    }
                }

                if (!mBToCheck.isActive)
                {
                    if (mBToCheck.nPCWhereToGetMissionFrom != null)
                    {
                        if (corrMerchant != null && mBToCheck.nPCWhereToGetMissionFrom == corrMerchant.nPCBP)
                        {
                            currCorrMissions.Add(mBToCheck);

                            SetMissionSymbolToHasSomethingToAccept();
                        }
                        else if (corrTavernKeeper != null && mBToCheck.nPCWhereToGetMissionFrom == corrTavernKeeper.nPCBP)
                        {
                            currCorrMissions.Add(mBToCheck);

                            SetMissionSymbolToHasSomethingToAccept();
                        }
                        else if (corrNPC != null && mBToCheck.nPCWhereToGetMissionFrom == corrNPC.nPCBP)
                        {
                            currCorrMissions.Add(mBToCheck);

                            SetMissionSymbolToHasSomethingToAccept();
                        }
                    }
                }
            }

            if (isLast)
            {
                if (currCorrMissions.Count <= 0)
                {
                    mMIconMissionTxt.text = "";
                }

                Debug.Log(mBToCheck.missionName);
            }

            if (corrNPCMissionSymbol != null)
            {
                corrNPCMissionSymbol.missionSymbolTxt.text = mMIconMissionTxt.text;
                corrNPCMissionSymbol.missionSymbolTxt.color = mMIconMissionTxt.color;
            }
        }
        else
        {
            if (!mBToCheck.missionCompleted && MissionManager.instance.allCurrAcceptedMissions.Contains(mBToCheck))
            {
                for (int i = 0; i < mBToCheck.allMissionTasks.Length; i++)
                {
                    if (mBToCheck.allMissionTasks[i].mTB.canBeDisplayed && !mBToCheck.allMissionTasks[i].mTB.missionTaskCompleted)
                    {
                        if (mBToCheck.allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
                        {
                            if (mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile != null)
                            {
                                if (corrMerchant != null && mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile == corrMerchant.nPCBP)
                                {
                                    currCorrMissions.Add(mBToCheck);

                                    SetMissionSymbolToHasSomethingToComplete();
                                }
                                else if (corrTavernKeeper != null && mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile == corrTavernKeeper.nPCBP)
                                {
                                    currCorrMissions.Add(mBToCheck);

                                    SetMissionSymbolToHasSomethingToComplete();
                                }
                                else if (corrNPC != null && mBToCheck.allMissionTasks[i].mTB.nPCToTalkToBaseProfile == corrNPC.nPCBP)
                                {
                                    currCorrMissions.Add(mBToCheck);

                                    SetMissionSymbolToHasSomethingToComplete();
                                }
                            }
                        }
                        //else if (mBToCheck.allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.)
                    }
                }

                if (!mBToCheck.isActive)
                {
                    if (mBToCheck.nPCWhereToGetMissionFrom != null)
                    {
                        if (corrMerchant != null && mBToCheck.nPCWhereToGetMissionFrom == corrMerchant.nPCBP)
                        {
                            currCorrMissions.Add(mBToCheck);

                            SetMissionSymbolToHasSomethingToAccept();
                        }
                        else if (corrTavernKeeper != null && mBToCheck.nPCWhereToGetMissionFrom == corrTavernKeeper.nPCBP)
                        {
                            currCorrMissions.Add(mBToCheck);

                            SetMissionSymbolToHasSomethingToAccept();
                        }
                        else if (corrNPC != null && mBToCheck.nPCWhereToGetMissionFrom == corrNPC.nPCBP)
                        {
                            currCorrMissions.Add(mBToCheck);

                            SetMissionSymbolToHasSomethingToAccept();
                        }
                    }
                }
            }

            if (isLast)
            {
                if (currCorrMissions.Count <= 0)
                {
                    mMIconMissionTxt.text = "";
                }

                Debug.Log(mBToCheck.missionName);
            }

            if (corrNPCMissionSymbol != null)
            {
                corrNPCMissionSymbol.missionSymbolTxt.text = mMIconMissionTxt.text;
                corrNPCMissionSymbol.missionSymbolTxt.color = mMIconMissionTxt.color;
            }
        }
    }

    public void SetMissionSymbolToHasSomethingToAccept()
    {
        mMIconMissionTxt.text = "?";
        mMIconMissionTxt.color = MinimapManager.instance.hasMissionToAcceptColor;
    }

    public void SetMissionSymbolToHasSomethingToComplete()
    {
        //if (mMIconMissionTxt.text != "?")
        //{
        mMIconMissionTxt.text = "!";
        mMIconMissionTxt.color = MinimapManager.instance.hasMissionToCompleteColor;
        //}
    }
}
