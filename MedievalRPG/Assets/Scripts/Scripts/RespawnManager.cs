using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public Transform playerDeathSpawnTrans; // After player died.
    public Transform usedEscapeRopeSpawnTrans; // Pos infront of dungeon entrance.
    public Transform playerGotTooDrunkSpawnTrans; // After player drank too much beer and fainted.

    public TimelineAsset afterPlayerDeathTimeline;

    public void Awake()
    {
        if (instance == this)
        {
            instance = this;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //RespawnPlayer(playerDeathSpawnTrans);

            RespawnPlayerAfterDeath();
        }
    }

    public void RespawnPlayer(Transform respawnTrans)
    {
        GameManager.instance.playerGO.transform.position = respawnTrans.position;
        GameManager.instance.playerGO.transform.rotation = respawnTrans.rotation;

        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = false;
        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = true;
    }

    public void RespawnPlayer(Vector3 respawnPos, Quaternion respawnRot)
    {
        GameManager.instance.playerGO.transform.position = respawnPos;
        GameManager.instance.playerGO.transform.rotation = respawnRot;

        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = false;
        GameManager.instance.playerGO.GetComponent<ThirdPersonController>().enabled = true;
    }

    public void RespawnPlayerAfterDeath()
    {
        CutsceneManager.instance.playableDirector.playableAsset = afterPlayerDeathTimeline;
        CutsceneManager.instance.playableDirector.Play();

        DebuffManager.instance.StopAllDebuffs();

        if (PlayerValueManager.instance.money > 1000)
        {
            PlayerValueManager.instance.money -= (int)((PlayerValueManager.instance.money * 10) / 100);
        }
        else if (PlayerValueManager.instance.money > 100)
        {
            PlayerValueManager.instance.money -= (int)((PlayerValueManager.instance.money * 20) / 100);
        }
        else if (PlayerValueManager.instance.money <= 10)
        {
            PlayerValueManager.instance.money = 0;
        }
        else if (PlayerValueManager.instance.money > 10 && PlayerValueManager.instance.money < 100)
        {
            PlayerValueManager.instance.money -= (int)(PlayerValueManager.instance.money / 2);
        }

        PlayerValueManager.instance.CurrHP = (int)PlayerValueManager.instance.normalHP / 2;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;

        PlayerValueManager.instance.currStamina = PlayerValueManager.instance.normalStamina;
        PlayerValueManager.instance.staminaSlider.value = PlayerValueManager.instance.currStamina;
    }
}
