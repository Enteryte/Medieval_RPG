using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameObject
{
    public string savingType;

    // Save-Game-Data
    public float playtimeInSeconds;
    public string dayOfSaving;

    // Tutorial
    public bool displayTutorial;
    public List<int> allCompletedTutorialNumbers = new List<int>();

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

    public List<string> currWaypointNames = new List<string>();

    // Enemies
    public List<Vector3> allEnemyPositions = new List<Vector3>();
    public List<Quaternion> allEnemyRotations = new List<Quaternion>();

    // --------------------------------------------- WIP: Noch schauen, ob man das �berhaupt ben�tigt.
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

    public List<int> allCurrKillMissionKills = new List<int>();
    public List<int> allCurrCollectMissionProgresses = new List<int>();
    public List<int> allCurrExamineMissionProgresses = new List<int>();

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

    // Options
    // - Audio
    public float masterSlValue;
    public float environmentSlValue;
    public float voiceSlValue;
    public float musicSlValue;
    public float sfxSlValue;

    // - Video
    public bool isWindowed;
    public int resolutionDDValue;
    public bool displaySubtitle;

    // - Controls
    public float camSensiSlValue;
    public float mouseSensiSlValue;

    // -------------------------------> WIP: Save Keys ( Walk, Run, Jump, ( ... ) )
}
