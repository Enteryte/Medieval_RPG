using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameObject
{
    // Save-Game-Data
    public float playtimeInSeconds;
    public string dayOfSaving;

    // Player
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public float currPlayerMoney;
    public float currPlayerHealth;
    public float currPlayerStamina;

    // NPCs
    public List<Vector3> allNPCPositions = new List<Vector3>();
    public List<Quaternion> allNPCRotations = new List<Quaternion>();
    public List<bool> isNPCVisible = new List<bool>();

    // --------------------------------------------- WIP: Noch schauen, ob man das überhaupt benötigt.
    //// Merchants
    //public List<Vector3> allMerchantPositions = new List<Vector3>();
    //public List<Quaternion> allMerchantRotations = new List<Quaternion>();
    //public List<bool> isMerchantVisible = new List<bool>();

    //// TavernKeeper
    //public Vector3 tavernKeeperPosition;
    //public Quaternion tavernKeeperRotation;

    // Missions
    public List<int> allCurrAcceptedMissionNumbers = new List<int>();
    public List<int> allCurrOpenNotAcceptedMissionNumbers = new List<int>();

    public List<bool> allAcceptedMissionTaskIsCompletedStates = new List<bool>();
    public List<bool> allAcceptedMissionTaskIsDisplayableStates = new List<bool>();

    public string currentMainMissionName;

    // Inventory
    //public float currHoldingWeight;
    public List<int> itemID = new List<int>();
    public List<int> itemAmountInInventory = new List<int>();

    // Equipment
    public int currLeftHandWeaponID;
    public int currRightHandWeaponID;

    // Hotbar
    public List<int> storedItemID = new List<int>();
    public List<int> storedItemAmount = new List<int>();

    // Interactable Objects ( /+ Doors )
    public List<string> allInteractableObjectNames = new List<string>();

    public List<bool> isDoorOpen = new List<bool>();
}
