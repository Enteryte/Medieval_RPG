using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerGOParent;
    public GameObject playerGO;

    public ItemBaseProfile iBPOfEscapeRope;

    public bool playedTheGameThrough = false; // Soll true sein, sobald der Spieler das Spiel zum ersten Mal durchgespielt hat.

    public List<NPC> allVillageNPCs;

    //public BeerScreenMissionButton bSMButton;
    public GameObject readBookOrNoteScreen;
    public static ItemBaseProfile currBookOrNote;

    public bool isNight = false; // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )
    public CutsceneProfile correspondingCutsceneProfilAtNight; // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )

    public void Awake()
    {
        instance = this;

        //BeerScreenMissionButton.instance = bSMButton;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !ShopManager.instance.shopScreen.activeSelf)
        {
            OpenInventory();
        }

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

        // NUR ZUM TESTEN FÜR DIE CUTSCENES! ( in DNCircle ersetzen )
        if (isNight && correspondingCutsceneProfilAtNight != null)
        {
            CutsceneManager.instance.currCP = correspondingCutsceneProfilAtNight;
            CutsceneManager.instance.playableDirector.playableAsset = correspondingCutsceneProfilAtNight.cutscene;
            CutsceneManager.instance.playableDirector.Play();

            correspondingCutsceneProfilAtNight = null;
        }
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
}
