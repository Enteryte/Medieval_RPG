using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public List<MissionBaseProfile> allMissions;

    public List<MissionBaseProfile> allCurrAcceptedMissions;
    public List<MissionBaseProfile> allCurrOpenNotAcceptedMissions;

    public List<GameObject> allItemsToChangeAfterMissions;
    public List<Item> objectsToExamine;
    public List<Item> itemsToActivateWithTasks;

    [Header("For Speaking With NPCs")]
    public MissionTaskBase mTBWSymon;
    public MissionTaskBase mTBWMya;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if (Blackboard.instance != null)
        {
            if (allCurrOpenNotAcceptedMissions.Count > 0)
            {
                for (int i = 0; i < allCurrOpenNotAcceptedMissions.Count; i++)
                {
                    AddOpenMission(allCurrOpenNotAcceptedMissions[i]);
                }
            }
        }

        for (int i = 0; i < allMissions.Count; i++)
        {
            allMissions[i].missionCompleted = false;
            allMissions[i].isActive = false;

            for (int y = 0; y < allMissions[i].allMissionTasks.Length; y++)
            {
                if (allMissions[i].allMissionTasks[y].mTB.canNormallyBeDisplayed)
                {
                    allMissions[i].allMissionTasks[y].mTB.canBeDisplayed = true;
                }
                else
                {
                    allMissions[i].allMissionTasks[y].mTB.canBeDisplayed = false;
                }

                allMissions[i].allMissionTasks[y].mTB.howManyAlreadyCollected = 0;
                allMissions[i].allMissionTasks[y].mTB.howManyAlreadyKilled = 0;
                allMissions[i].allMissionTasks[y].mTB.howManyAlreadyExamined = 0;
                allMissions[i].allMissionTasks[y].mTB.currGainedPoints = 0;

                allMissions[i].allMissionTasks[y].mTB.missionTaskCompleted = false;
            }
        }

        MinimapManager.instance.CheckAllMinimapSymbols();
    }

    public void AddMission(MissionBaseProfile missionToAdd)
    {
        allCurrAcceptedMissions.Add(missionToAdd);

        if (allCurrOpenNotAcceptedMissions.Contains(missionToAdd))
        {
            allCurrOpenNotAcceptedMissions.Remove(missionToAdd);

            //if (missionToAdd.missionType == MissionBaseProfile.MissionType.side)
            //{
            //    Debug.Log("JKS");

            //    for (int i = 0; i < Blackboard.instance.allBlackboardMB.Length; i++)
            //    {
            //        if (Blackboard.instance.allBlackboardMB[i].storedMissionBP == missionToAdd)
            //        {
            //            Blackboard.instance.allBlackboardMB[i].SetStoredMission(null);
            //            Blackboard.instance.allBlackboardMB[i].gameObject.SetActive(false);
            //        }
            //    }
            //}

            missionToAdd.isActive = true;
        }

        for (int i = 0; i < missionToAdd.allMissionTasks.Length; i++)
        {
            if (missionToAdd.allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
            {
                for (int y = 0; y < InventoryManager.instance.inventory.slots.Count; y++)
                {
                    if (InventoryManager.instance.inventory.slots[y].itemBase == missionToAdd.allMissionTasks[i].mTB.itemToCollectBase)
                    {
                        missionToAdd.allMissionTasks[i].mTB.howManyAlreadyCollected = InventoryManager.instance.inventory.slots[y].itemAmount;
                    }
                }
            }
        }

        if (UIManager.missionToDisplay == null && missionToAdd.missionType == MissionBaseProfile.MissionType.main)
        {
            UIManager.missionToDisplay = missionToAdd;

            UIManager.instance.CreateMissionDisplay();
        }

        UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.addedMissionString;
        UIAnimationHandler.instance.addedMissionTxt.text = missionToAdd.missionName;
        UIAnimationHandler.instance.AnimateAddedNewMissionMessage();

        MinimapManager.instance.CheckAllMinimapSymbols();
    }

    public void RemoveMission(MissionBaseProfile missionToRemove)
    {
        allCurrAcceptedMissions.Remove(missionToRemove);

        UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.completedMissionString;
        UIAnimationHandler.instance.addedMissionTxt.text = missionToRemove.missionName;
        UIAnimationHandler.instance.AnimateAddedNewMissionMessage();

        MinimapManager.instance.CheckAllMinimapSymbols();
    }

    public void AddOpenMission(MissionBaseProfile missionToAdd)
    {
        if (!allCurrOpenNotAcceptedMissions.Contains(missionToAdd))
        {
            allCurrOpenNotAcceptedMissions.Add(missionToAdd);
        }

        //if (missionToAdd.missionType == MissionBaseProfile.MissionType.side)
        //{
        //    for (int i = 0; i < Blackboard.instance.allBlackboardMB.Length; i++)
        //    {
        //        if (Blackboard.instance.allBlackboardMB[i].storedMissionBP == null)
        //        {
        //            Blackboard.instance.allBlackboardMB[i].SetStoredMission(missionToAdd);
        //            Blackboard.instance.allBlackboardMB[i].gameObject.SetActive(true);

        //            MinimapManager.instance.CheckAllMinimapSymbols();

        //            return;
        //        }
        //    }
        //}
    }

    //public void RemoveOpenMission(BlackboardMissionButton blackboardMB, MissionBaseProfile missionToRemove)
    //{
    //    allCurrOpenNotAcceptedMissions.Remove(missionToAdd);

    //    if (missionToAdd.missionType == MissionBaseProfile.MissionType.side)
    //    {
    //        for (int i = 0; i < Blackboard.instance.allBlackboardMB.Length; i++)
    //        {
    //            if (Blackboard.instance.allBlackboardMB[i].storedMissionBP == null)
    //            {
    //                Blackboard.instance.allBlackboardMB[i].SetStoredMission(missionToAdd);
    //                Blackboard.instance.allBlackboardMB[i].gameObject.SetActive(true);

    //                return;
    //            }
    //        }
    //    }
    //}

    public void CheckMissionTaskProgress(MissionBaseProfile mBP, MissionTaskBase missionTaskToCheck)
    {
        if (missionTaskToCheck.missionTaskType == MissionTaskBase.MissionTaskType.kill)
        {
            if (missionTaskToCheck.howManyAlreadyKilled >= missionTaskToCheck.howManyToKill)
            {
                CompleteMissionTask(mBP, missionTaskToCheck);
            }
        }
        else if (missionTaskToCheck.missionTaskType == MissionTaskBase.MissionTaskType.collect)
        {
            if (missionTaskToCheck.howManyAlreadyCollected >= missionTaskToCheck.howManyToCollect)
            {
                CompleteMissionTask(mBP, missionTaskToCheck);
            }
        }
        else if (missionTaskToCheck.missionTaskType == MissionTaskBase.MissionTaskType.examine)
        {
            if (missionTaskToCheck.howManyAlreadyExamined >= missionTaskToCheck.howManyToExamine)
            {
                CompleteMissionTask(mBP, missionTaskToCheck);
            }
        }
    }

    public void CompleteMissionTask(MissionBaseProfile mBP, MissionTaskBase missionTaskToComplete)
    {
        missionTaskToComplete.missionTaskCompleted = true;

        if (missionTaskToComplete.moneyReward > 0)
        {
            PlayerValueManager.instance.money += missionTaskToComplete.moneyReward;
        }

        for (int i = 0; i < missionTaskToComplete.itemRewards.Length; i++)
        {
            InventoryManager.instance.inventory.AddItem(missionTaskToComplete.itemRewards[i].iBP, missionTaskToComplete.itemRewards[i].howManyToGet);
        }

        for (int i = 0; i < missionTaskToComplete.itemsToRemove.Length; i++)
        {
            for (int x = 0; x < InventoryManager.instance.inventory.slots.Count; x++)
            {
                if (InventoryManager.instance.inventory.slots[x].itemBase == missionTaskToComplete.itemsToRemove[i].iBP)
                {
                    InventoryManager.instance.inventory.slots[x].RemoveAmount(missionTaskToComplete.itemsToRemove[i].howManyToRemove);
                }
            }
        }

        missionTaskToComplete.canBeDisplayed = false;

        if (missionTaskToComplete.missionTaskToActivate != null)
        {
            missionTaskToComplete.missionTaskToActivate.canBeDisplayed = true;

            if (missionTaskToComplete.missionTaskToActivate.missionTaskType == MissionTaskBase.MissionTaskType.examine)
            {
                for (int i = 0; i < objectsToExamine.Count; i++)
                {
                    if (objectsToExamine[i].iBP == missionTaskToComplete.missionTaskToActivate.iBPOfItemToExamine)
                    {
                        objectsToExamine[i].InstantiateIOCanvas();
                    }
                    else if (missionTaskToComplete.missionTaskToActivate.iBPOfItemToExamine2 != null && objectsToExamine[i].iBP == missionTaskToComplete.missionTaskToActivate.iBPOfItemToExamine2)
                    {
                        objectsToExamine[i].InstantiateIOCanvas();
                    }
                }
            }

            for (int i = 0; i < itemsToActivateWithTasks.Count; i++)
            {
                if (itemsToActivateWithTasks[i].corresspondingMissionTask == missionTaskToComplete.missionTaskToActivate)
                {
                    itemsToActivateWithTasks[i].InstantiateIOCanvas();
                }
            }

            if (UIManager.missionToDisplay != null && UIManager.missionToDisplay == mBP)
            {
                var numb = 0;

                for (int i = 0; i < mBP.allMissionTasks.Length; i++)
                {
                    if (mBP.allMissionTasks[i].mTB == missionTaskToComplete.missionTaskToActivate)
                    {
                        numb = i;
                    }
                }

                if (UIManager.missionToDisplay != mBP || UIManager.missionToDisplay == null)
                {
                    UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.updatedMissionString;
                    UIAnimationHandler.instance.addedMissionTxt.text = mBP.missionName;
                    UIAnimationHandler.instance.AnimateAddedNewMissionMessage();
                }
                else
                {
                    if (missionTaskToComplete.missionTaskToActivate2 != null)
                    {
                        UIManager.instance.UpdateMissionDisplayTasks(missionTaskToComplete, missionTaskToComplete.missionTaskToActivate, mBP.allMissionTasks[numb], missionTaskToComplete.missionTaskToActivate2,
                            mBP.allMissionTasks[numb + 2], mBP.allMissionTasks[numb].taskDescription, true);
                    }
                    else
                    {
                        UIManager.instance.UpdateMissionDisplayTasks(missionTaskToComplete, missionTaskToComplete.missionTaskToActivate, mBP.allMissionTasks[numb], null, null,
                            mBP.allMissionTasks[numb].taskDescription, true);
                    }
                }

                //if (missionTaskToComplete.missionTaskToActivate.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
                //{
                //    if (missionTaskToComplete.missionTaskToActivate.talkToAllNPCs)
                //    {
                //        for (int i = 0; i < GameManager.instance.allVillageNPCs.Count; i++)
                //        {

                //        }
                //    }
                //}
            }
            else if (UIManager.missionToDisplay == null || UIManager.missionToDisplay != mBP)
            {
                UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.updatedMissionString;
                UIAnimationHandler.instance.addedMissionTxt.text = mBP.missionName;
                UIAnimationHandler.instance.AnimateAddedNewMissionMessage();
            }

            //if (missionTaskToComplete.cutsceneToTrigger != null && missionTaskToComplete.cutsceneToTrigger.isNightCutscene)
            //{
            //    CutsceneManager.instance.currCP = missionTaskToComplete.cutsceneToTrigger;
            //    CutsceneManager.instance.playableDirector.playableAsset = missionTaskToComplete.cutsceneToTrigger.cutscene;
            //    CutsceneManager.instance.playableDirector.Play();
            //}

            // WIP: Animation dazu fehlt noch + HUD-Missionsanzeige muss noch geupdated werden.
        }
        else
        {
            if (UIManager.missionToDisplay != mBP || UIManager.missionToDisplay == null)
            {
                UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.updatedMissionString;
                UIAnimationHandler.instance.addedMissionTxt.text = mBP.missionName;
                UIAnimationHandler.instance.AnimateAddedNewMissionMessage();
            }
            else
            {
                UIManager.instance.UpdateMissionDisplayTasks(missionTaskToComplete, null, null, null, null, null, false);
            }
        }

        if (missionTaskToComplete.cutsceneToTrigger != null && missionTaskToComplete.missionTaskToActivate == null)
        {
            CutsceneManager.instance.currCP = missionTaskToComplete.cutsceneToTrigger;
            CutsceneManager.instance.playableDirector.playableAsset = missionTaskToComplete.cutsceneToTrigger.cutscene;
            CutsceneManager.instance.playableDirector.Play();
        }

        MinimapManager.instance.CheckAllMinimapSymbols();

        CheckMissionProgress(mBP);
    }

    public void CheckMissionProgress(MissionBaseProfile missionToCheck)
    {
        for (int i = 0; i < missionToCheck.allMissionTasks.Length; i++)
        {
            if (!missionToCheck.allMissionTasks[i].mTB.missionTaskCompleted)
            {
                return;
            }
        }

        CompleteMission(missionToCheck);
    }

    public void CompleteMission(MissionBaseProfile missionToComplete)
    {
        allCurrAcceptedMissions.Remove(missionToComplete);

        missionToComplete.missionCompleted = true;

        for (int i = 0; i < missionToComplete.allMissionTasks.Length; i++)
        {
            if (!missionToComplete.allMissionTasks[i].mTB.missionTaskCompleted && missionToComplete.allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
            {
                InventoryManager.instance.inventory.RemoveItem(missionToComplete.allMissionTasks[i].mTB.itemToCollectBase, missionToComplete.allMissionTasks[i].mTB.howManyToCollect);
            }

            missionToComplete.allMissionTasks[i].mTB.missionTaskCompleted = true;
        }

        if (UIManager.missionToDisplay == missionToComplete)
        {
            UIManager.missionToDisplay = null;
        }

        if (missionToComplete.nextMissionToTrigger != null)
        {
            AddMission(missionToComplete.nextMissionToTrigger);
            //allCurrAcceptedMissions.Add(missionToComplete.nextMissionToTrigger);
        }

        if (missionToComplete.cutsceneToTrigger != null)
        {
            CutsceneManager.instance.currCP = missionToComplete.cutsceneToTrigger;
            CutsceneManager.instance.playableDirector.playableAsset = missionToComplete.cutsceneToTrigger.cutscene;
            CutsceneManager.instance.playableDirector.Play();
        }

        PlayerValueManager.instance.money += missionToComplete.moneyReward;

        for (int i = 0; i < missionToComplete.itemRewards.Length; i++)
        {
            InventoryManager.instance.inventory.AddItem(missionToComplete.itemRewards[i].iBP, missionToComplete.itemRewards[i].howManyToGet);
        }

        for (int i = 0; i < missionToComplete.itemsToRemove.Length; i++)
        {
            for (int x = 0; x < InventoryManager.instance.inventory.slots.Count; x++)
            {
                if (InventoryManager.instance.inventory.slots[x].itemBase == missionToComplete.itemsToRemove[i].iBP)
                {
                    InventoryManager.instance.inventory.slots[x].RemoveAmount(missionToComplete.itemsToRemove[i].howManyToRemove);
                }
            }
        }

        if (missionToComplete.changeEnvironment)
        {
            ChangeEnvironmentAfterMission(missionToComplete);
        }

        if (missionToComplete.missionAfterItsActive != null)
        {
            CheckSideMissions(missionToComplete.missionAfterItsActive);
        }

        UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.completedMissionString;
        UIAnimationHandler.instance.addedMissionTxt.text = missionToComplete.missionName;
        UIAnimationHandler.instance.AnimateAddedNewMissionMessage();

        MinimapManager.instance.CheckAllMinimapSymbols();

        //UIManager.instance.missionTaskObjParentObj.gameObject.transform.parent.gameObject.SetActive(true);

        //UIManager.instance.missionTaskObjParentObj.GetComponent<GridLayoutGroup>().enabled = true;

        //for (int i = 0; i < UIManager.instance.missionTaskObjParentObj.transform.childCount; i++)
        //{
        //    Destroy(UIManager.instance.missionTaskObjParentObj.transform.GetChild(i).gameObject);
        //}
    }

    public void ChangeEnvironmentAfterMission(MissionBaseProfile mission)
    {
        for (int i = 0; i < allItemsToChangeAfterMissions.Count; i++)
        {
            for (int y = 0; y < mission.itemToChangeAfterMissionNames.Length; y++)
            {
                if (allItemsToChangeAfterMissions[i].name == mission.itemToChangeAfterMissionNames[y])
                {
                    allItemsToChangeAfterMissions[i].SetActive(!allItemsToChangeAfterMissions[i].activeSelf);

                    allItemsToChangeAfterMissions.Remove(allItemsToChangeAfterMissions[i]);
                }
            }
        }
    }

    public void CheckSideMissions(MissionBaseProfile missionToCheckFor)
    {
        missionToCheckFor.isActive = true;

        AddOpenMission(missionToCheckFor);

        //for (int i = 0; i < GameManager.instance.allVillageNPCs.Count; i++)
        //{
        //    for (int y = 0; y < GameManager.instance.allVillageNPCs[i].allSideMissionsWithNPC.Count; y++)
        //    {
        //        if (GameManager.instance.allVillageNPCs[i].allSideMissionsWithNPC[y].missionAfterItsActive == missionToCheckFor)
        //        {
        //            GameManager.instance.allVillageNPCs[i].allSideMissionsWithNPC[y].isActive = true;

        //            AddOpenMission(GameManager.instance.allVillageNPCs[i].allSideMissionsWithNPC[y]);

        //            Debug.Log("NHJS");
        //        }
        //    }
        //}
    }

    public void StartMissionAfterCutscene()
    {
       AddMission(CutsceneManager.instance.currCP.missionToPlayAfter);
    }

    //public void AddMissionTaskToActiveMissionAfterCutscene()
    //{
    //    AddMission(CutsceneManager.instance.currCP.missionToPlayAfter);
    //}
}
