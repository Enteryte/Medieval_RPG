using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoint : MonoBehaviour
{
    public GameObject correspondingNPCObj;

    public GameObject nextWaypoint;

    public bool stayForSpecificTime = false;
    public float stayingTime;
    [Min(0)] public int standingAnimNumber = 0;

    public void Start()
    {
        correspondingNPCObj.GetComponent<NPC>().allCorrWaypoints.Add(this);
    }

    public IEnumerator StartStayingForSpecificTime()
    {
        float currStayingTime = 0;

        correspondingNPCObj.GetComponent<NPC>().navMeshAgent.isStopped = true;

        correspondingNPCObj.GetComponent<NPC>().animator.SetInteger("StandingAnimNumber", standingAnimNumber);

        correspondingNPCObj.GetComponent<NPC>().animator.SetBool("IsStanding", true);

        while (currStayingTime < stayingTime)
        {
            currStayingTime += Time.deltaTime;

            yield return null;
        }

        correspondingNPCObj.GetComponent<NPC>().navMeshAgent.isStopped = false;

        correspondingNPCObj.GetComponent<NPC>().animator.SetBool("IsStanding", false);

        StartCoroutine(correspondingNPCObj.GetComponent<NPC>().SetNewWaypoint(nextWaypoint.GetComponent<NPCWaypoint>()));
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == correspondingNPCObj && correspondingNPCObj.GetComponent<NPC>().currWaypoint == this)
        {
            //if (stayForSpecificTime)
            //{
            //    StartCoroutine(StartStayingForSpecificTime());
            //}
            //else
            //{
                correspondingNPCObj.GetComponent<NPC>().SetNewWaypointWithoutStopping(nextWaypoint.GetComponent<NPCWaypoint>());
            //}
        }
    }
}
