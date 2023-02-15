using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteractable
{
    public EnemyBaseProfile eBP;

    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    [Header("Dead")]
    public bool isDead = false;
    public bool despawnAfterTime = true;

    public float despawnTime = 15;

    [Header("Loot")]
    public List<ItemBaseProfile> lootItems;
    public int lootMoney;

    [Header("If Has To Examine Enemy")]
    public CutsceneProfile cutsceneToPlayAfterExamine;

    public void Start()
    {
        if (despawnAfterTime)
        {
            SetLoot();
        }
    }

    public void Update()
    {
        // NUR ZUM TESTEN FÜR DIE CUTSCENES!

        if (Input.GetKeyDown(KeyCode.U))
        {
            EnemyDie();
        }
    }

    public void SetLoot()
    {
        var howManyLootItems = Random.Range(0, eBP.possibleLootItems.Length);

        for (int i = 0; i < howManyLootItems; i++)
        {
            lootItems.Add(eBP.possibleLootItems[Random.Range(0, eBP.possibleLootItems.Length)]);
        }

        lootMoney = Random.Range(eBP.minLootMoney, eBP.maxLootMoney);
    }

    public void EnemyDie()
    {
        isDead = true;

        CheckIfNeededForMission();

        InstantiateIOCanvas();

        this.gameObject.layer = LayerMask.NameToLayer("Interactable");

        if (despawnAfterTime)
        {
            StartCoroutine(WaitTillDespawn());
        }
    }

    public IEnumerator WaitTillDespawn()
    {
        float passedTime = 0;

        while (passedTime < despawnTime)
        {
            passedTime += Time.deltaTime;

            yield return null;
        }

        if (iOCanvas != null)
        {
            Destroy(iOCanvas.gameObject);
        }

        if (this.gameObject.transform.parent.GetComponent<MeleeEnemyKi>() != null && GameManager.instance.allMeleeEnemies.Contains(this.gameObject.transform.parent.GetComponent<MeleeEnemyKi>()))
        {
            GameManager.instance.allMeleeEnemies.Remove(this.gameObject.transform.parent.GetComponent<MeleeEnemyKi>());
        }
        else if (this.gameObject.transform.parent.GetComponent<ArcherEnemyKI>() != null && GameManager.instance.allArcherEnemies.Contains(this.gameObject.transform.parent.GetComponent<ArcherEnemyKI>()))
        {
            GameManager.instance.allArcherEnemies.Remove(this.gameObject.transform.parent.GetComponent<ArcherEnemyKI>());
        }

        Destroy(this.gameObject.transform.parent.gameObject);
    }

    public void InstantiateIOCanvas()
    {
        GameObject newIOCanvas = Instantiate(Interacting.instance.interactCanvasPrefab, Interacting.instance.iOCSParentObj.transform);

        newIOCanvas.GetComponent<InteractableObjectCanvas>().correspondingGO = this.gameObject;

        iOCanvas = newIOCanvas.GetComponent<InteractableObjectCanvas>();

        newIOCanvas.transform.SetAsFirstSibling();
    }

    public string GetInteractUIText()
    {
        if (despawnAfterTime)
        {
            return "Durchsuchen";
        }
        else
        {
            return "Untersuchen";
        }
    }

    public float GetTimeTillInteract()
    {
        return 0;
    }

    public void Interact(Transform transform)
    {
        GameManager.instance.playerGO.GetComponent<ThirdPersonController>()._animator.SetBool("GrabItem", false);

        for (int i = 0; i < lootItems.Count; i++)
        {
            InventoryManager.instance.inventory.AddItem(lootItems[i], 1);

            MessageManager.instance.CreateCollectedMessage(lootItems[i]);
        }

        PlayerValueManager.instance.money += lootMoney;

        //CheckIfNeededForMission();

        Interacting.instance.rightHandParentRig.weight = 0;
        Interacting.instance.headRig.weight = 0;

        ThirdPersonController.instance._animator.SetLayerWeight(1, 0);

        if (!despawnAfterTime)
        {
            CutsceneManager.instance.currCP = cutsceneToPlayAfterExamine;
            CutsceneManager.instance.playableDirector.playableAsset = cutsceneToPlayAfterExamine.cutscene;
            CutsceneManager.instance.playableDirector.Play();

            StartCoroutine(WaitTillDespawn());
        }

        Destroy(iOCanvas.gameObject);

        Interacting.instance.howToInteractGO.SetActive(false);

        //Destroy(iOCanvas.gameObject);
        //Destroy(this.gameObject);
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }    

    public void CheckIfNeededForMission()
    {
        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
            {
                for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.kill)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.enemyToKillBase == eBP)
                        {
                            MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.howManyAlreadyKilled += 1;

                            MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                        }
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 0)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.kill)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.enemyToKillBase == eBP)
                        {
                            MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.howManyAlreadyKilled += 1;

                            MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                        }
                    }
                }
            }
        }
    }
}
