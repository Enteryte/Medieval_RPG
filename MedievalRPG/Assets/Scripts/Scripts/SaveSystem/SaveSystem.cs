using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [Tooltip("")] public static string path;

    public enum SavingType
    {
        manual,
        auto,
        checkpoint
    }

    public SavingType currSavingType = SavingType.manual;

    public void Awake()
    {
        instance = this;
    }

    public void Save()
    {
        path = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")/* + "_" + GameManager.instance.playtimeInSeconds*/;

        if (!Directory.Exists(Application.persistentDataPath + "/SaveData/" + path))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData/" + path);
        }

        SaveGameObject sGO = createSaveGameObject();

        string JsonString = JsonUtility.ToJson(sGO);

        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/SaveData/" + path + "/" + path + "_gameData.text");

        sw.Write(JsonString);
        sw.Close();

        //sGO.playerPosition = GameManager.instance.playerGO.transform.position;


        //GameManager.Instance.player.inventory.Save();

        //SaveGameData(sGO);
        //SavePlayer(sGO);
        //SavePlayer();
        //SaveInteractableObjects();
        //SaveInventory();
        //SaveNPCs();
        //SaveMissions();
    }

    // Immer nach einer bestimmten Zeit
    public void SaveAutomatic()
    {
        currSavingType = SavingType.auto;

        Save();
    }

    // Bzw. nach dem Erfüllen einer Mission / Nach der Cutscene der erfüllten Mission
    public void SaveCheckpoint()
    {
        currSavingType = SavingType.checkpoint;

        Save();
    }

    public SaveGameObject createSaveGameObject()
    {
        SaveGameObject sGO = new SaveGameObject();

        if (currSavingType == SavingType.manual)
        {
            sGO.savingType = "Manuel";
        }
        else if (currSavingType == SavingType.auto)
        {
            sGO.savingType = "Autosave";
        }
        else
        {
            sGO.savingType = "Checkpoint";
        }

        SaveGameData(sGO);
        SavePlayer(sGO);
        SaveNPCs(sGO);
        SaveMissions(sGO);
        SaveInteractableObjects(sGO);
        SaveInventory(sGO);

        currSavingType = SavingType.manual;

        return sGO;
    }

    public void Load()
    {
        if (Directory.Exists(StartScreenManager.currSelectedLoadSlotBtn.correspondingSaveDataDirectory))
        {
            StreamReader sr = new StreamReader(StartScreenManager.currSelectedLoadSlotBtn.correspondingTextFile);

            string JsonString = sr.ReadToEnd();

            sr.Close();

            //GameManager.instance.displayInventory.inventory.database = Resources.Load<ItemDatabaseBase>("Database");
            //GameManager.Instance.enemyDatabase = Resources.Load<EnemyDatabase>("EnemyDatabase");
            //#endif

            SaveGameObject sGO = JsonUtility.FromJson<SaveGameObject>(JsonString);

            // Playtime
            //GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;

            LoadGameData(sGO);
            LoadPlayer(sGO);
            LoadMissions(sGO);
            LoadNPCs(sGO);
            LoadInventory(sGO);
            LoadInteractableObjects(sGO);

            Debug.Log("nhjmkklsssssssssssss");
            //LoadInteractableObjects();
            //LoadInventory();
            //LoadNPCs();
            //LoadMissions();
        }
    }

    #region Saving
    public void SaveGameData(SaveGameObject sGO)
    {
        sGO.playtimeInSeconds = Mathf.RoundToInt(GameManager.instance.playtimeInSeconds);
        sGO.dayOfSaving = System.DateTime.Now.ToString();

        var screenshotName = "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(Application.persistentDataPath + "/SaveData/" + path, screenshotName), 2);
    }

    public void SavePlayer(SaveGameObject sGO)
    {
        sGO.playerPosition = GameManager.instance.playerGO.transform.localPosition;

        sGO.playerRotation = GameManager.instance.playerGO.transform.localRotation;

        sGO.currPlayerMoney = PlayerValueManager.instance.money;
        sGO.currPlayerHealth = PlayerValueManager.instance.healthSlider.value;
        sGO.currPlayerStamina = PlayerValueManager.instance.staminaSlider.value;
    }

    public void SaveInventory(SaveGameObject sGO)
    {
        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            sGO.itemID.Add(InventoryManager.instance.inventory.slots[i].itemID);
            sGO.itemAmountInInventory.Add(InventoryManager.instance.inventory.slots[i].itemAmount);
        }

        for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
        {
            if (HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase != null)
            {
                sGO.storedItemID.Add(InventoryManager.instance.inventory.database.GetID[HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase]);
                sGO.storedItemAmount.Add(HotbarManager.instance.allHotbarSlotBtn[i].storedAmount);
            }
            else
            {
                sGO.storedItemID.Add(-1);
                sGO.storedItemAmount.Add(-1);
            }
        }

        if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
        {
            sGO.currLeftHandWeaponID = InventoryManager.instance.inventory.database.GetID[EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase];
        }
        else
        {
            sGO.currLeftHandWeaponID = -1;
        }

        if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
        {
            sGO.currRightHandWeaponID = InventoryManager.instance.inventory.database.GetID[EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase];
        }
        else
        {
            sGO.currRightHandWeaponID = -1;
        }
    }

    //public void CheckIfEquipmentIsNull(int idToSet, ClickableInventorySlot invSlotToCheck)
    //{
    //    if (invSlotToCheck.storedItemBase != null)
    //    {
    //        idToSet = InventoryManager.instance.inventory.database.GetID[invSlotToCheck.storedItemBase];
    //    }
    //    else
    //    {
    //        idToSet = -1;
    //    }
    //}

    public void SaveMissions(SaveGameObject sGO)
    {
        for (int i = 0; i < MissionManager.instance.allMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions.Contains(MissionManager.instance.allMissions[i]))
            {
                sGO.allCurrAcceptedMissionNumbers.Add(i);

                for (int y = 0; y < MissionManager.instance.allMissions[i].allMissionTasks.Length; y++)
                {
                    sGO.allAcceptedMissionTaskIsCompletedStates.Add(MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskCompleted);
                    sGO.allAcceptedMissionTaskIsDisplayableStates.Add(MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.canBeDisplayed);
                }
            }

            if (MissionManager.instance.allCurrOpenNotAcceptedMissions.Contains(MissionManager.instance.allMissions[i]))
            {
                sGO.allCurrOpenNotAcceptedMissionNumbers.Add(i);
            }
        }

        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allMissions[i].missionType == MissionBaseProfile.MissionType.main)
            {
                sGO.currentMainMissionName = MissionManager.instance.allCurrAcceptedMissions[i].missionName;
            }
        }
    }

    public void SaveNPCs(SaveGameObject sGO)
    {
        for (int i = 0; i < GameManager.instance.allVillageNPCs.Count; i++)
        {
            sGO.allNPCPositions.Add(GameManager.instance.allVillageNPCs[i].gameObject.transform.position);
            sGO.allNPCRotations.Add(GameManager.instance.allVillageNPCs[i].transform.rotation);

            sGO.isNPCVisible.Add(GameManager.instance.allVillageNPCs[i].gameObject.activeSelf);
        }
    }

    public void SaveInteractableObjects(SaveGameObject sGO)
    {
        for (int i = 0; i < GameManager.instance.allInteractableObjects.Count; i++)
        {
            if (GameManager.instance.allInteractableObjects[i] != null)
            {
                sGO.allInteractableObjectNames.Add(GameManager.instance.allInteractableObjects[i].name);
            }
            else
            {
                sGO.allInteractableObjectNames.Add(null);
            }
        }

        for (int i = 0; i < GameManager.instance.allInteractableDoors.Count; i++)
        {
            sGO.isDoorOpen.Add(GameManager.instance.allInteractableDoors[i].isOpen);
        }
    }
    #endregion

    #region Loading
    public void LoadGameData(SaveGameObject sGO)
    {
        GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;
    }

    public void LoadPlayer(SaveGameObject sGO)
    {
        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = false;
        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = true;

        GameManager.instance.playerGO.transform.localPosition = sGO.playerPosition;
        GameManager.instance.playerGO.transform.localRotation = sGO.playerRotation;

        PlayerValueManager.instance.money = sGO.currPlayerMoney;
        PlayerValueManager.instance.healthSlider.value = sGO.currPlayerHealth;
        PlayerValueManager.instance.staminaSlider.value = sGO.currPlayerStamina;
    }

    public void LoadInventory(SaveGameObject sGO)
    {
        for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
        {
            HotbarManager.instance.allHotbarSlotBtn[i].ClearHotbarSlot();
        }

        EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
        EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

        InventoryManager.instance.inventory.slots.Clear();

        for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
        {
            if (sGO.storedItemID[i] != -1)
            {
                HotbarManager.instance.allHotbarSlotBtn[i].EquipItemToHotbar(InventoryManager.instance.inventory.database.GetItem[sGO.storedItemID[i]], sGO.storedItemAmount[i]);
            }
        }

        for (int i = 0; i < sGO.itemID.Count; i++)
        {
            InventoryManager.instance.inventory.AddItem(InventoryManager.instance.inventory.database.GetItem[sGO.itemID[i]], sGO.itemAmountInInventory[i]);
        }

        if (sGO.currLeftHandWeaponID != -1)
        {
            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().EquipItemToEquipment(InventoryManager.instance.inventory.database.GetItem[sGO.currLeftHandWeaponID], 1);
        }

        if (sGO.currRightHandWeaponID != -1)
        {
            EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().EquipItemToEquipment(InventoryManager.instance.inventory.database.GetItem[sGO.currRightHandWeaponID], 1);
        }
    }

    public void LoadMissions(SaveGameObject sGO)
    {
        MissionManager.instance.allCurrAcceptedMissions.Clear();
        MissionManager.instance.allCurrOpenNotAcceptedMissions.Clear();

        int taskNumber = 0;

        for (int i = 0; i < MissionManager.instance.allMissions.Count; i++)
        {
            if (sGO.allCurrAcceptedMissionNumbers.Contains(i))
            {
                MissionManager.instance.allCurrAcceptedMissions.Add(MissionManager.instance.allMissions[i]);

                for (int y = 0; y < MissionManager.instance.allMissions[i].allMissionTasks.Length; y++)
                {
                    MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskCompleted = sGO.allAcceptedMissionTaskIsCompletedStates[i];
                    MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.canBeDisplayed = sGO.allAcceptedMissionTaskIsDisplayableStates[i];

                    taskNumber += 1;
                }
            }

            if (sGO.allCurrOpenNotAcceptedMissionNumbers.Contains(i))
            {
                MissionManager.instance.allCurrOpenNotAcceptedMissions.Add(MissionManager.instance.allMissions[i]);
            }
        }
    }

    public void LoadNPCs(SaveGameObject sGO)
    {
        for (int i = 0; i < GameManager.instance.allVillageNPCs.Count; i++)
        {
            GameManager.instance.allVillageNPCs[i].transform.position = sGO.allNPCPositions[i];
            GameManager.instance.allVillageNPCs[i].transform.rotation = sGO.allNPCRotations[i];

            GameManager.instance.allVillageNPCs[i].gameObject.SetActive(sGO.isNPCVisible[i]);

            Debug.Log("gjkdefegge");
        }
    }

    public void LoadInteractableObjects(SaveGameObject sGO)
    {
        for (int i = 0; i < GameManager.instance.allInteractableObjects.Count; i++)
        {
            if (!sGO.allInteractableObjectNames.Contains(GameManager.instance.allInteractableObjects[i].name))
            {
                GameManager.instance.allInteractableObjects[i].gameObject.SetActive(false);
                //GameManager.instance.allInteractableObjects.Remove(GameManager.instance.allInteractableObjects[i]);
            }
            else
            {
                GameManager.instance.allInteractableObjects[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < GameManager.instance.allInteractableDoors.Count; i++)
        {
            GameManager.instance.allInteractableDoors[i].isOpen = sGO.isDoorOpen[i];
        }
    }
    #endregion
}
