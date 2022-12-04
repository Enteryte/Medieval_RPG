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
                Debug.Log("NEW GAME");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainAnimator.Play(closeAreYouSureNewGameScreenAnim.name);
            }
        }
    }

    public void SetCanPressKeyTrue()
    {
        canPressAnyKey = true;
    }
}
