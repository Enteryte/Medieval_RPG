using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GameObject areYouSureNewGameScreen;
    public AnimationClip closeAreYouSureNewGameScreenAnim;

    public bool dontAskOnDeleteDataAgain = false;
    public static LoadSlot currClickedLoadSlot;
    public GameObject areYouSureDeleteSavaDataScreen;
    public AnimationClip closeAreYouSureDeleteSavaDataAnim;
    public Image dontShowAgainDeleteCheckmark;

    public GameObject areYouSureExitGameScreen;
    public AnimationClip closeAreYouSureExitGameAnim;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                mainObjectAnimator.Play("OpenLoadingScreenInStartScreenAnim");
                Debug.Log("NEW GAME");
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

                Destroy(currClickedLoadSlot.gameObject);
                currClickedLoadSlot = null;

                mainAnimator.Play(closeAreYouSureDeleteSavaDataAnim.name);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainAnimator.Play(closeAreYouSureDeleteSavaDataAnim.name);
            }
        }
        else if (areYouSureExitGameScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainAnimator.Play(closeAreYouSureExitGameAnim.name);
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
        mainObjectAnimator.Rebind();
        mainObjectAnimator.enabled = true;
        mainObjectAnimator.Play("OpenLoadingScreenInStartScreenAnim");
    }
}
