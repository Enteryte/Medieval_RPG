using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoint : MonoBehaviour
{
    public GameObject correspondingNPCObj;

    public GameObject nextWaypoint;

    public bool stayForSpecificTime = false;
    public float stayingTime;

    public IEnumerator StartStayingForSpecificTime()
    {
        float currStayingTime = 0;

        correspondingNPCObj.GetComponent<NPC>().navMeshAgent.isStopped = true;

        correspondingNPCObj.GetComponent<NPC>().animator.SetBool("IsStanding", true);

        while (currStayingTime < stayingTime)
        {
            currStayingTime += Time.deltaTime;

            yield return null;
        }

        correspondingNPCObj.GetComponent<NPC>().navMeshAgent.isStopped = false;

        correspondingNPCObj.GetComponent<NPC>().animator.SetBool("IsStanding", false);

        correspondingNPCObj.GetComponent<NPC>().SetNewWaypoint(nextWaypoint.GetComponent<NPCWaypoint>());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == correspondingNPCObj && correspondingNPCObj.GetComponent<NPC>().currWaypoint == this)
        {
            Debug.Log("HNJKMS");

            if (stayForSpecificTime)
            {
                StartCoroutine(StartStayingForSpecificTime());
            }
            else
            {
                correspondingNPCObj.GetComponent<NPC>().SetNewWaypoint(nextWaypoint.GetComponent<NPCWaypoint>());
            }
        }
    }
}
