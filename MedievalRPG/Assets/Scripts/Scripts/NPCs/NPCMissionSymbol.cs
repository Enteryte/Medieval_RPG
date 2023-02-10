using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCMissionSymbol : MonoBehaviour
{
    public GameObject exclaImg;
    public GameObject questionImg;

    //public MinimapIcon corrMMI;

    public void Start()
    {
        //exclaImg = this.gameObject.transform.GetChild(1).gameObject;
        //questionImg = this.gameObject.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var rot = Camera.main.transform.rotation;
        //rot.x = 90;

        transform.rotation = rot;

        //exclaImg.SetActive(corrMMI.exclamationMarkImg.activeSelf);
        //questionImg.SetActive(corrMMI.questionMarkImg.activeSelf);
    }
}
