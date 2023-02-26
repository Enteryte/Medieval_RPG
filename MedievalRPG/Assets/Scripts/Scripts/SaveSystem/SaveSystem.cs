using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [Tooltip("")] public static string path;
    [Tooltip("")] public static string pathEnd;

    public enum SavingType
    {
        manual,
        auto,
        checkpoint
    }

    public SavingType currSavingType = SavingType.manual;

    public static string continuePath;
    public static string continuePath2;
    public static string continueTxtFile;
    public static string continueTxtFile2;

    [Header("New Game Values")]
    public ItemBaseProfile[] startItems;
    public int[] startItemAmounts;
    public ItemBaseProfile gems;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }

    public void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/OptionsData"))
        {
            SaveOptions();
        }
        else
        {
            LoadOptionsData();
        }

        if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

            if (dirInfo.Length > 0)
            {
                var gameDataFolder = Directory.GetFiles(dirInfo[dirInfo.Length - 1]);

                for (int i = dirInfo.Length - 1; i > -1; i--)
                {
                    if (!dirInfo[i].Contains("_N"))
                    {
                        gameDataFolder = Directory.GetFiles(dirInfo[i]);

                        StartScreenManager.instance.continueBtn.interactable = true;
                        StartScreenManager.instance.loadSaveDataBtn.interactable = true;

                        return;
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadContinueData();
        }
    }

    public void Save()
    {
        path = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")/* + "_" + GameManager.instance.playtimeInSeconds*/;

        if (SceneChangeManager.instance.wentThroughTrigger)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                pathEnd = "_V_N";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                pathEnd = "_T_N";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                pathEnd = "_D1_N";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                pathEnd = "_D2_N";
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                pathEnd = "_V";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                pathEnd = "_T";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                pathEnd = "_D1";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                pathEnd = "_D2";
            }
        }

        if (!Directory.Exists(Application.persistentDataPath + "/SaveData/" + path + pathEnd))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData/" + path + pathEnd);
        }

        SaveGameObject sGO = createSaveGameObject();

        string JsonString = JsonUtility.ToJson(sGO);

        StreamWriter sw = null;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            sw = new StreamWriter(Application.persistentDataPath + "/SaveData/" + path + pathEnd + "/" + path + "_gameData_V.text");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            sw = new StreamWriter(Application.persistentDataPath + "/SaveData/" + path + pathEnd + "/" + path + "_gameData_T.text");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            sw = new StreamWriter(Application.persistentDataPath + "/SaveData/" + path + pathEnd + "/" + path + "_gameData_D1.text");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            sw = new StreamWriter(Application.persistentDataPath + "/SaveData/" + path + pathEnd + "/" + path + "_gameData_D2.text");
        }

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

    public void SaveOptions()
    {
        path = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")/* + "_" + GameManager.instance.playtimeInSeconds*/;

        if (!Directory.Exists(Application.persistentDataPath + "/OptionsData"))
        {
            OptionManager.instance.tutorialToggle.isOn = true;
            StartScreenManager.instance.showTutorialToggle.isOn = !OptionManager.instance.tutorialToggle.isOn;

            // Set Start-Option-Values
            OptionManager.instance.masterSlider.value = 0.5f;
            OptionManager.instance.environmentSlider.value = 0.5f;
            OptionManager.instance.voiceSlider.value = 0.5f;
            OptionManager.instance.musicSlider.value = 0.1f;
            OptionManager.instance.sfxSlider.value = 0.5f;

            OptionManager.instance.windowModeToggle.isOn = false;
            OptionManager.instance.resolutionDropdown.value = 0;
            OptionManager.instance.subtitleToggle.isOn = true;
            StartScreenManager.instance.showSubtitle = true;

            OptionManager.instance.cameraSensiSlider.value = 1f;
            OptionManager.instance.mouseSensiSlider.value = 0.5f;

            OptionManager.instance.controllerToggle.isOn = false;

            Directory.CreateDirectory(Application.persistentDataPath + "/OptionsData");
        }

        SaveGameObject sGO = createOptionsSaveGameObject();

        string JsonString = JsonUtility.ToJson(sGO);

        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/OptionsData/" +/* path + */"/" + path + "_optionsData.text");

        sw.Write(JsonString);
        sw.Close();

        Debug.Log("HJKL");
        if (GameManager.instance)
        {
            Debug.Log("HJKL");
            if (GameManager.instance.pauseMenuScreen.activeSelf)
            {
                Debug.Log("HJKL");
                GameManager.instance.areYouSureScreenIsActive = true;
            }
        }
    }

    // Immer nach einer bestimmten Zeit
    public void SaveAutomatic()
    {
        currSavingType = SavingType.auto;

        Save();
    }

    // Bzw. nach dem Erf�llen einer Mission / Nach der Cutscene der erf�llten Mission
    public void SaveCheckpoint()
    {
        currSavingType = SavingType.checkpoint;

        Save();
    }

    public SaveGameObject createSaveGameObject()
    {
        SaveGameObject sGO = new SaveGameObject();

        sGO.savingType = "Manuel";
        //if (currSavingType == SavingType.manual)
        //{
        //    sGO.savingType = "Manuel";
        //}
        //else if (currSavingType == SavingType.auto)
        //{
        //    sGO.savingType = "Autosave";
        //}
        //else
        //{
        //    sGO.savingType = "Checkpoint";
        //}

        sGO.currSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SaveGameData(sGO);
        SaveTutorial(sGO);
        SavePlayer(sGO);
        SaveNPCs(sGO);
        SaveEnemies(sGO);
        SaveMissions(sGO);
        SaveInteractableObjects(sGO);
        SaveInventoryAndItemDatabase(sGO);
        SaveDaytimeAndWeather(sGO);
        SaveCutscene(sGO);

        currSavingType = SavingType.manual;

        return sGO;
    }

    public SaveGameObject createOptionsSaveGameObject()
    {
        SaveGameObject sGO = new SaveGameObject();

        SaveOptions(sGO);

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

            //SceneManager.LoadScene(sGO.currSceneIndex);

            LoadGameData(sGO);
            LoadTutorial(sGO);
            LoadPlayer(sGO);
            LoadMissions(sGO);
            LoadNPCs(sGO);
            LoadEnemies(sGO);
            LoadInventoryAndItemDatabase(sGO);
            LoadInteractableObjects(sGO);
            LoadDaytimeAndWeather(sGO);
            LoadCutscene(sGO);

            //LoadInteractableObjects();
            //LoadInventory();
            //LoadNPCs();
            //LoadMissions();
        }
    }

    public void LoadOptionsData()
    {
        if (Directory.Exists(Application.persistentDataPath + "/OptionsData/"))
        {
            var dirInfo = Directory.GetFiles(Application.persistentDataPath + "/OptionsData/");

            //continueTxtFileDirectory.GetFiles(dirInfo[dirInfo.Length])

            //for (int i = dirInfo.Length - 1; i > -1; i--)
            //{
            //    var gameDataFolder = Directory.GetFiles(dirInfo[i]);
            //}

            //if (dirInfo.Length > 0)
            //{
            //    var optionsDataFolder = Directory.GetFiles(dirInfo[dirInfo.Length - 1]);

            //    continuePath = dirInfo[dirInfo.Length - 1];
            //    continueTxtFile = optionsDataFolder[0];
            //}
            //else
            //{
            //    var optionsDataFolder = Directory.GetFiles(dirInfo[dirInfo.Length]);

            //    continuePath = dirInfo[dirInfo.Length];
            //    continueTxtFile = optionsDataFolder[0];
            //}

            continueTxtFile = dirInfo[dirInfo.Length - 1];
        }

        if (Directory.Exists(Application.persistentDataPath + "/OptionsData/"))
        {
            StreamReader sr = new StreamReader(continueTxtFile);

            string JsonString = sr.ReadToEnd();

            sr.Close();

            //GameManager.instance.displayInventory.inventory.database = Resources.Load<ItemDatabaseBase>("Database");
            //GameManager.Instance.enemyDatabase = Resources.Load<EnemyDatabase>("EnemyDatabase");
            //#endif

            SaveGameObject sGO = JsonUtility.FromJson<SaveGameObject>(JsonString);

            // Playtime
            //GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;

            LoadOptions(sGO);

            //LoadInteractableObjects();
            //LoadInventory();
            //LoadNPCs();
            //LoadMissions();
        }
    }

    public void LoadContinueData()
    {
        if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

            var gameDataFolder = Directory.GetFiles(dirInfo[dirInfo.Length - 1]);

            for (int i = dirInfo.Length - 1; i > -1; i--)
            {
                if (!dirInfo[i].Contains("_N"))
                {
                    Debug.Log(i);

                    gameDataFolder = Directory.GetFiles(dirInfo[i]);

                    break;
                }

                //Debug.Log(i);

                //var gameDataFolder2 = dirInfo[i];

                //Debug.Log(gameDataFolder2.ToString());
            }

            continuePath = dirInfo[dirInfo.Length - 1];
            continueTxtFile = gameDataFolder[0];
        }

        if (Directory.Exists(continuePath))
        {
            StreamReader sr = new StreamReader(continueTxtFile);

            string JsonString = sr.ReadToEnd();

            sr.Close();

            //GameManager.instance.displayInventory.inventory.database = Resources.Load<ItemDatabaseBase>("Database");
            //GameManager.Instance.enemyDatabase = Resources.Load<EnemyDatabase>("EnemyDatabase");
            //#endif

            SaveGameObject sGO = JsonUtility.FromJson<SaveGameObject>(JsonString);

            // Playtime
            //GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;

            //SceneManager.LoadScene(sGO.currSceneIndex);

            LoadGameData(sGO);
            LoadTutorial(sGO);
            LoadPlayer(sGO);
            LoadMissions(sGO);
            LoadNPCs(sGO);
            LoadEnemies(sGO);
            LoadInventoryAndItemDatabase(sGO);
            LoadInteractableObjects(sGO);
            LoadDaytimeAndWeather(sGO);
            LoadCutscene(sGO);

            //LoadInteractableObjects();
            //LoadInventory();
            //LoadNPCs();
            //LoadMissions();
        }
    }

    public void LoadAfterChangedSceneIG()
    {
        if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            continuePath = "";

            continueTxtFile = "";

            var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

            //for (int i = dirInfo.Length - 1; i > -1; i--)
            //{
            //    var gameDataFolder = Directory.GetFiles(dirInfo[i]);
            //}

            var gameDataFolder = Directory.GetFiles(dirInfo[dirInfo.Length - 1]);

            for (int i = 0; i < dirInfo.Length - 1; i++)
            {
                if (dirInfo[i].Contains("_V") && LoadingScreen.currLSP.sceneToLoadIndex == 1)
                {
                    continuePath = dirInfo[i];

                    gameDataFolder = Directory.GetFiles(dirInfo[i]);

                    Debug.Log(i);
                    Debug.Log(dirInfo[i]);
   
                    continueTxtFile = gameDataFolder[0];

                    break;
                }
                else if (dirInfo[i].Contains("_T") && LoadingScreen.currLSP.sceneToLoadIndex == 2)
                {
                    continuePath = dirInfo[i];

                    gameDataFolder = Directory.GetFiles(dirInfo[i]);

                    continueTxtFile = gameDataFolder[0];

                    break;
                }
                else if (dirInfo[i].Contains("_D1") && LoadingScreen.currLSP.sceneToLoadIndex == 3)
                {
                    continuePath = dirInfo[i];

                    gameDataFolder = Directory.GetFiles(dirInfo[i]);

                    continueTxtFile = gameDataFolder[0];

                    break;
                }
                else if (dirInfo[i].Contains("_D2") && LoadingScreen.currLSP.sceneToLoadIndex == 4)
                {
                    continuePath = dirInfo[i];

                    gameDataFolder = Directory.GetFiles(dirInfo[i]);

                    continueTxtFile = gameDataFolder[0];

                    break;
                }
            }

            var dirInfo2 = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

            var gameDataFolder2 = Directory.GetFiles(dirInfo2[dirInfo2.Length - 1]);

            //if (continueTxtFile == "")
            //{

                //for (int i = dirInfo.Length - 1; i > -1; i--)
                //{
                //    var gameDataFolder = Directory.GetFiles(dirInfo[i]);
                //}

                gameDataFolder2 = Directory.GetFiles(dirInfo2[dirInfo2.Length - 1]);

                continuePath2 = dirInfo2[dirInfo2.Length - 1];
                continueTxtFile2 = gameDataFolder2[0];
            //}
        }

        if (continuePath != "" && Directory.Exists(continuePath))
        {
            Debug.Log("HHHHHHHHHHHHHHHHIER");

            StreamReader sr = new StreamReader(continueTxtFile);

            string JsonString = sr.ReadToEnd();

            sr.Close();

            //GameManager.instance.displayInventory.inventory.database = Resources.Load<ItemDatabaseBase>("Database");
            //GameManager.Instance.enemyDatabase = Resources.Load<EnemyDatabase>("EnemyDatabase");
            //#endif

            SaveGameObject sGO = JsonUtility.FromJson<SaveGameObject>(JsonString);

            // Playtime
            //GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;

            //LoadGameData(sGO);
            //LoadTutorial(sGO);
            //LoadPlayer(sGO);

            //PlayerValueManager.instance.money = sGO.currPlayerMoney;
            //PlayerValueManager.instance.healthSlider.value = sGO.currPlayerHealth;
            //PlayerValueManager.instance.staminaSlider.value = sGO.currPlayerStamina;

            //LoadMissions(sGO);
            //LoadNPCs(sGO);
            LoadEnemies(sGO);
            //LoadInventory(sGO);
            LoadInteractableObjects(sGO);

            //LoadInteractableObjects();
            //LoadInventory();
            //LoadNPCs();
            //LoadMissions();
        }

        if (Directory.Exists(continuePath2))
        {
            Debug.Log("HHHHHHHHHHHHHHHHIER");

            StreamReader sr = new StreamReader(continueTxtFile2);

            string JsonString = sr.ReadToEnd();

            sr.Close();

            //GameManager.instance.displayInventory.inventory.database = Resources.Load<ItemDatabaseBase>("Database");
            //GameManager.Instance.enemyDatabase = Resources.Load<EnemyDatabase>("EnemyDatabase");
            //#endif

            SaveGameObject sGO = JsonUtility.FromJson<SaveGameObject>(JsonString);

            // Playtime
            //GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;

            LoadGameData(sGO);
            LoadTutorial(sGO);
            //LoadPlayer(sGO);

            PlayerValueManager.instance.money = sGO.currPlayerMoney;
            PlayerValueManager.instance.healthSlider.value = sGO.currPlayerHealth;
            PlayerValueManager.instance.staminaSlider.value = sGO.currPlayerStamina;

            LoadMissions(sGO);
            //LoadNPCs(sGO);
            //LoadEnemies(sGO);
            LoadInventoryAndItemDatabase(sGO);

            //if (continuePath.Contains("_D2") || continuePath.Contains("_D1") || LoadingScreen.currLSP.sceneToLoadIndex == 3 || LoadingScreen.currLSP.sceneToLoadIndex == 4)
            //{
            //    if (sGO.changeDaytime)
            //    {
            //        GameManager.instance.correspondingCutsceneProfilAtNight = null;

            //        GameManager.instance.hdrpTOD.m_timeOfDayMultiplier = 1;
            //    }
            //    else
            //    {
            //        GameManager.instance.correspondingCutsceneProfilAtNight = GameManager.instance.cutsceneProfileAtNightHolder;

            //        GameManager.instance.hdrpTOD.m_timeOfDayMultiplier = 0;
            //    }

            //    GameManager.instance.changeDaytime = sGO.changeDaytime;

            //    GameManager.instance.hdrpTOD.TimeOfDay = sGO.timeOfDay;

            //    GameManager.instance.currRainingDuration = 0;
            //}
            //else
            //{
                LoadDaytimeAndWeather(sGO);
            //}

            LoadBuffsAndDebuffs(sGO);
            //LoadInteractableObjects(sGO);

            //LoadInteractableObjects();
            //LoadInventory();
            //LoadNPCs();
            //LoadMissions();
        }
        //else
        //{
        //    StreamReader sr = new StreamReader(continueTxtFile);

        //    string JsonString = sr.ReadToEnd();

        //    sr.Close();

        //    //GameManager.instance.displayInventory.inventory.database = Resources.Load<ItemDatabaseBase>("Database");
        //    //GameManager.Instance.enemyDatabase = Resources.Load<EnemyDatabase>("EnemyDatabase");
        //    //#endif

        //    SaveGameObject sGO = JsonUtility.FromJson<SaveGameObject>(JsonString);

        //    // Playtime
        //    //GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;

        //    LoadGameData(sGO);
        //    LoadTutorial(sGO);
        //    //LoadPlayer(sGO);

        //    PlayerValueManager.instance.money = sGO.currPlayerMoney;
        //    PlayerValueManager.instance.healthSlider.value = sGO.currPlayerHealth;
        //    PlayerValueManager.instance.staminaSlider.value = sGO.currPlayerStamina;

        //    LoadMissions(sGO);
        //    LoadNPCs(sGO);
        //    LoadEnemies(sGO);
        //    LoadInventory(sGO);
        //    LoadInteractableObjects(sGO);

        //    //LoadInteractableObjects();
        //    //LoadInventory();
        //    //LoadNPCs();
        //    //LoadMissions();
        //}
    }

    #region Saving
    public void SaveGameData(SaveGameObject sGO)
    {
        sGO.playtimeInSeconds = Mathf.RoundToInt(GameManager.instance.playtimeInSeconds);
        sGO.dayOfSaving = System.DateTime.Now.ToString();

        var screenshotName = "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(Application.persistentDataPath + "/SaveData/" + path + pathEnd, screenshotName), 2);
    }

    public void SaveTutorial(SaveGameObject sGO)
    {
        //sGO.displayTutorial = GameManager.instance.displayTutorial;

        for (int i = 0; i < TutorialManager.instance.allCompletedTutorials.Count; i++)
        {
            sGO.allCompletedTutorialNumbers.Add(TutorialManager.instance.allCompletedTutorials[i].tutorialNumber);
        }
    }

    public void SavePlayer(SaveGameObject sGO)
    {
        sGO.playerPosition = GameManager.instance.playerGO.transform.localPosition;

        sGO.playerRotation = GameManager.instance.playerGO.transform.localRotation;

        sGO.currPlayerMoney = PlayerValueManager.instance.money;
        sGO.currPlayerHealth = PlayerValueManager.instance.healthSlider.value;
        sGO.currPlayerStamina = PlayerValueManager.instance.staminaSlider.value;
    }

    public void SaveInventoryAndItemDatabase(SaveGameObject sGO)
    {
        for (int i = 0; i < InventoryManager.instance.inventory.database.items.Length; i++)
        {
            sGO.itemIsNew.Add(InventoryManager.instance.inventory.database.items[i].isNew);
        }

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

        if (EquippingManager.instance.glovesES.GetComponent<ClickableInventorySlot>().storedItemBase == null)
        {
            sGO.usesGloves = false;
        }
        else
        {
            sGO.usesGloves = true;
        }

        if (EquippingManager.instance.pauldronsES.GetComponent<ClickableInventorySlot>().storedItemBase == null)
        {
            sGO.usesPauldrons = false;
        }
        else
        {
            sGO.usesPauldrons = true;
        }

        if (EquippingManager.instance.poleynsES.GetComponent<ClickableInventorySlot>().storedItemBase == null)
        {
            sGO.usesPoleyns = false;
        }
        else
        {
            sGO.usesPoleyns = true;
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

                    // Save Mission Task Progress
                    if (MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.kill)
                    {
                        sGO.allCurrKillMissionKills.Add(MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyKilled);
                    }
                    else if (MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                    {
                        sGO.allCurrCollectMissionProgresses.Add(MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyCollected);
                    }
                    else if (MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.examine)
                    {
                        sGO.allCurrExamineMissionProgresses.Add(MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyExamined);
                    }
                }
            }

            if (MissionManager.instance.allCurrOpenNotAcceptedMissions.Contains(MissionManager.instance.allMissions[i]))
            {
                sGO.allCurrOpenNotAcceptedMissionNumbers.Add(i);
            }
        }

        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions[i].missionType == MissionBaseProfile.MissionType.main)
            {
                sGO.currentMainMissionName = MissionManager.instance.allCurrAcceptedMissions[i].missionName;
            }
        }

        sGO.alreadyPlayedAgainstKilian = GameManager.instance.alreadyPlayedAgainstKilian;
    }

    public void SaveNPCs(SaveGameObject sGO)
    {
        for (int i = 0; i < GameManager.instance.allVillageNPCs.Count; i++)
        {
            sGO.allNPCPositions.Add(GameManager.instance.allVillageNPCs[i].gameObject.transform.localPosition);
            sGO.allNPCRotations.Add(GameManager.instance.allVillageNPCs[i].transform.localRotation);

            sGO.isNPCVisible.Add(GameManager.instance.allVillageNPCs[i].gameObject.activeSelf);
        }

        // Save Waypoints
        for (int i = 0; i < GameManager.instance.allWalkingNPCs.Count; i++)
        {
            if (GameManager.instance.allWalkingNPCs[i].currWaypoint == null)
            {
                sGO.currWaypointNames.Add(GameManager.instance.allWalkingNPCs[i].firstWaypoint.name);
            }
            else
            {
                sGO.currWaypointNames.Add(GameManager.instance.allWalkingNPCs[i].currWaypoint.name);
            }
        }
    }

    public void SaveEnemies(SaveGameObject sGO)
    {
        //for (int i = 0; i < GameManager.instance.allMeleeEnemies.Count; i++)
        //{
        //    sGO.allMeleeEnemyPositions.Add(GameManager.instance.allMeleeEnemies[i].gameObject.transform.position);
        //    sGO.allMeleeEnemyRotations.Add(GameManager.instance.allMeleeEnemies[i].gameObject.transform.rotation);
        //}

        //for (int i = 0; i < GameManager.instance.allArcherEnemies.Count; i++)
        //{
        //    sGO.allArcherEnemyPositions.Add(GameManager.instance.allArcherEnemies[i].gameObject.transform.position);
        //    sGO.allArcherEnemyRotations.Add(GameManager.instance.allArcherEnemies[i].gameObject.transform.rotation);
        //}

        for (int i = 0; i < GameManager.instance.allMeleeEnemies.Count; i++)
        {
            sGO.enemyIsActive.Add(GameManager.instance.allMeleeEnemies[i].gameObject.activeSelf);
        }
    }

    public void SaveInteractableObjects(SaveGameObject sGO)
    {
        for (int i = 0; i < GameManager.instance.allInteractableObjects.Count; i++)
        {
            if (GameManager.instance.allInteractableObjects[i] != null)
            {
                sGO.allInteractableObjectNames.Add(GameManager.instance.allInteractableObjects[i].name);
                sGO.allInteractableObjectIsActive.Add(GameManager.instance.allInteractableObjects[i].activeSelf);
            }
            else
            {
                sGO.allInteractableObjectNames.Add(null);
                sGO.allInteractableObjectIsActive.Add(false);
            }
        }

        for (int i = 0; i < GameManager.instance.allInteractableDoors.Count; i++)
        {
            sGO.isDoorOpen.Add(GameManager.instance.allInteractableDoors[i].isOpen);
        }

        for (int i = 0; i < GameManager.instance.allInteractableChests.Count; i++)
        {
            sGO.isChestOpen.Add(GameManager.instance.allInteractableChests[i].isOpen);
        }
    }

    public void SaveBuffsAndDebuffs(SaveGameObject sGO)
    {
        if (DebuffManager.instance.bleedingCoro != null)
        {
            sGO.bleedingDamage = DebuffManager.instance.bleedingDamage;
            sGO.bleedingTimes = DebuffManager.instance.bleedingTimes;
            sGO.currBleedingTimes = DebuffManager.instance.currBleedingTimes;
        }
        else
        {
            sGO.bleedingDamage = -1;
            sGO.bleedingTimes = -1;
            sGO.currBleedingTimes = -1;
        }

        if (DebuffManager.instance.higherSpeedCoro != null)
        {
            sGO.currSBBuffTime = DebuffManager.instance.currSBBuffTime;
            sGO.timerSB = DebuffManager.instance.timerSB;
        }
        else
        {
            sGO.currSBBuffTime = -1;
            sGO.timerSB = -1;
        }

        if (DebuffManager.instance.higherDamageCoro != null)
        {
            sGO.currDBBuffTime = DebuffManager.instance.currDBBuffTime;
            sGO.timerDB = DebuffManager.instance.timerDB;
        }
        else
        {
            sGO.currDBBuffTime = -1;
            sGO.timerDB = -1;
        }

        if (DebuffManager.instance.slowPlayerCoro != null)
        {
            sGO.currSBBuffTime = DebuffManager.instance.currSBBuffTime;
            sGO.timerSB = DebuffManager.instance.timerSB;
        }
        else
        {
            sGO.currSBBuffTime = -1;
            sGO.timerSB = -1;
        }

        if (DebuffManager.instance.lowerStaminaCoro != null)
        {
            sGO.currStDDebuffTime = DebuffManager.instance.currStDDebuffTime;
            sGO.timerStD = DebuffManager.instance.timerStD;

            sGO.currStaminaReduceAmount = DebuffManager.instance.currStaminaReduceAmount;
        }
        else
        {
            sGO.currStDDebuffTime = -1;
            sGO.timerStD = -1;

            sGO.currStaminaReduceAmount = -1;
        }

        if (DebuffManager.instance.lowerStrengthCoro != null)
        {
            sGO.currShDDebuffTime = DebuffManager.instance.currShDDebuffTime;
            sGO.timerShD = DebuffManager.instance.timerShD;

            sGO.currStrengthReduceAmount = DebuffManager.instance.currStrengthReduceAmount;
        }
        else
        {
            sGO.currShDDebuffTime = -1;
            sGO.timerShD = -1;

            sGO.currStrengthReduceAmount = -1;
        }

        if (DebuffManager.instance.lowerArmorCoro != null)
        {
            sGO.currADDebuffTime = DebuffManager.instance.currADDebuffTime;
            sGO.timerAD = DebuffManager.instance.timerAD;

            sGO.currArmorReduceAmount = DebuffManager.instance.currArmorReduceAmount;
        }
        else
        {
            sGO.currADDebuffTime = -1;
            sGO.timerAD = -1;

            sGO.currArmorReduceAmount = -1;
        }
    }

    public void SaveDaytimeAndWeather(SaveGameObject sGO)
    {
        // Daytime
        sGO.changeDaytime = GameManager.instance.changeDaytime;
        sGO.timeOfDay = GameManager.instance.hdrpTOD.TimeOfDay;

        if (GameManager.instance.correspondingCutsceneProfilAtNight == null)
        {
            sGO.nightCSisNull = true;
        }
        else
        {
            sGO.nightCSisNull = false;
        }

        // Weather
        sGO.isRaining = GameManager.instance.hdrpTOD.WeatherActive();
        sGO.currRainingDuration = GameManager.instance.currRainingDuration;
    }

    public void SaveCutscene(SaveGameObject sGO)
    {
        //if (CutsceneManager.instance.currCP == null)
        //{
        //    sGO.cutsceneIsCurrPlaying = CutsceneManager.instance.playableDirector.playableAsset != null
        //        && CutsceneManager.instance.playableDirector.playableGraph.IsPlaying();

        //    Debug.Log("1");
        //}
        //else
        //{
        //    sGO.cutsceneIsCurrPlaying = CutsceneManager.instance.playableDirector.playableAsset != null && CutsceneManager.instance.playableDirector.playableGraph.IsPlaying()
        //        && CutsceneManager.instance.currCP.canPauseWhilePlaying;

        //    Debug.Log("2");
        //}

        sGO.currCSID = -1;

        //Debug.Log(CutsceneManager.instance.playableDirector.playableGraph.IsPlaying());

        if (GameManager.instance.pausedCutsceneTime != -1)
        {
            for (int i = 0; i < CutsceneManager.instance.allCSWAllowedPausing.Length; i++)
            {
                if (CutsceneManager.instance.allCSWAllowedPausing[i].cutscene == CutsceneManager.instance.playableDirector.playableAsset)
                {
                    sGO.currCSID = i;
                }
            }
        }

        sGO.currCutsceneTime = CutsceneManager.instance.playableDirector.time;
    }

    public void SaveOptions(SaveGameObject sGO)
    {
        sGO.useTutorial = OptionManager.instance.tutorialToggle.isOn;

        // Audio
        sGO.masterSlValue = OptionManager.instance.masterSlider.value;
        sGO.environmentSlValue = OptionManager.instance.environmentSlider.value;
        sGO.voiceSlValue = OptionManager.instance.voiceSlider.value;
        sGO.musicSlValue = OptionManager.instance.musicSlider.value;
        sGO.sfxSlValue = OptionManager.instance.sfxSlider.value;

        // Video
        sGO.isWindowed = OptionManager.instance.windowModeToggle.isOn;
        sGO.resolutionDDValue = OptionManager.instance.resolutionDropdown.value;
        sGO.displaySubtitle = StartScreenManager.instance.showSubtitle;

        // Controls
        sGO.camSensiSlValue = OptionManager.instance.cameraSensiSlider.value;
        sGO.mouseSensiSlValue = OptionManager.instance.mouseSensiSlider.value;

        // - Keys
        for (int i = 0; i < OptionManager.instance.keyTxts.Length; i++)
        {
            sGO.keyTxtStrings.Add(OptionManager.instance.keyTxts[i].text);
        }
    }
    #endregion

    #region Loading
    public void LoadGameData(SaveGameObject sGO)
    {
        GameManager.instance.playtimeInSeconds = sGO.playtimeInSeconds;
    }

    public void LoadTutorial(SaveGameObject sGO)
    {
        //GameManager.instance.displayTutorial = sGO.displayTutorial;

        TutorialManager.instance.allCompletedTutorials.Clear();

        for (int i = 0; i < TutorialManager.instance.allTutorialProfiles.Count; i++)
        {
            if (sGO.allCompletedTutorialNumbers.Contains(TutorialManager.instance.allTutorialProfiles[i].tutorialNumber))
            {
                TutorialManager.instance.allCompletedTutorials.Add(TutorialManager.instance.allTutorialProfiles[i]);
            }
        }
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

        PlayerValueManager.instance.CurrHP = sGO.currPlayerHealth;
        PlayerValueManager.instance.currStamina = sGO.currPlayerStamina;
    }

    public void LoadInventoryAndItemDatabase(SaveGameObject sGO)
    {
        for (int i = 0; i < InventoryManager.instance.inventory.database.items.Length; i++)
        {
            InventoryManager.instance.inventory.database.items[i].isNew = sGO.itemIsNew[i];
        }

        for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
        {
            HotbarManager.instance.allHotbarSlotBtn[i].ClearHotbarSlot();
        }

        EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
        EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
        EquippingManager.instance.glovesES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
        EquippingManager.instance.pauldronsES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
        EquippingManager.instance.poleynsES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

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

        if (sGO.usesGloves)
        {
            EquippingManager.instance.glovesES.GetComponent<ClickableInventorySlot>().EquipItemToEquipment(EquippingManager.instance.glovesIB, 1);
        }

        if (sGO.usesPauldrons)
        {
            EquippingManager.instance.pauldronsES.GetComponent<ClickableInventorySlot>().EquipItemToEquipment(EquippingManager.instance.pauldronsIB, 1);
        }

        if (sGO.usesPoleyns)
        {
            EquippingManager.instance.poleynsES.GetComponent<ClickableInventorySlot>().EquipItemToEquipment(EquippingManager.instance.poleynsIB, 1);
        }

        InventoryManager.instance.weightTxt.text = InventoryManager.instance.currHoldingWeight + " / " + InventoryManager.instance.maxHoldingWeight;
        ShopManager.instance.weightTxt.text = InventoryManager.instance.currHoldingWeight + " / " + InventoryManager.instance.maxHoldingWeight;
    }

    public void LoadMissions(SaveGameObject sGO)
    {
        MissionManager.instance.allCurrAcceptedMissions.Clear();
        MissionManager.instance.allCurrOpenNotAcceptedMissions.Clear();

        int taskNumber = 0;

        int killNumber = 0;
        int collectNumber = 0;
        int examineNumber = 0;

        for (int i = 0; i < MissionManager.instance.allMissions.Count; i++)
        {
            for (int y = 0; y < MissionManager.instance.allMissions[i].allMissionTasks.Length; y++)
            {
                if (MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.canNormallyBeDisplayed)
                {
                    MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.canBeDisplayed = true;
                }
                else
                {
                    MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.canBeDisplayed = false;
                }

                MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyCollected = 0;
                MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyKilled = 0;
                MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyExamined = 0;
                MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.currGainedPoints = 0;

                MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskCompleted = false;
            }
        }

        for (int i = 0; i < MissionManager.instance.allMissions.Count; i++)
        {
            if (sGO.allCurrAcceptedMissionNumbers.Contains(i))
            {
                MissionManager.instance.allCurrAcceptedMissions.Add(MissionManager.instance.allMissions[i]);

                for (int y = 0; y < MissionManager.instance.allMissions[i].allMissionTasks.Length; y++)
                {
                    MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskCompleted = sGO.allAcceptedMissionTaskIsCompletedStates[y];
                    MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.canBeDisplayed = sGO.allAcceptedMissionTaskIsDisplayableStates[y];

                    taskNumber += 1;

                    // Load Mission Task Progress
                    if (MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.kill)
                    {
                        MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyKilled = sGO.allCurrKillMissionKills[killNumber];

                        killNumber += 1;
                    }
                    else if (MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                    {
                        MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyCollected = sGO.allCurrCollectMissionProgresses[collectNumber];

                        collectNumber += 1;
                    }
                    else if (MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.examine)
                    {
                        MissionManager.instance.allMissions[i].allMissionTasks[y].mTB.howManyAlreadyExamined = sGO.allCurrExamineMissionProgresses[examineNumber];

                        examineNumber += 1;
                    }
                }

                if (MissionManager.instance.allMissions[i].missionName == sGO.currentMainMissionName)
                {
                    //if (UIManager.missionToDisplay == null && missionToAdd.missionType == MissionBaseProfile.MissionType.main)
                    //{
                    UIManager.missionToDisplay = MissionManager.instance.allMissions[i];

                    UIManager.instance.CreateMissionDisplay();

                    //UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.addedMissionString;
                    //UIAnimationHandler.instance.addedMissionTxt.text = MissionManager.instance.allCurrAcceptedMissions[i].missionName;
                    //UIAnimationHandler.instance.AnimateAddedNewMissionMessage();

                    MinimapManager.instance.CheckAllMinimapSymbols();
                    //}
                }
            }

            if (sGO.allCurrOpenNotAcceptedMissionNumbers.Contains(i))
            {
                MissionManager.instance.allCurrOpenNotAcceptedMissions.Add(MissionManager.instance.allMissions[i]);
            }
        }

        GameManager.instance.alreadyPlayedAgainstKilian = sGO.alreadyPlayedAgainstKilian;
    }

    public void LoadNPCs(SaveGameObject sGO)
    {
        //if (sGO.isNPCVisible.Count > GameManager.instance.allVillageNPCs.Count)
        //{
        //    GameManager.instance.allVillageNPCs[GameManager.instance.allVillageNPCs.Count].gameObject.SetActive(false);
        //    GameManager.instance.allVillageNPCs.Remove(GameManager.instance.allVillageNPCs[GameManager.instance.allVillageNPCs.Count]);
        //}

        for (int i = 0; i < GameManager.instance.allVillageNPCs.Count; i++)
        {
            if (GameManager.instance.allWalkingNPCs.Contains(GameManager.instance.allVillageNPCs[i]))
            {
                GameManager.instance.allVillageNPCs[i].animator.speed = 0;

                GameManager.instance.allVillageNPCs[i].GetComponent<NavMeshAgent>().enabled = false;
            }

            GameManager.instance.allVillageNPCs[i].transform.localPosition = sGO.allNPCPositions[i];
            GameManager.instance.allVillageNPCs[i].transform.localRotation = sGO.allNPCRotations[i];

            GameManager.instance.allVillageNPCs[i].gameObject.SetActive(sGO.isNPCVisible[i]);

            //if (GameManager.instance.allWalkingNPCs.Contains(GameManager.instance.allVillageNPCs[i]))
            //{
            //    for (int y = 0; y < GameManager.instance.allVillageNPCs[i].allCorrWaypoints.Count; y++)
            //    {
            //        if (GameManager.instance.allVillageNPCs[i].allCorrWaypoints[y].name == sGO.currWaypointNames[i])
            //        {
            //            GameManager.instance.allVillageNPCs[i].SetNewWaypoint(GameManager.instance.allVillageNPCs[i].allCorrWaypoints[y]);
            //        }
            //    }
            //}
        }

        // Load Waypoints
        for (int i = 0; i < GameManager.instance.allWalkingNPCs.Count; i++)
        {
            for (int y = 0; y < GameManager.instance.allWalkingNPCs[i].allCorrWaypoints.Count; y++)
            {
                if (GameManager.instance.allWalkingNPCs[i].allCorrWaypoints[y].name == sGO.currWaypointNames[i])
                {
                    GameManager.instance.allWalkingNPCs[i].SetNewWaypointWithoutStopping(GameManager.instance.allWalkingNPCs[i].allCorrWaypoints[y]);

                    GameManager.instance.allWalkingNPCs[i].animator.speed = 1;

                    GameManager.instance.allWalkingNPCs[i].GetComponent<NavMeshAgent>().enabled = true;
                }
            }
        }

        //for (int i = 0; i < GameManager.instance.allWalkingNPCs.Count; i++)
        //{
        //    GameManager.instance.allWalkingNPCs[i].nPCBaseMesh.transform.localPosition = Vector3.zero;
        //    GameManager.instance.allWalkingNPCs[i].nPCBaseMesh.transform.localRotation = Quaternion.identity;
        //}
    }

    public void LoadEnemies(SaveGameObject sGO)
    {
        //for (int i = 0; i < GameManager.instance.allMeleeEnemies.Count; i++)
        //{
        //    GameManager.instance.allMeleeEnemies[i].transform.position = sGO.allMeleeEnemyPositions[i];
        //    GameManager.instance.allMeleeEnemies[i].transform.rotation = sGO.allMeleeEnemyRotations[i];
        //}

        //for (int i = 0; i < GameManager.instance.allArcherEnemies.Count; i++)
        //{
        //    GameManager.instance.allArcherEnemies[i].transform.position = sGO.allArcherEnemyPositions[i];
        //    GameManager.instance.allArcherEnemies[i].transform.rotation = sGO.allArcherEnemyRotations[i];
        //}

        for (int i = 0; i < GameManager.instance.allMeleeEnemies.Count; i++)
        {
            GameManager.instance.allMeleeEnemies[i].gameObject.SetActive(sGO.enemyIsActive[i]);
        }
    }

    public void LoadInteractableObjects(SaveGameObject sGO)
    {
        for (int i = 0; i < GameManager.instance.allInteractableObjects.Count; i++)
        {
            if (GameManager.instance.allInteractableObjects[i] == null)
            {
                break;
            }

            if (!sGO.allInteractableObjectNames.Contains(GameManager.instance.allInteractableObjects[i].name))
            {
                //if (GameManager.instance.allInteractableObjects[i] != null)
                //{
                    GameManager.instance.allInteractableObjects[i].gameObject.SetActive(false);
                    //GameManager.instance.allInteractableObjects.Remove(GameManager.instance.allInteractableObjects[i]);
                //}
            }
            else
            {
                GameManager.instance.allInteractableObjects[i].gameObject.SetActive(sGO.allInteractableObjectIsActive[i]);
            }
        }

        for (int i = 0; i < GameManager.instance.allInteractableDoors.Count; i++)
        {
            if (sGO.isDoorOpen[i] == true)
            {
                Debug.Log("OPEN");
                GameManager.instance.allInteractableDoors[i].OpenDoor(Vector3.zero);
            }
            //GameManager.instance.allInteractableDoors[i].isOpen = sGO.isDoorOpen[i];
        }

        for (int i = 0; i < GameManager.instance.allInteractableChests.Count; i++)
        {
            if (sGO.isChestOpen[i] == true)
            {
                Debug.Log("OPEN");
                GameManager.instance.allInteractableChests[i].isOpen = true;

                GameManager.instance.allInteractableChests[i].gameObject.GetComponent<Animator>().enabled = true;
            }
        }
    }

    public void LoadBuffsAndDebuffs(SaveGameObject sGO)
    {
        DebuffManager.instance.StopAllBuffs();
        DebuffManager.instance.StopAllDebuffs();

        if (sGO.currBleedingTimes > -1)
        {
            DebuffManager.instance.currBleedingTimes = sGO.currBleedingTimes;

            DebuffManager.instance.Bleeding(sGO.bleedingTimes, sGO.bleedingDamage, false);

            DebuffManager.instance.bleedingImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            DebuffManager.instance.bleedingImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        if (sGO.timerSB > -1)
        {
            DebuffManager.instance.timerSB = sGO.timerSB;

            DebuffManager.instance.SpeedUpPlayer(sGO.currSBBuffTime, false);
        }
        else
        {
            DebuffManager.instance.higherSpeedImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        if (sGO.timerDB > -1)
        {
            DebuffManager.instance.timerDB = sGO.timerDB;

            DebuffManager.instance.SetPlayerDamageHigher(sGO.currDBBuffTime, false);
        }
        else
        {
            DebuffManager.instance.higherDamageImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        if (sGO.timerSD > -1)
        {
            DebuffManager.instance.timerSD = sGO.timerSD;

            DebuffManager.instance.SlowPlayer(sGO.currSDDebuffTime, false);
        }
        else
        {
            DebuffManager.instance.slowPlayerImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        if (sGO.timerStD > -1)
        {
            DebuffManager.instance.timerStD = sGO.timerStD;

            DebuffManager.instance.LowerMaxStamina(sGO.currStrengthReduceAmount, sGO.currStDDebuffTime, false);
        }
        else
        {
            DebuffManager.instance.lowerStaminaImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        if (sGO.timerShD > -1)
        {
            DebuffManager.instance.timerShD = sGO.timerShD;

            DebuffManager.instance.LowerStrength(sGO.currStrengthReduceAmount, sGO.currShDDebuffTime, false);
        }
        else
        {
            DebuffManager.instance.lowerStrengthImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        if (sGO.timerAD > -1)
        {
            DebuffManager.instance.timerAD = sGO.timerAD;

            DebuffManager.instance.LowerMaxArmor(sGO.currArmorReduceAmount, sGO.currADDebuffTime, false);
        }
        else
        {
            DebuffManager.instance.lowerArmorImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }
    }

    public void LoadDaytimeAndWeather(SaveGameObject sGO)
    {
        //if (continuePath.Contains("_D2") || continuePath.Contains("_D1") || LoadingScreen.currLSP.sceneToLoadIndex == 3 || LoadingScreen.currLSP.sceneToLoadIndex == 4)
        //{
        //    if (sGO.changeDaytime)
        //    {
        //        GameManager.instance.correspondingCutsceneProfilAtNight = null;

        //        GameManager.instance.hdrpTOD.m_timeOfDayMultiplier = 1;
        //    }
        //    else
        //    {
        //        GameManager.instance.correspondingCutsceneProfilAtNight = GameManager.instance.cutsceneProfileAtNightHolder;

        //        GameManager.instance.hdrpTOD.m_timeOfDayMultiplier = 0;
        //    }

        //    GameManager.instance.changeDaytime = sGO.changeDaytime;

        //    GameManager.instance.hdrpTOD.TimeOfDay = sGO.timeOfDay;

        //    GameManager.instance.currRainingDuration = 0;

        if (!continuePath.Contains("_D2") && !continuePath.Contains("_D1") || LoadingScreen.currLSP != null && LoadingScreen.currLSP.sceneToLoadIndex != 3 && LoadingScreen.currLSP.sceneToLoadIndex != 4)
        {
            if (sGO.changeDaytime)
            {
                if (sGO.nightCSisNull)
                {
                    GameManager.instance.correspondingCutsceneProfilAtNight = null;
                }
                else
                {
                    GameManager.instance.correspondingCutsceneProfilAtNight = GameManager.instance.cutsceneProfileAtNightHolder;
                }

                if (GameManager.instance.shouldChangeTime)
                {
                    GameManager.instance.hdrpTOD.m_timeOfDayMultiplier = 1;
                }
            }
            else
            {
                GameManager.instance.correspondingCutsceneProfilAtNight = GameManager.instance.cutsceneProfileAtNightHolder;

                GameManager.instance.hdrpTOD.m_timeOfDayMultiplier = 0;
            }

            GameManager.instance.changeDaytime = sGO.changeDaytime;

            GameManager.instance.hdrpTOD.TimeOfDay = sGO.timeOfDay;

            if (GameManager.instance.shouldChangeTime)
            {
                if (sGO.isRaining)
                {
                    GameManager.instance.hdrpTOD.StartWeather(0);
                }

                GameManager.instance.currRainingDuration = sGO.currRainingDuration;
            }
        }
        else
        {
            GameManager.instance.changeDaytime = false;
            GameManager.instance.correspondingCutsceneProfilAtNight = null;

            GameManager.instance.hdrpTOD.UseWeatherFX = false;
        }
    }

    public void LoadCutscene(SaveGameObject sGO)
    {
        Debug.Log("33333333333333333333333333333333333333333333333333333333");

        if (sGO.currCutsceneTime > 0)
        {
            CutsceneManager.instance.currCP = CutsceneManager.instance.allCSWAllowedPausing[sGO.currCSID];
            CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.allCSWAllowedPausing[sGO.currCSID].cutscene;

            CutsceneManager.instance.playableDirector.Play();

            CutsceneManager.instance.playableDirector.time = sGO.currCutsceneTime;

            Debug.Log(sGO.currCutsceneTime);
        }
    }

    public void LoadOptions(SaveGameObject sGO)
    {
        OptionManager.instance.tutorialToggle.isOn = sGO.useTutorial;
        StartScreenManager.instance.showTutorialToggle.isOn = !OptionManager.instance.tutorialToggle.isOn;

        OptionManager.instance.masterSlider.value = sGO.masterSlValue;
        OptionManager.instance.environmentSlider.value = sGO.environmentSlValue;
        OptionManager.instance.voiceSlider.value = sGO.voiceSlValue;
        OptionManager.instance.musicSlider.value = sGO.musicSlValue;
        OptionManager.instance.sfxSlider.value = sGO.sfxSlValue;

        OptionManager.instance.windowModeToggle.isOn = sGO.isWindowed;
        OptionManager.instance.resolutionDropdown.value = sGO.resolutionDDValue;
        OptionManager.instance.subtitleToggle.isOn = sGO.displaySubtitle;
        StartScreenManager.instance.showSubtitle = sGO.displaySubtitle;

        OptionManager.instance.cameraSensiSlider.value = sGO.camSensiSlValue;
        OptionManager.instance.mouseSensiSlider.value = sGO.mouseSensiSlValue;

        OptionManager.instance.MasterSliderOnValueChange();
        OptionManager.instance.EnvironmentSliderOnValueChange();
        OptionManager.instance.VoiceSliderOnValueChange();
        OptionManager.instance.MusicSliderOnValueChange();
        OptionManager.instance.SFXSliderOnValueChange();

        OptionManager.instance.CameraSensitivitySliderOnValueChange();
        OptionManager.instance.MouseSensitivitySliderOnValueChange();

        //OptionManager.instance.controllerToggle.isOn = false;

        //for (int i = 0; i < sGO.keyTxtStrings.Count; i++)
        //{
        //    OptionManager.instance.keyTxts[i].text = sGO.keyTxtStrings[i];
        //}
    }
    #endregion
}
