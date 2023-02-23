using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

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
        //else
        //{
        //    Destroy(this.gameObject);
        //}

        //DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
    }

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene()
    {
        if (StartScreenManager.instance.changeBackToMM)
        {
            StartScreenManager.instance.changeBackToMM = false;

            SceneManager.LoadScene(0);
        }
        else if (startedNewGame)
        {
            SceneManager.LoadScene(1);
        }
        else if (wentThroughTrigger)
        {
            //wentThroughTrigger = false;

            if (PlayerValueManager.instance.CurrHP > 0)
            {
                SceneManager.LoadScene(LoadingScreen.currLSP.sceneToLoadIndex);
            }
            else
            {
                if (SceneManager.GetActiveScene().buildIndex != 1)
                {
                    PlayerValueManager.instance.isDead = true;

                    SceneManager.LoadScene(1);
                }
                else
                {
                    StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim");

                    RespawnManager.instance.RespawnPlayerAfterDeath();

                    Debug.Log(PlayerValueManager.instance.CurrHP + " 222222222222222");

                    PlayerValueManager.instance.isDead = false;
                }
            }
        }
        else
        {
            if (StartScreenManager.currSceneIndex > -1)
            {
                Debug.Log("--------------------------------------" + StartScreenManager.currSceneIndex);

                SceneManager.LoadScene(StartScreenManager.currSceneIndex);

                StartScreenManager.currSceneIndex = -1;
            }
            else
            {
                SceneManager.LoadScene(1);
            }
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

            pressedContinue = false;
        }
        else if (!wentThroughTrigger)
        {
            SaveSystem.instance.Load();
        }
        else
        {
            wentThroughTrigger = false;

            RespawnManager.instance.RespawnPlayer(LoadingScreen.currSpawnPos, LoadingScreen.currSpawnRot);

            SaveSystem.instance.LoadAfterChangedSceneIG();
        }
        
        if (GameManager.instance != null && GameManager.instance.cutsceneBlackFadeGO != null)
        {
            GameManager.instance.cutsceneBlackFadeGO.SetActive(false);
        }
    }

    public void DeactivateMainObjectAnimator()
    {
        //StartScreenManager.instance.mainObjectAnimator.enabled = false;

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            for (int i = 0; i < allGOsToDeactivate.Count; i++)
            {
                allGOsToDeactivate[i].SetActive(false);
            }

            for (int i = 0; i < allGosToActivate.Count; i++)
            {
                allGosToActivate[i].SetActive(true);
            }
        }
        else
        {
            Debug.Log("MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");

            for (int i = 0; i < allGOsToDeactivate.Count; i++)
            {
                allGOsToDeactivate[i].SetActive(true);
            }

            for (int i = 0; i < allGosToActivate.Count; i++)
            {
                allGosToActivate[i].SetActive(false);
            }
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        PlayerValueManager.instance.isDead = LoadingScreen.instance.playerWasDead;

        if (GameManager.instance)
        {
            CutsceneManager.instance.subtitleTxtObj.SetActive(StartScreenManager.instance.showSubtitle);
        }

        if (level == 1 && startedNewGame)
        {
            if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
            {
                //var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

                //for (int i = 0; i < dirInfo.Length; i++)
                //{
                //    if (dirInfo[i].ToString() == StartScreenManager.currClickedLoadSlot.correspondingSaveDataDirectory.ToString())
                //    {
#if UNITY_EDITOR
                        FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/SaveData/");
#endif
                //    }
                //}
            }

            InventoryManager.instance.inventory.slots.Clear();

            for (int i = 0; i < SaveSystem.instance.startItems.Length; i++)
            {
                InventoryManager.instance.inventory.AddItem(SaveSystem.instance.startItems[i], SaveSystem.instance.startItemAmounts[i]);
            }

            StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim");
        }
        else if (level != 0)
        {
            StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim_2");

            GameManager.instance.musicAudioSource.Play();

            Debug.Log(PlayerValueManager.instance.CurrHP + " 222222222222222");

            if (level == 1 && PlayerValueManager.instance.isDead)
            {
                RespawnManager.instance.RespawnPlayerAfterDeath();

                Debug.Log(PlayerValueManager.instance.CurrHP + " 222222222222222");

                PlayerValueManager.instance.isDead = false;
            }
            else/* if (level == 1)*/
            {
                CutsceneManager.instance.currCP = null;
                CutsceneManager.instance.playableDirector.playableAsset = null;
            }

            Debug.Log(PlayerValueManager.instance.CurrHP);
        }
        else
        {
            GameManager.instance.musicAudioSource.Play();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            StartScreenManager.currSelectedSSMBtn = null;
            Debug.Log(StartScreenManager.currSelectedSSMBtn);
            StartScreenManager.instance.mainObjectAnimator.Play("CloseLoadingScreenInStartScreenAnim_3");
        }

        //GameManager.instance.pauseMenuScreen = this.gameObject;

        //if (level != 0)
        //{
        //    GameManager.instance.pauseMenuScreen = pauseMenuScreen;

        //    Debug.Log(pauseMenuScreen);
        //}

        StartScreenManager.instance.mainAnimator.enabled = false;

        StartScreenManager.instance.dontChangeToggle = false;
    }
}
