using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardMissionButton : MonoBehaviour
{
    public MissionBaseProfile storedMissionBP;

    public void DisplayMissionInformations()
    {
        // WIP: -> Warten auf UI.
    }

    public void AddMissionToAccepted()
    {
        MissionManager.instance.AddMission(storedMissionBP);
    }

    public void SetStoredMission(MissionBaseProfile newMissionToStore)
    {
        storedMissionBP = newMissionToStore;
    }
}
