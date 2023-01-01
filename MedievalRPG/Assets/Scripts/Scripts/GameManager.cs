using Cinemachine;
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

    public bool isNight = false; // NUR ZUM TESTEN F�R DIE CUTSCENES! ( in DNCircle ersetzen )
    public CutsceneProfile correspondingCutsceneProfilAtNight; // NUR ZUM TESTEN F�R DIE CUTSCENES! ( in DNCircle ersetzen )

    public GameObject cutsceneBlackFadeGO;
    public GameObject interactCanvasasParentGO;
    public GameObject mapGO;
    public GameObject hotbarGO;
    public GameObject playerStatsGO;

    public CinemachineVirtualCamera normalPlayerFollowCamCVC;

    public GameObject pauseMenuScreen;
    public bool gameIsPaused = false;

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

    public void Awake()
    {
        instance = this;

        //BeerScreenMissionButton.instance = bSMButton;
    }

    //public void Start()
    //{
    //    CreateSaveGameSlotButton();
    //}

    public void Update()
    {
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
        }

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

        if (Input.GetKeyDown(KeyCode.I) && !ShopManager.instance.shopScreen.activeSelf)
        {
            OpenInventory();

            MissionLogScreenHandler.instance.DisplayMissions();
        }

        if (pauseMenuScreen != null && !TutorialManager.instance.bigTutorialUI.activeSelf/* && TutorialManager.currTBP == null*/)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !readBookOrNoteScreen.activeSelf && !ShopManager.instance.shopScreen.activeSelf/* &&*/ /*!CutsceneManager.instance.playableDirector.playableGraph.IsV*//*alid()*/)
            {
                if (CutsceneManager.instance.currCP != null && !CutsceneManager.instance.cutsceneCam.activeSelf)
                {
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
                else if (CutsceneManager.instance.currCP == null)
                {
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

        // NUR ZUM TESTEN F�R DIE CUTSCENES! ( in DNCircle ersetzen )
        if (isNight && correspondingCutsceneProfilAtNight != null)
        {
            CutsceneManager.instance.currCP = correspondingCutsceneProfilAtNight;
            CutsceneManager.instance.playableDirector.playableAsset = correspondingCutsceneProfilAtNight.cutscene;
            CutsceneManager.instance.playableDirector.Play();

            correspondingCutsceneProfilAtNight = null;
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
            allMeleeEnemies[i].Animator.speed = 0;

            allMeleeEnemies[i].GetComponent<NavMeshAgent>().isStopped = true;
        }

        PrickMinigameManager.instance.prickCardAnimator.speed = 0;

        if (CutsceneManager.instance.playableDirector.playableAsset != null && CutsceneManager.instance.playableDirector.playableGraph.IsValid())
        {
            pausedCutsceneTime = CutsceneManager.instance.playableDirector.time;
            CutsceneManager.instance.playableDirector.Pause();
        }

        if (TutorialManager.instance.smallTutorialUI.activeSelf)
        {
            TutorialManager.instance.animator.speed = 0;
        }

        UIAnimationHandler.instance.addedMissionAnimator.speed = 0;
        UIAnimationHandler.instance.missionDisplayAnimator.speed = 0;

        //StartScreenManager.instance.mainObjectAnimator.enabled = true;

        gameIsPaused = true;
    }

    public void ContinueGame()
    {
        // Player
        playerGO.GetComponent<Animator>().speed = 1;

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
                allNPCScreamingHandler[i].nPCAudioSource.UnPause();
            }
        }

        // Enemies
        for (int i = 0; i < allMeleeEnemies.Count; i++)
        {
            allMeleeEnemies[i].Animator.speed = 1;

            allMeleeEnemies[i].GetComponent<NavMeshAgent>().isStopped = false;
        }

        PrickMinigameManager.instance.prickCardAnimator.speed = 1;

        if (CutsceneManager.instance.playableDirector.playableAsset != null && CutsceneManager.instance.playableDirector.playableGraph.IsValid())
        {
            CutsceneManager.instance.playableDirector.Play();
            CutsceneManager.instance.playableDirector.time = pausedCutsceneTime;
        }

        if (TutorialManager.instance.smallTutorialUI.activeSelf)
        {
            TutorialManager.instance.animator.speed = 1;
        }

        UIAnimationHandler.instance.addedMissionAnimator.speed = 1;
        UIAnimationHandler.instance.missionDisplayAnimator.speed = 1;

        StartScreenManager.instance.mainAnimator.enabled = false;

        gameIsPaused = false;
    }

    public void OpenInventory()
    {
        InventoryManager.instance.inventoryScreen.SetActive(!InventoryManager.instance.inventoryScreen.activeSelf);

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
}
