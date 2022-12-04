using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LoadSlot : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text loadGameNameTxt;
    public TMP_Text loadGameSavingTypeTxt;

    public Animator animator;
    public Image boarderImg;

    public Color32 normalColor;
    public Color32 selectedColor;

    public AnimationClip hoverOverAnim;
    public AnimationClip hoverOverExitAnim;

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

        animator.Rebind();
        animator.Play(hoverOverAnim.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        boarderImg.gameObject.SetActive(false);
        loadGameNameTxt.color = normalColor;
        loadGameSavingTypeTxt.color = normalColor;

        animator.Rebind();
        animator.Play(hoverOverExitAnim.name);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Load Game
    }
}
