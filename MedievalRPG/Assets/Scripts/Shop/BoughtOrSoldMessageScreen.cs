using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoughtOrSoldMessageScreen : MonoBehaviour
{
    public TMP_Text boughtOrSoldTxt;

    public float timeTillDeactivation = 3;
    public float currPassedTime = 0;

    public void Update()
    {
        currPassedTime += Time.deltaTime;

        if (currPassedTime >= timeTillDeactivation)
        {
            currPassedTime = 0;

            this.gameObject.SetActive(false);
            this.enabled = false;
        }
    }
}
