using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StartScreenMainButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image boarder;

    public GameObject screenToOpen;
    public AnimationClip openScreenAnim;

    public bool canPressButtonMoreThanOnce = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.gameObject.GetComponent<Button>().interactable)
        {
            boarder.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.gameObject.GetComponent<Button>().interactable)
        {
            boarder.gameObject.SetActive(false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!canPressButtonMoreThanOnce)
        {
            if (StartScreenManager.currSelectedSSMBtn != null && StartScreenManager.currSelectedLoadSlotBtn != this)
            {
                StartScreenManager.currSelectedSSMBtn.GetComponent<TextMeshProUGUI>().color = Color.white;

                if (StartScreenManager.currSelectedSSMBtn.screenToOpen != null)
                {
                    StartScreenManager.currSelectedSSMBtn.screenToOpen.SetActive(false);

                    StartScreenManager.instance.mainAnimator.enabled = false;
                }
            }

            if (screenToOpen != null)
            {
                screenToOpen.SetActive(true);

                StartScreenManager.instance.mainAnimator.enabled = true;
                StartScreenManager.instance.mainAnimator.Rebind();
                StartScreenManager.instance.mainAnimator.Play(openScreenAnim.name);
            }

            StartScreenManager.currSelectedSSMBtn = this;
            this.gameObject.GetComponent<TMP_Text>().color = Color.gray;
        }       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canPressButtonMoreThanOnce)
        {
            if (StartScreenManager.currSelectedSSMBtn != null && StartScreenManager.currSelectedLoadSlotBtn != this)
            {
                StartScreenManager.currSelectedSSMBtn.GetComponent<TextMeshProUGUI>().color = Color.white;

                if (StartScreenManager.currSelectedSSMBtn.screenToOpen != null)
                {
                    StartScreenManager.currSelectedSSMBtn.screenToOpen.SetActive(false);

                    StartScreenManager.instance.mainAnimator.enabled = false;
                }
            }

            if (screenToOpen != null)
            {
                screenToOpen.SetActive(true);

                StartScreenManager.instance.mainAnimator.enabled = true;
                StartScreenManager.instance.mainAnimator.Rebind();
                StartScreenManager.instance.mainAnimator.Play(openScreenAnim.name);
            }

            StartScreenManager.currSelectedSSMBtn = this;
            this.gameObject.GetComponent<TMP_Text>().color = Color.gray;
        }
    }
}
