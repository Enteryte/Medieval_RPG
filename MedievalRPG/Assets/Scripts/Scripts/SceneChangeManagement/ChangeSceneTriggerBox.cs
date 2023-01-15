using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTriggerBox : MonoBehaviour
{
    public LoadingScreenProfile[] possibleLSProfiles;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO)
        {
            Debug.Log("HIER");

            var randomLSPNumber = Random.Range(0, possibleLSProfiles.Length);
            var randomLSP = possibleLSProfiles[randomLSPNumber];

            LoadingScreen.instance.placeNameTxt.text = randomLSP.placeName;
            LoadingScreen.instance.backgroundImg.sprite = randomLSP.backgroundSprite;
            LoadingScreen.instance.descriptionTxt.text = randomLSP.descriptionTextString;
        }
    }
}
