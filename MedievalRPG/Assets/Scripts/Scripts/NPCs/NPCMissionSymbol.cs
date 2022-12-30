using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCMissionSymbol : MonoBehaviour
{
    public TMP_Text missionSymbolTxt;

    // Update is called once per frame
    void Update()
    {
        var rot = Camera.main.transform.rotation;
        //rot.x = 90;

        transform.rotation = rot;
    }
}
