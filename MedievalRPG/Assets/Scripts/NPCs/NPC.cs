using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider), typeof(NavMeshAgent))]
public class NPC : MonoBehaviour, IInteractable
{
    public NPCBaseProfile nPCBP;

    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public Animator animator;

    public List<CutsceneProfile> possibleNormalDialogues;
    public List<MissionBaseProfile> allSideMissionsWithNPC;

    public NPCWaypoint firstWaypoint;
    public NPCWaypoint currWaypoint;

    public NavMeshAgent navMeshAgent;

    public GameObject iOCanvasLookAtObj;

    public bool isInDialogue = false;

    void Start()
    {
        InstantiateIOCanvas();

        if (firstWaypoint != null)
        {
            SetNewWaypointWithoutStopping(firstWaypoint);
        }

        GameManager.instance.allVillageNPCs.Add(this);
    }

    public void Update()
    {
        if (currWaypoint != null)
        {
            navMeshAgent.SetDestination(currWaypoint.transform.position);

            //if (Vector3.Distance(transform.position, currWaypoint.transform.position) <= 1f)
            //{
            //    transform.LookAt(currWaypoint.transform);
            //}
        }
    }

    public IEnumerator SetNewWaypoint(NPCWaypoint newWaypoint)
    {
        //transform.LookAt(newWaypoint.transform);

        animator.SetBool("IsStanding", true);

        while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(newWaypoint.transform.position - transform.position)) > 4f)
        {
            var targetRotation = Quaternion.LookRotation(newWaypoint.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(newWaypoint.transform.position - transform.position)) <= 4f)
            {
                currWaypoint = newWaypoint;
            }

            yield return null;
        }

        animator.SetBool("IsStanding", false);
    }

    public void SetNewWaypointWithoutStopping(NPCWaypoint newWaypoint)
    {
        transform.LookAt(newWaypoint.transform);

        currWaypoint = newWaypoint;
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
        return "Sprechen";
    }

    public float GetTimeTillInteract()
    {
        return 0;
    }

    public void Interact(Transform transform)
    {
        CutsceneManager.instance.currCP = possibleNormalDialogues[Random.Range(0, possibleNormalDialogues.Count)];
        CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
        CutsceneManager.instance.playableDirector.Play();

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;

            animator.SetBool("IsStanding", true);
        }

        transform.LookAt(GameManager.instance.playerGO.transform);

        isInDialogue = true;

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        for (int i = 0; i < MessageManager.instance.collectedMessageParentObj.transform.childCount; i++)
        {
            Destroy(MessageManager.instance.collectedMessageParentObj.transform.GetChild(i).gameObject);
        }

        CheckIfNeededForMission();
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
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.nPCToTalkToBaseProfile == nPCBP)
                        {
                            MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                        }
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.nPCToTalkToBaseProfile == nPCBP)
                    {
                        MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                    }
                }
            }
        }
    }
}
