using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTriggerBox : MonoBehaviour
{
    public LoadingScreenProfile[] possibleLSProfiles;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var randomLSPNumber = Random.Range(0, possibleLSProfiles.Length);
            var randomLSP = possibleLSProfiles[randomLSPNumber];

            if (LoadingScreen.instance != null)
            {
                LoadingScreen.currLSP = randomLSP;

                LoadingScreen.instance.placeNameTxt.text = randomLSP.placeName;
                LoadingScreen.instance.backgroundImg.sprite = randomLSP.backgroundSprite;
                LoadingScreen.instance.descriptionTxt.text = randomLSP.descriptionTextString;

                SceneChangeManager.instance.GetComponent<Animator>().enabled = false;
                SceneChangeManager.instance.GetComponent<Animator>().Rebind();
                SceneChangeManager.instance.GetComponent<Animator>().enabled = true;

                SceneChangeManager.instance.loadingScreen.SetActive(true);
                SceneChangeManager.instance.gameObject.GetComponent<Animator>().Play("OpenLoadingScreenInStartScreenAnim");
            }
            else
            {
                SceneManager.LoadScene(randomLSP.sceneToLoadIndex);
            }

            SceneChangeManager.instance.wentThroughTrigger = true;
        }
    }
}
