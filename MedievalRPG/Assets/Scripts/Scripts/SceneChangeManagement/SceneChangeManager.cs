using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance;

    public GameObject loadingScreen;

    public bool startedNewGame = false;
    public bool pressedContinue = false;
    public CutsceneProfile startCutsceneP;

    [Header("Deactivate If Load GameScene")]
    public List<GameObject> allGOsToDeactivate;
    public List<GameObject> allGosToActivate;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
    }

    public void Start()
    {
        
    }

    public void LoadScene()
    {
        if (startedNewGame)
        {
            Debug.Log("NS");

            SceneManager.LoadScene("Fabienne_TestingScene");
        }
        else
        {
            // Load

            SceneManager.LoadScene("Fabienne_TestingScene");
        }
    }

    public void PlayOpeningCutscene()
    {
        if (startedNewGame)
        {
            //StartScreenManager.instance.mainObjectAnimator.enabled = false;

            CutsceneManager.instance.currCP = startCutsceneP;
            CutsceneManager.instance.playableDirector.playableAsset = startCutsceneP.cutscene;
            CutsceneManager.instance.playableDirector.Play();

            //StartScreenManager.instance.mainObjectAnimator.enabled = false;
            //loadingScreen.SetActive(false);

            //StartScreenManager.instance.mainObjectAnimator.Rebind();

            //StartScreenManager.instance.mainObjectAnimator.Rebind();
            //StartScreenManager.instance.mainObjectAnimator.enabled = true;

            startedNewGame = false;

            DeactivateMainObjectAnimator();
        }
    }

    public void LoadContinuePath()
    {
        if (pressedContinue)
        {
            SaveSystem.instance.LoadContinueData();
        }
        else
        {
            SaveSystem.instance.Load();
        }
        
        GameManager.instance.cutsceneBlackFadeGO.SetActive(false);
    }

    public void DeactivateMainObjectAnimator()
    {
        StartScreenManager.instance.mainObjectAnimator.enabled = false;

        for (int i = 0; i < allGOsToDeactivate.Count; i++)
        {
            allGOsToDeactivate[i].SetActive(false);
        }

        for (int i = 0; i < allGosToActivate.Count; i++)
        {
            allGosToActivate[i].SetActive(true);
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        if (level == 2 && startedNewGame)
        {
            Debug.Log(StartScreenManager.instance.mainObjectAnimator);
            Debug.Log(StartScreenManager.instance.gameObject.name);
            StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim");
        }
        else if (level == 2)
        {
            StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim_2");
        }

        GameManager.instance.pauseMenuScreen = this.gameObject;

        StartScreenManager.instance.mainAnimator.enabled = false;

        Debug.Log("LOADED");
    }
}
