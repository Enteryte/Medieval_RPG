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

    [Header("Ingame-Scene-Change")]
    public AnimationClip openOtherGameSceneOpenLSAnim;

    public bool wentThroughTrigger = false;

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
            SceneManager.LoadScene(1);
        }
        else if (wentThroughTrigger)
        {
            //wentThroughTrigger = false;

            SceneManager.LoadScene(LoadingScreen.currLSP.sceneToLoadIndex);
        }
        else
        {
            // Load

            SceneManager.LoadScene(1);
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
        else if (!wentThroughTrigger)
        {
            SaveSystem.instance.Load();
        }
        else
        {
            wentThroughTrigger = false;

            RespawnManager.instance.RespawnPlayer(LoadingScreen.currSpawnPos, LoadingScreen.currSpawnRot);
        }
        
        if (GameManager.instance != null && GameManager.instance.cutsceneBlackFadeGO != null)
        {
            GameManager.instance.cutsceneBlackFadeGO.SetActive(false);
        }
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
        if (level == 1 && startedNewGame)
        {
            StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim");
        }
        else if (level != 0)
        {
            StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim_2");
        }

        GameManager.instance.pauseMenuScreen = this.gameObject;

        StartScreenManager.instance.mainAnimator.enabled = false;
    }
}
