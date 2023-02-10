using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ChooseDifficultyButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image boarder;

    public string textToDisplay;

    public GameObject screenToOpen;
    public AnimationClip openScreenAnim;

    public TMP_Text difficultyDescriptionTxt;

    public void OnPointerClick(PointerEventData eventData)
    {
        screenToOpen.SetActive(true);

        StartScreenManager.instance.mainAnimator.enabled = true;
        StartScreenManager.instance.mainAnimator.Rebind();
        StartScreenManager.instance.mainAnimator.Play(openScreenAnim.name);

        //SceneChangeManager.instance.startedNewGame = true;
        //SceneChangeManager.instance.LoadVillage();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        boarder.gameObject.SetActive(true);

        difficultyDescriptionTxt.text = textToDisplay;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        boarder.gameObject.SetActive(false);

        difficultyDescriptionTxt.text = "";
    }
}
