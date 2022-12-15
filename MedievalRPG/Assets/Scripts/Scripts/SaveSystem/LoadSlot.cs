using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LoadSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text loadGameNameTxt;
    public TMP_Text loadGameSavingTypeTxt;

    public Sprite saveGameScreenshot;

    public Animator animator;
    public Image boarderImg;

    public Color32 normalColor;
    public Color32 selectedColor;

    public AnimationClip hoverOverAnim;
    public AnimationClip hoverOverExitAnim;

    [HideInInspector] public string correspondingSaveDataDirectory;
    [HideInInspector] public string correspondingTextFile;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Load Game
        Debug.Log("jhmsdfwfwfwf");
        SaveSystem.instance.Load();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (StartScreenManager.currSelectedLoadSlotBtn != null)
        {
            StartScreenManager.currSelectedLoadSlotBtn.boarderImg.gameObject.SetActive(false);
            StartScreenManager.currSelectedLoadSlotBtn.loadGameNameTxt.color = normalColor;
            StartScreenManager.currSelectedLoadSlotBtn.loadGameSavingTypeTxt.color = normalColor;

            StartScreenManager.currSelectedLoadSlotBtn.animator.Play(hoverOverExitAnim.name);
        }

        boarderImg.gameObject.SetActive(true);
        loadGameNameTxt.color = selectedColor;
        loadGameSavingTypeTxt.color = selectedColor;

        StartScreenManager.currSelectedLoadSlotBtn = this;

        StartScreenManager.instance.saveGameScreenshot.enabled = true;
        StartScreenManager.instance.saveGameScreenshot.sprite = saveGameScreenshot;

        animator.Rebind();
        animator.Play(hoverOverAnim.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        boarderImg.gameObject.SetActive(false);
        loadGameNameTxt.color = normalColor;
        loadGameSavingTypeTxt.color = normalColor;

        StartScreenManager.instance.saveGameScreenshot.enabled = false;
        StartScreenManager.instance.saveGameScreenshot.sprite = null;

        animator.Rebind();
        animator.Play(hoverOverExitAnim.name);
    }

    //public void OnSelect(BaseEventData eventData)
    //{

    //}
}
