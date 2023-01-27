using Cinemachine;
using ProceduralWorlds.HDRPTOD;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject TEST;

    [HideInInspector] public float playtimeInSeconds;

    public GameObject playerGOParent;
    public GameObject playerGO;

    public ItemBaseProfile iBPOfEscapeRope;

    public bool playedTheGameThrough = false; // Soll true sein, sobald der Spieler das Spiel zum ersten Mal durchgespielt hat.

    public List<NPC> allVillageNPCs;
    public List<NPC> allWalkingNPCs;

    // WIP ------------------------------------- > Muss noch gespeichert werden!
    public List<Merchant> allMerchants;
    public List<NPCScreamingHandler> allNPCScreamingHandler;

    public List<GameObject> allInteractableObjects;
    public List<Door> allInteractableDoors;
    public List<MeleeEnemyKi> allMeleeEnemies;

    //public BeerScreenMissionButton bSMButton;
    public GameObject readBookOrNoteScreen;
    public static ItemBaseProfile currBookOrNote;

    public bool isNight = false; // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )
    public CutsceneProfile correspondingCutsceneProfilAtNight; // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )

    public GameObject cutsceneBlackFadeGO;
    public GameObject interactCanvasasParentGO;
    public GameObject mapGO;
    public GameObject hotbarGO;
    public GameObject playerStatsGO;

    public CinemachineVirtualCamera normalPlayerFollowCamCVC;

    public GameObject pauseMenuScreen;
    public bool gameIsPaused = false;

    public GameObject arrowHUDDisplayGO;

    public GameObject canvasParentGO;

    public GameObject prickMGUI;
    public GameObject gTCMGUI;

    [Header("Day-Night + Weather")]
    public HDRPTimeOfDay hdrpTOD;

    public bool changeDaytime = false;

    [Header("Saving/Loading")]
    public GameObject saveGameSlotPrefab;
    public GameObject saveGameSlotParentObj;

    public float autoSaveTime;
    public float passedTimeTillLastSave = 0;

    [Header("Tutorial")]
    public TutorialBaseProfile meleeTutorial;
    public TutorialBaseProfile rangedTutorial;

    public bool displayTutorial = true;

    [Header("Pausing Game")]
    public double pausedCutsceneTime;

    [Header("Player AFK")]
    public AudioSource playerAudioSource;
    public AudioClip[] allAFKPlayerAudioClips;

    public bool isAFK = false;

    public float timeTillAfk;
    public float timeSinceLastButtonPressed = 0;

    [Header("NPC One-Liner")]
    public NPCOneLinerProfile quietMaleOL;
    public NPCOneLinerProfile[] allMaleProfiles;

    public NPCOneLinerProfile quietFemaleOL;
    public NPCOneLinerProfile[] allFemaleProfiles;

    [Header("Cutscenes To Reset On New Game")]
    public List<CutsceneProfile> cutscenesToReset;

    [Header("New-Game-Values")]
    public bool alreadySpokeWThava = false;
    public bool alreadySpokeWOskar = false;
    public bool alreadySpokeWMorrin = false;

    public void Awake()
    {
        //if (instance == null)
        //{
            instance = this;

            //DontDestroyOnLoad(this.gameObject);
            //DontDestroyOnLoad(canvasParentGO);
        //}
        //else
        //{
        //    Destroy(canvasParentGO);
        //    Destroy(this.gameObject);
        //}

        for (int i = 0; i < cutscenesToReset.Count; i++)
        {
            cutscenesToReset[i].alreadyPlayedCutscene = false;
        }

        //BeerScreenMissionButton.instance = bSMButton;
    }

    public void Start()
    {
        pauseMenuScreen = LoadingScreen.instance.startScreenMainUI;
        pauseMenuScreen.SetActive(false);
    }

    //public void Start()
    //{
    //    CreateSaveGameSlotButton();
    //}

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    pauseMenuScreen.SetActive(false);
        //    Debug.Log("gzhjklö");
        //}

        // Handle Player-AFK
        if (ThirdPersonController.instance.canMove)
        {
            if (!Input.anyKey && !isAFK)
            {
                timeSinceLastButtonPressed += Time.deltaTime;

                if (timeSinceLastButtonPressed >= timeTillAfk)
                {
                    isAFK = true;

                    var rndmAudioNumber = Random.Range(0, allAFKPlayerAudioClips.Length);
                    playerAudioSource.clip = allAFKPlayerAudioClips[rndmAudioNumber];

                    playerAudioSource.volume = 1;
                    playerAudioSource.Play();
                }
            }
            else if (Input.anyKey && isAFK)
            {
                isAFK = false;

                //playerAudioSource.Stop();

                StartCoroutine(FadePlayerAFKAudioToZero());

                timeSinceLastButtonPressed = 0;
            }
            else if (Input.anyKey && timeSinceLastButtonPressed > 0)
            {
                timeSinceLastButtonPressed = 0;
            }
        }

        // Play-Time
        if (!pauseMenuScreen.activeSelf)
        {
            playtimeInSeconds += Time.deltaTime;

            if (passedTimeTillLastSave < autoSaveTime)
            {
                passedTimeTillLastSave += Time.deltaTime;

                if (passedTimeTillLastSave >= autoSaveTime)
                {
                    passedTimeTillLastSave = 0;

                    SaveSystem.instance.SaveAutomatic();
                }
            }
        }

        //if (ThirdPersonController.instance._animator.GetBool("UsingHBItem"))
        //{
        //    ThirdPersonController.instance._animator.SetLayerWeight(1, ThirdPersonController.instance._animator.GetLayerWeight(1) + Time.deltaTime);
        //}
        //else if (ThirdPersonController.instance._animator.GetLayerWeight(1) > 0 && !ThirdPersonController.instance._animator.GetBool("UsingHBItem")
        //    && !ThirdPersonController.instance._animator.GetBool("GrabItem") && !ThirdPersonController.instance._animator.GetBool("GrabGroundItem")
        //    && !ThirdPersonController.instance.isRolling)
        //{
        //    ThirdPersonController.instance._animator.SetLayerWeight(1, 0);
        //}

        // Tombstone
        if (Input.GetKeyDown(KeyCode.Escape) && UIManager.instance.readTombstoneUI.activeSelf && !pauseMenuScreen.activeSelf)
        {
            UIManager.instance.readTombstoneUI.SetActive(false);
            gameIsPaused = false;
        }

        // Open/Close Inventory
        if (Input.GetKeyDown(KeyCode.I) && !ShopManager.instance.shopScreen.activeSelf)
        {
            OpenInventory();

            if (MissionLogScreenHandler.instance != null)
            {
                MissionLogScreenHandler.instance.DisplayMissions();
            }
        }

        // Day-Night
        if (!gameIsPaused && changeDaytime)
        {
            if (hdrpTOD.ConvertTimeOfDay() >= 17.7f)
            {
                if (correspondingCutsceneProfilAtNight != null)
                {
                    CutsceneManager.instance.currCP = correspondingCutsceneProfilAtNight;
                    CutsceneManager.instance.playableDirector.playableAsset = correspondingCutsceneProfilAtNight.cutscene;
                    CutsceneManager.instance.playableDirector.Play();

                    correspondingCutsceneProfilAtNight = null; // ---------------------------------> NEEDS TO BE SAVED
                }
            }
        }

        Debug.Log(hdrpTOD.ConvertTimeOfDay());

        if (pauseMenuScreen != null && !TutorialManager.instance.bigTutorialUI.activeSelf/* && TutorialManager.currTBP == null*/  && !ShopManager.instance.shopScreen.activeSelf && !ShopManager.instance.mainShopScreen.activeSelf)
        {
            Debug.Log(CutsceneManager.instance.playableDirector.playableGraph.IsValid());
            //Debug.Log(CutsceneManager.instance.playableDirector.playableGraph.IsPlaying());
            //Debug.Log(!CutsceneManager.instance.currCP.cantBeSkipped);
            Debug.Log(CutsceneManager.instance.currCP + "56tzhejklöf");

            if (StartScreenManager.instance.areYouSureExitGameScreen.activeSelf)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !readBookOrNoteScreen.activeSelf && !ShopManager.instance.shopScreen.activeSelf/* &&*/ /*!CutsceneManager.instance.playableDirector.playableGraph.IsV*//*alid()*/)
            {
                if (CutsceneManager.instance.currCP == null)
                {
                    Debug.Log(CutsceneManager.instance.currCP + "56tzhejklöf");

                    LoadingScreen.instance.gameObject.SetActive(!pauseMenuScreen.activeSelf);
                    LoadingScreen.instance.startScreenMainUIButtonParent.SetActive(!pauseMenuScreen.activeSelf);
                    pauseMenuScreen.SetActive(!pauseMenuScreen.activeSelf);

                    FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, !pauseMenuScreen.activeSelf);

                    if (pauseMenuScreen.activeSelf)
                    {
                        PauseGame();
                    }
                    else
                    {
                        ContinueGame();
                    }
                }
                else if (CutsceneManager.instance.currCP != null && CutsceneManager.instance.playableDirector.playableGraph.IsValid() && !CutsceneManager.instance.playableDirector.playableGraph.IsPlaying()
                    || CutsceneManager.instance.currCP != null && !CutsceneManager.instance.currCP.cantBeSkipped)
                {
                    Debug.Log(CutsceneManager.instance.currCP + "56tzhejklöf");

                    LoadingScreen.instance.gameObject.SetActive(!pauseMenuScreen.activeSelf);
                    LoadingScreen.instance.startScreenMainUIButtonParent.SetActive(!pauseMenuScreen.activeSelf);
                    pauseMenuScreen.SetActive(!pauseMenuScreen.activeSelf);

                    FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, !pauseMenuScreen.activeSelf);

                    if (pauseMenuScreen.activeSelf)
                    {
                        PauseGame();
                    }
                    else
                    {
                        ContinueGame();
                    }
                }
            }
            //else
            //{
            //    Debug.Log(readBookOrNoteScreen.activeSelf);
            //    Debug.Log(ShopManager.instance.shopScreen.activeSelf);
            //}
        }

        // Close Tutorial ( Big )
        if (gameIsPaused && TutorialManager.currTBP != null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                TutorialManager.instance.CloseBigTutorial();
            }
        }

        // Close Read-Book- Or Note-Screen
        if (readBookOrNoteScreen != null)
        {
            if (readBookOrNoteScreen.activeSelf)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    readBookOrNoteScreen.SetActive(false);

                    if (!currBookOrNote.hasBeenRead && currBookOrNote.cutsceneToPlayAfterCloseReadScreen != null)
                    {
                        if (currBookOrNote.corresspondingMissionTask != null && currBookOrNote.corresspondingMissionTask.canBeDisplayed)
                        {
                            CutsceneManager.instance.currCP = currBookOrNote.cutsceneToPlayAfterCloseReadScreen;
                            CutsceneManager.instance.playableDirector.playableAsset = currBookOrNote.cutsceneToPlayAfterCloseReadScreen.cutscene;
                            CutsceneManager.instance.playableDirector.Play();

                            currBookOrNote.hasBeenRead = true;
                        }
                        else if (currBookOrNote.corresspondingMissionTask == null)
                        {
                            CutsceneManager.instance.currCP = currBookOrNote.cutsceneToPlayAfterCloseReadScreen;
                            CutsceneManager.instance.playableDirector.playableAsset = currBookOrNote.cutsceneToPlayAfterCloseReadScreen.cutscene;
                            CutsceneManager.instance.playableDirector.Play();

                            currBookOrNote.hasBeenRead = true;
                        }
                    }
                }
            }
        }
    }

    public IEnumerator FadePlayerAFKAudioToZero()
    {
        float currentTime = 0;

        float start = playerAudioSource.volume;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            playerAudioSource.volume = Mathf.Lerp(start, 0, currentTime / 1f);

            yield return null;
        }

        playerAudioSource.Stop();

        yield break;
    }

    public void PauseGame()
    {
        // Player
        playerGO.GetComponent<Animator>().speed = 0;

        if (LoadingScreen.instance.gameObject.activeSelf)
        {
            LoadingScreen.instance.gameObject.SetActive(true);
            pauseMenuScreen.SetActive(true);
        }

        // NPCs
        for (int i = 0; i < allVillageNPCs.Count; i++)
        {
            allVillageNPCs[i].animator.speed = 0;
        }

        for (int i = 0; i < allWalkingNPCs.Count; i++)
        {
            allWalkingNPCs[i].navMeshAgent.isStopped = true;
        }

        for (int i = 0; i < allMerchants.Count; i++)
        {
            if (allMerchants[i].normalMerchantObj != null)
            {
                allMerchants[i].normalMerchantObj.GetComponent<Animator>().speed = 0;
            }
        }

        for (int i = 0; i < allNPCScreamingHandler.Count; i++)
        {
            allNPCScreamingHandler[i].nPCAudioSource.Pause();
        }

        // Enemies
        for (int i = 0; i < allMeleeEnemies.Count; i++)
        {
            allMeleeEnemies[i].SetAnimatorSpeed(0f);

            allMeleeEnemies[i].GetComponent<NavMeshAgent>().isStopped = true;
        }

        if (PrickMinigameManager.instance != null)
        {
            PrickMinigameManager.instance.prickCardAnimator.speed = 0;
        }

        if (CutsceneManager.instance.playableDirector.playableAsset != null && CutsceneManager.instance.playableDirector.playableGraph.IsValid())
        {
            pausedCutsceneTime = CutsceneManager.instance.playableDirector.time;
            CutsceneManager.instance.playableDirector.Pause();
        }

        if (TutorialManager.instance.smallTutorialUI.activeSelf)
        {
            TutorialManager.instance.animator.speed = 0;
        }

        hdrpTOD.m_timeOfDayMultiplier = 0;

        UIAnimationHandler.instance.addedMissionAnimator.speed = 0;
        UIAnimationHandler.instance.missionDisplayAnimator.speed = 0;

        //StartScreenManager.instance.mainObjectAnimator.enabled = true;

        gameIsPaused = true;
    }

    public void ContinueGame()
    {
        SaveSystem.instance.SaveOptions();

        // Player
        playerGO.GetComponent<Animator>().speed = 1;

        if (!LoadingScreen.instance.gameObject.activeSelf)
        {
            LoadingScreen.instance.gameObject.SetActive(false);
            pauseMenuScreen.SetActive(false);
        }

        // NPCs
        for (int i = 0; i < allVillageNPCs.Count; i++)
        {
            allVillageNPCs[i].animator.speed = 1;
        }

        for (int i = 0; i < allWalkingNPCs.Count; i++)
        {
            allWalkingNPCs[i].navMeshAgent.isStopped = false;
        }

        for (int i = 0; i < allMerchants.Count; i++)
        {
            if (allMerchants[i].normalMerchantObj != null)
            {
                allMerchants[i].normalMerchantObj.GetComponent<Animator>().speed = 1;
            }
        }

        for (int i = 0; i < allNPCScreamingHandler.Count; i++)
        {
            if (allNPCScreamingHandler[i].isPlayingAudio)
            {
                //allNPCScreamingHandler[i].nPCAudioSource.clip = null;
                allNPCScreamingHandler[i].nPCAudioSource.UnPause();
            }
        }

        // Enemies
        for (int i = 0; i < allMeleeEnemies.Count; i++)
        {
            allMeleeEnemies[i].SetAnimatorSpeed(1f);

            allMeleeEnemies[i].GetComponent<NavMeshAgent>().isStopped = false;
        }

        if (PrickMinigameManager.instance != null)
        {
            PrickMinigameManager.instance.prickCardAnimator.speed = 1;
        }

        if (CutsceneManager.instance.playableDirector.playableAsset != null && CutsceneManager.instance.playableDirector.playableGraph.IsValid())
        {
            CutsceneManager.instance.playableDirector.Play();
            CutsceneManager.instance.playableDirector.time = pausedCutsceneTime;
        }

        if (TutorialManager.instance.smallTutorialUI.activeSelf)
        {
            TutorialManager.instance.animator.speed = 1;
        }

        if (changeDaytime)
        {
            hdrpTOD.m_timeOfDayMultiplier = 2;
        }

        UIAnimationHandler.instance.addedMissionAnimator.speed = 1;
        UIAnimationHandler.instance.missionDisplayAnimator.speed = 1;

        StartScreenManager.instance.mainAnimator.enabled = false;

        gameIsPaused = false;
    }

    public void OpenInventory()
    {
        InventoryManager.instance.inventoryScreen.SetActive(!InventoryManager.instance.inventoryScreen.activeSelf);

        if (InventoryManager.instance.inventoryScreen.activeSelf)
        {
            ThirdPersonController.instance._animator.Play("Inventory Pose");

            Debug.Log("fghbj");
        }
        else
        {
            ThirdPersonController.instance._animator.enabled = false;
            ThirdPersonController.instance._animator.Rebind();
            ThirdPersonController.instance._animator.enabled = true;

            FightingActions.instance.GetWeapon();

            //Debug.Log("fghbj2");
        }

        ThirdPersonController.instance.canMove = !InventoryManager.instance.inventoryScreen.activeSelf;
        //InventoryManager.instance.DisplayItemsOfCategory();

        for (int i = 0; i < InventoryManager.instance.allInvCategoryButton.Count; i++)
        {
            if (InventoryManager.currIBP != null)
            {
                if (!InventoryManager.currIBP.neededForMissions)
                {
                    if (InventoryManager.instance.allInvCategoryButton[i].itemTypeToDisplay != ItemBaseProfile.ItemType.none)
                    {
                        if (InventoryManager.instance.allInvCategoryButton[i].itemTypeToDisplay == InventoryManager.currIBP.itemType)
                        {
                            InventoryManager.instance.allInvCategoryButton[i].ChangeCurrentInvItemCategory();
                        }
                    }
                }
                else
                {
                    if (InventoryManager.instance.allInvCategoryButton[i].itemTypeToDisplay == ItemBaseProfile.ItemType.none)
                    {
                        InventoryManager.instance.allInvCategoryButton[i].ChangeCurrentInvItemCategory();
                    }
                }
            }
            else
            {
                if (InventoryManager.instance.allInvCategoryButton[i].itemTypeToDisplay == ItemBaseProfile.ItemType.food)
                {
                    InventoryManager.instance.allInvCategoryButton[i].ChangeCurrentInvItemCategory();
                }
            }
        }

        FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, !InventoryManager.instance.inventoryScreen.activeSelf);

        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        if (!InventoryManager.instance.inventoryScreen.activeSelf)
        {
            if (EquippingManager.instance.leftWeaponES.currEquippedItem != null)
            {
                if (EquippingManager.instance.leftWeaponES.currEquippedItem.weaponType == ItemBaseProfile.WeaponType.bow)
                {
                    TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(rangedTutorial);
                }
                else
                {
                    TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(meleeTutorial);
                }
            }
            else if (EquippingManager.instance.rightWeaponES.currEquippedItem != null)
            {
                if (EquippingManager.instance.rightWeaponES.currEquippedItem.weaponType == ItemBaseProfile.WeaponType.bow)
                {
                    TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(rangedTutorial);
                }
                else
                {
                    TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(meleeTutorial);
                }
            }
        }
    }

    public void DisplayTavernKeeperUI()
    {
        TavernKeeper.instance.DisplayTavernKeeperUI();
    }

    public void FreezeCameraAndSetMouseVisibility(ThirdPersonController tPC, StarterAssetsInputs _input, bool isVisible)
    {
        _input.cursorInputForLook = isVisible;
        _input.cursorLocked = isVisible;

        tPC.LockCameraPosition = !isVisible;

        Cursor.visible = !isVisible;

        _input.SetCursorState(_input.cursorLocked);
    }

    public void CreateSaveGameSlotButton()
    {
        for (int i = 0; i < saveGameSlotParentObj.transform.childCount; i++)
        {
            Destroy(saveGameSlotParentObj.transform.GetChild(i).gameObject);
        }

        if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

            for (int i = dirInfo.Length - 1; i > -1; i--)
            {
                var gameDataFolder = Directory.GetFiles(dirInfo[i]);

                StreamReader sr = new StreamReader(gameDataFolder[0]);

                string JsonString = sr.ReadToEnd();

                sr.Close();

                SaveGameObject sOG = JsonUtility.FromJson<SaveGameObject>(JsonString);

                var newSGSlot = Instantiate(saveGameSlotPrefab, saveGameSlotParentObj.transform);

                if (sOG.currentMainMissionName != "")
                {
                    newSGSlot.GetComponent<LoadSlot>().loadGameNameTxt.text = "<b>" + sOG.currentMainMissionName + "</b>, " + sOG.dayOfSaving.ToString();
                }
                else
                {
                    newSGSlot.GetComponent<LoadSlot>().loadGameNameTxt.text = sOG.dayOfSaving.ToString();
                }

                newSGSlot.GetComponent<LoadSlot>().saveGameScreenshot = LoadNewSprite(gameDataFolder[1]);

                newSGSlot.GetComponent<LoadSlot>().correspondingSaveDataDirectory = dirInfo[i];
                newSGSlot.GetComponent<LoadSlot>().correspondingTextFile = gameDataFolder[0];

                newSGSlot.GetComponent<LoadSlot>().loadGameSavingTypeTxt.text = sOG.savingType;
            }
        }
    }

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Sprite newSprite;
        Texture2D SpriteTexture = LoadTexture(FilePath);
        newSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return newSprite;
    }

    public Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        FileData = File.ReadAllBytes(FilePath);
        Tex2D = new Texture2D(2, 2);

        if (Tex2D.LoadImage(FileData))
        {
            return Tex2D;
        }

        return null;
    }

    //public void OnLevelWasLoaded(int level)
    //{
    //    allVillageNPCs.Clear();
    //    allNPCScreamingHandler.Clear();
    //    allMerchants.Clear();
    //    allWalkingNPCs.Clear();
    //    allMeleeEnemies.Clear();
    //    allInteractableObjects.Clear();
    //    allInteractableDoors.Clear();
    //}
}
