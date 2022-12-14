using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public List<TutorialBaseProfile> allTutorialProfiles;
    public List<TutorialBaseProfile> allCompletedTutorials;

    public GameObject smallTutorialUI;
    public TMP_Text smallTUITNameTxt;
    public TMP_Text smallTUITDescriptionTxt;

    public GameObject bigTutorialUI;
    public TMP_Text bigTUITNameTxt;
    public TMP_Text bigTUITDescriptionTxt;

    public Animator animator;

    public AnimationClip openAndCloseSmallTutorialUIAnim;

    public AnimationClip openBigTutorialUIAnim;
    public AnimationClip closeBigTutorialUIAnim;

    public static TutorialBaseProfile currTBP;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sollte am Ende rausgenommen werden können.
        for (int i = 0; i < allTutorialProfiles.Count; i++)
        {
            allTutorialProfiles[i].tutorialNumber = i;
        }
    }

    public void OpenTutorialUI()
    {
        animator.enabled = false;

        if (currTBP.useSmallTutorialUI)
        {
            if (bigTutorialUI.activeSelf)
            {
                bigTutorialUI.SetActive(false);
            }

            // Open smallUI
            smallTUITNameTxt.text = currTBP.tutorialName;
            smallTUITDescriptionTxt.text = currTBP.tutorialDescription;

            animator.Rebind();
            animator.enabled = true;
            animator.Play(openAndCloseSmallTutorialUIAnim.name);
        }
        else
        {
            if (smallTutorialUI.activeSelf)
            {
                smallTutorialUI.SetActive(false);
            }

            // Open bigUI
            bigTUITNameTxt.text = currTBP.tutorialName;
            bigTUITDescriptionTxt.text = currTBP.tutorialDescription;

            bigTutorialUI.SetActive(true);

            animator.Rebind();
            animator.enabled = true;
            animator.Play(openBigTutorialUIAnim.name);

            GameManager.instance.PauseGame();
        }
    }

    // If pressing enter
    public void CloseBigTutorial()
    {
        // Close bigUI
        animator.Play(closeBigTutorialUIAnim.name);

        if (currTBP.tutorialToTrigger == null)
        {
            GameManager.instance.ContinueGame();

            currTBP = null;
        }
        else if (currTBP.tutorialToTrigger.useSmallTutorialUI)
        {
            GameManager.instance.ContinueGame();
        }
    }

    public void CheckIfTutorialIsAlreadyCompleted(TutorialBaseProfile tutorialBaseToCheck)
    {
        if (!allCompletedTutorials.Contains(tutorialBaseToCheck))
        {
            allCompletedTutorials.Add(tutorialBaseToCheck);

            currTBP = tutorialBaseToCheck;
            OpenTutorialUI();
        }
    }

    #region CutsceneEvents
    public void TriggerWelcomeTutorial()
    {
        allCompletedTutorials.Add(CutsceneManager.instance.currCP.tutorialToTrigger);

        currTBP = CutsceneManager.instance.currCP.tutorialToTrigger;
        OpenTutorialUI();
    }

    public void TriggerNextTutorial()
    {
        if (currTBP != null && currTBP.tutorialToTrigger != null)
        {
            instance.animator.enabled = false;

            instance.allCompletedTutorials.Add(currTBP.tutorialToTrigger);

            currTBP = currTBP.tutorialToTrigger;
            instance.OpenTutorialUI();

            Debug.Log("666688834");
        }
    }
    #endregion
}
