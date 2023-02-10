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
    public AnimationClip closeScreenAnim;

    public bool canPressButtonMoreThanOnce = false;

    //public void Update()
    //{
    //    if (StartScreenManager.currSelectedSSMBtn.closeScreenAnim != null)
    //    {
    //        if (Input.GetKeyDown(KeyCode.N))
    //        {
    //            StartScreenManager.instance.mainObjectAnimator.Rebind();
    //            //StartScreenManager.instance.mainObjectAnimator.enabled = true;
    //            StartScreenManager.instance.mainObjectAnimator.Play(StartScreenManager.currSelectedSSMBtn.closeScreenAnim.name);
    //        }
    //    }
    //}

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
        if (this.gameObject.name == "Text (TMP):ContinueAndStopPausingGame"/* || this.gameObject.name == "Text (TMP):SaveGame"*/)
        {
            return;
        }

        this.gameObject.GetComponent<TMP_Text>().color = Color.gray;

        if (!canPressButtonMoreThanOnce)
        {
            if (StartScreenManager.currSelectedSSMBtn != null && StartScreenManager.currSelectedLoadSlotBtn != this)
            {
                StartScreenManager.currSelectedSSMBtn.GetComponent<TextMeshProUGUI>().color = Color.white;

                //if (StartScreenManager.currSelectedSSMBtn.screenToOpen != null)
                //{
                //    StartScreenManager.currSelectedSSMBtn.screenToOpen.SetActive(false);

                //    StartScreenManager.instance.mainAnimator.enabled = false;
                //}

                if (StartScreenManager.currSelectedSSMBtn.screenToOpen != null && StartScreenManager.currSelectedSSMBtn.closeScreenAnim != null)
                {
                    if (closeScreenAnim == null)
                    {
                        Debug.Log("OOOOOOOOOOOOOOOOO");
                        StartScreenManager.instance.mainObjectAnimator.Rebind();
                        StartScreenManager.instance.mainObjectAnimator.enabled = true;
                        Debug.Log(StartScreenManager.currSelectedSSMBtn);
                        Debug.Log(StartScreenManager.currSelectedSSMBtn.closeScreenAnim.name);
                        Debug.Log(StartScreenManager.instance.mainObjectAnimator);
                        StartScreenManager.instance.mainObjectAnimator.Play(StartScreenManager.currSelectedSSMBtn.closeScreenAnim.name);
                    }
                }
                else if (StartScreenManager.currSelectedSSMBtn.screenToOpen != null && StartScreenManager.currSelectedSSMBtn.closeScreenAnim == null)
                {
                    Debug.Log("OOOOOOOOOOOOOOOOO");
                    StartScreenManager.currSelectedSSMBtn.screenToOpen.SetActive(false);

                    StartScreenManager.instance.mainAnimator.enabled = false;
                }

                if (StartScreenManager.currSelectedSSMBtn.screenToOpen == null || StartScreenManager.currSelectedSSMBtn.closeScreenAnim == null)
                {
                    Debug.Log("OOOOOOOOOOOOOOOOO");
                    StartScreenManager.instance.mainObjectAnimator.Rebind();
                    StartScreenManager.instance.mainObjectAnimator.enabled = false;
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
        }       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.gameObject.name == "Text (TMP):ContinueAndStopPausingGame"/* || this.gameObject.name == "Text (TMP):SaveGame"*/)
        {
            return;
        }

        this.gameObject.GetComponent<TMP_Text>().color = Color.gray;

        if (canPressButtonMoreThanOnce)
        {
            if (StartScreenManager.currSelectedSSMBtn != null && StartScreenManager.currSelectedLoadSlotBtn != this)
            {
                StartScreenManager.currSelectedSSMBtn.GetComponent<TextMeshProUGUI>().color = Color.white;

                if (StartScreenManager.currSelectedSSMBtn.screenToOpen != null && StartScreenManager.currSelectedSSMBtn.closeScreenAnim != null)
                {
                    if (closeScreenAnim == null)
                    {
                        StartScreenManager.instance.mainObjectAnimator.Rebind();
                        StartScreenManager.instance.mainObjectAnimator.enabled = true;
                        StartScreenManager.instance.mainObjectAnimator.Play(StartScreenManager.currSelectedSSMBtn.closeScreenAnim.name);
                    }
                }
                else if (StartScreenManager.currSelectedSSMBtn.screenToOpen != null && StartScreenManager.currSelectedSSMBtn.closeScreenAnim == null)
                {
                    StartScreenManager.currSelectedSSMBtn.screenToOpen.SetActive(false);

                    StartScreenManager.instance.mainAnimator.enabled = false;
                }

                if (StartScreenManager.currSelectedSSMBtn.screenToOpen == null || StartScreenManager.currSelectedSSMBtn.closeScreenAnim == null)
                {
                    StartScreenManager.instance.mainObjectAnimator.Rebind();
                    StartScreenManager.instance.mainObjectAnimator.enabled = false;
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
        }
    }
}
