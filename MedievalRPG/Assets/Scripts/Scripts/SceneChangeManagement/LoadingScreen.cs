using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public TMP_Text placeNameTxt;
    public Image backgroundImg;
    public TMP_Text descriptionTxt;

    [Header("LoadingScreen-Profile")]
    public static LoadingScreenProfile currLSP;
    public static Vector3 currSpawnPos;
    public static Quaternion currSpawnRot;

    [Header("Values To Set After Change")]
    public GameObject pauseMenuScreen;
    public GameObject startScreenMainUI;
    public GameObject startScreenMainUIButtonParent;

    public bool playerWasDead = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Start()
    {
   
    }

    public void ChangeToNewScene()
    {
        SceneManager.LoadScene(currLSP.sceneToLoadIndex);
    }

    public void ActivateAnimator()
    {
        StartScreenManager.instance.mainObjectAnimator.enabled = true;

        //        this.gameObject.SetActive(false);
        //StartScreenManager.instance.mainObjectAnimator.enabled = false;
        //SceneChangeManager.instance.loadingScreen.SetActive(false);

        //LoadingScreen.instance.startScreenMainUI.SetActive(false);
    }

    public void DeactivateAnimator()
    {
        this.gameObject.SetActive(false);
        StartScreenManager.instance.mainObjectAnimator.enabled = false;
        SceneChangeManager.instance.loadingScreen.SetActive(false);

        LoadingScreen.instance.startScreenMainUI.SetActive(false);

        //GameManager.instance.ContinueGame();
        Debug.Log("GZJHNKMJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJ");
        //StartScreenManager.instance.mainObjectAnimator.Rebind();

        //if (GameManager.instance != null)
        //{
        //pauseMenuScreen.SetActive(true);
        //    GameManager.instance.pauseMenuScreen = startScreenMainUI;

        //}
    }
}
