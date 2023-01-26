using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    public static StartScreenManager instance;

    public Animator mainObjectAnimator;
    public Animator mainAnimator;
    public AnimationClip afterPressedAnyKeyAnim;

    public bool canPressAnyKey = false;

    public static StartScreenMainButton currSelectedSSMBtn;
    public static LoadSlot currSelectedLoadSlotBtn;
    public static int currSceneIndex = -1;

    public GameObject areYouSureNewGameScreen;
    public AnimationClip closeAreYouSureNewGameScreenAnim;

    public bool dontAskOnDeleteDataAgain = false;
    public static LoadSlot currClickedLoadSlot;
    public GameObject areYouSureDeleteSavaDataScreen;
    public AnimationClip closeAreYouSureDeleteSavaDataAnim;
    public Image dontShowAgainDeleteCheckmark;

    public GameObject areYouSureExitGameScreen;
    public AnimationClip closeAreYouSureExitGameAnim;

    public GameObject areYouSureBackToMMScreen;
    public AnimationClip closeAreYouSureBackToMMAnim;

    public GameObject saveGameSlotPrefab;
    public GameObject saveGameSlotParentObj;
    public Image saveGameScreenshot;

    public Button continueBtn;
    public Button loadSaveDataBtn;

    public bool changeBackToMM = false;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateSaveGameSlotButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && canPressAnyKey)
        {
            mainAnimator.Play("AfterPressedAnyKey");
            canPressAnyKey = false;
        }

        if (areYouSureNewGameScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Start new game
                mainObjectAnimator.Rebind();
                mainObjectAnimator.enabled = true;
                mainAnimator.Play(closeAreYouSureNewGameScreenAnim.name);
                mainObjectAnimator.Play("OpenLoadingScreenInStartScreenAnim");

                SceneChangeManager.instance.startedNewGame = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainAnimator.Play(closeAreYouSureNewGameScreenAnim.name);
            }
        }
        else if (areYouSureDeleteSavaDataScreen.activeSelf)
        {
            if (dontShowAgainDeleteCheckmark.enabled)
            {
                dontAskOnDeleteDataAgain = true;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                // WIP: Delete SavaData

                mainAnimator.Play(closeAreYouSureDeleteSavaDataAnim.name);

                if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
                {
                    var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

                    for (int i = 0; i < dirInfo.Length; i++)
                    {
                        if (dirInfo[i].ToString() == StartScreenManager.currClickedLoadSlot.correspondingSaveDataDirectory.ToString())
                        {
                            FileUtil.DeleteFileOrDirectory(StartScreenManager.currClickedLoadSlot.correspondingSaveDataDirectory.ToString());
                        }
                    }
                }

                Destroy(currClickedLoadSlot.gameObject);
                currClickedLoadSlot = null;

                StartScreenManager.instance.saveGameScreenshot.enabled = false;
                StartScreenManager.instance.saveGameScreenshot.sprite = null;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainAnimator.Play(closeAreYouSureDeleteSavaDataAnim.name);

                StartScreenManager.instance.saveGameScreenshot.enabled = false;
                StartScreenManager.instance.saveGameScreenshot.sprite = null;
            }
        }
        else if (areYouSureExitGameScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SaveSystem.instance.SaveOptions();

                Debug.Log("EXIT GAME");

                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainAnimator.Play(closeAreYouSureExitGameAnim.name);
            }
        }
        else if (areYouSureBackToMMScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                changeBackToMM = true;

                LoadingScreen.instance.gameObject.SetActive(true);
                LoadingScreen.instance.ActivateAnimator();

                SceneChangeManager.instance.loadingScreen.SetActive(true);
                SceneChangeManager.instance.gameObject.GetComponent<Animator>().Play("OpenLoadingScreenInStartScreenAnim");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SaveSystem.instance.SaveOptions();
                currSelectedSSMBtn = null;
                currSelectedLoadSlotBtn = null;

                mainAnimator.Play(closeAreYouSureBackToMMAnim.name);
            }
        }
    }

    public void SetCanPressKeyTrue()
    {
        canPressAnyKey = true;
    }

    public void ContinueGameButton()
    {
        // Continue game
        LoadingScreen.instance.gameObject.SetActive(true);
        LoadingScreen.instance.ActivateAnimator();

        //SceneChangeManager.instance.GetComponent<Animator>().enabled = false;
        mainObjectAnimator.Rebind();
        mainObjectAnimator.enabled = true;
        //SceneChangeManager.instance.GetComponent<Animator>().enabled = true;

        //SaveSystem.instance.SaveAutomatic();

        SceneChangeManager.instance.loadingScreen.SetActive(true);
        SceneChangeManager.instance.pressedContinue = true;

        mainObjectAnimator.Play("OpenLoadingScreenInStartScreenAnim");
    }

    public void CreateSaveGameSlotButton()
    {
        for (int i = 0; i < saveGameSlotParentObj.transform.childCount; i++)
        {
            Destroy(saveGameSlotParentObj.transform.GetChild(i).gameObject);
        }

        if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            var dirInfo = Directory.GetDirectories(Application.persistentDataPath + "/SaveData/");

            for (int i = dirInfo.Length - 1; i > -1; i--)
            {
                if (!dirInfo[i].Contains("_N"))
                {
                    var gameDataFolder = Directory.GetFiles(dirInfo[i]);

                    StreamReader sr = new StreamReader(gameDataFolder[0]);

                    string JsonString = sr.ReadToEnd();

                    sr.Close();

                    SaveGameObject sOG = JsonUtility.FromJson<SaveGameObject>(JsonString);

                    var newSGSlot = Instantiate(saveGameSlotPrefab, saveGameSlotParentObj.transform);

                    if (sOG.currentMainMissionName != "")
                    {
                        newSGSlot.GetComponent<LoadSlot>().loadGameNameTxt.text = "<b>" + sOG.currentMainMissionName + "</b>, " + sOG.dayOfSaving.ToString();
                    }
                    else
                    {
                        newSGSlot.GetComponent<LoadSlot>().loadGameNameTxt.text = sOG.dayOfSaving.ToString();
                    }

                    newSGSlot.GetComponent<LoadSlot>().sceneToLoadIndex = sOG.currSceneIndex;

                    newSGSlot.GetComponent<LoadSlot>().saveGameScreenshot = LoadNewSprite(gameDataFolder[1]);

                    newSGSlot.GetComponent<LoadSlot>().correspondingSaveDataDirectory = dirInfo[i];
                    newSGSlot.GetComponent<LoadSlot>().correspondingTextFile = gameDataFolder[0];

                    if (currSceneIndex <= -1)
                    {
                        currSceneIndex = sOG.currSceneIndex;
                    }
                }
                else
                {
                    if (currSceneIndex <= -1)
                    {
                        FileUtil.DeleteFileOrDirectory(dirInfo[i].ToString());
                    }
                }
            }
        }
    }

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Sprite newSprite;
        Texture2D SpriteTexture = LoadTexture(FilePath);
        newSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return newSprite;
    }

    public Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        FileData = File.ReadAllBytes(FilePath);
        Tex2D = new Texture2D(2, 2);

        if (Tex2D.LoadImage(FileData))
        {
            return Tex2D;
        }

        return null;
    }
}
