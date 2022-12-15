using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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
    public List<GameObject> allInteractableObjects;
    public List<Door> allInteractableDoors;

    //public BeerScreenMissionButton bSMButton;
    public GameObject readBookOrNoteScreen;
    public static ItemBaseProfile currBookOrNote;

    public bool isNight = false; // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )
    public CutsceneProfile correspondingCutsceneProfilAtNight; // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )

    public GameObject pauseMenuScreen;
    public bool gameIsPaused = false;

    [Header("Saving/Loading")]
    public GameObject saveGameSlotPrefab;
    public GameObject saveGameSlotParentObj;

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
        playtimeInSeconds += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.I) && !ShopManager.instance.shopScreen.activeSelf)
        {
            OpenInventory();
        }

        if (pauseMenuScreen != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !readBookOrNoteScreen.activeSelf && !ShopManager.instance.shopScreen.activeSelf && !CutsceneManager.instance.playableDirector.playableGraph.IsValid())
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

        // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )
        if (isNight && correspondingCutsceneProfilAtNight != null)
        {
            CutsceneManager.instance.currCP = correspondingCutsceneProfilAtNight;
            CutsceneManager.instance.playableDirector.playableAsset = correspondingCutsceneProfilAtNight.cutscene;
            CutsceneManager.instance.playableDirector.Play();

            correspondingCutsceneProfilAtNight = null;
        }
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
    }

    public void DisplayGetBeerUI()
    {
        TavernKeeper.instance.OpenGetBeerScreen();
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
