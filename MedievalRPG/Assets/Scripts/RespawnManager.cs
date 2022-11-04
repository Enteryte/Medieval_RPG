using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public Transform playerDeathSpawnTrans; // After player died.
    public Transform usedEscapeRopeSpawnTrans; // Pos infront of dungeon entrance.
    public Transform playerGotTooDrunkSpawnTrans; // After player drank too much beer and fainted.

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            RespawnPlayer(playerDeathSpawnTrans);
        }
    }

    public void RespawnPlayer(Transform respawnTrans)
    {
        GameManager.instance.playerGO.transform.position = respawnTrans.position;
        GameManager.instance.playerGO.transform.rotation = respawnTrans.rotation;

        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = false;
        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = true;
    }
}
