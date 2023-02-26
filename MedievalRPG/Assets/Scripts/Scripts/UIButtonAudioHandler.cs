using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudioHandler : MonoBehaviour, IPointerClickHandler, IOnPointerEnterHandler
{
    public AudioClip hoverAC;
    public AudioClip clickAC;

    public void Awake()
    {
        if (this.gameObject.GetComponent<ClickableInventorySlot>() != null && this.gameObject.GetComponent<ClickableInventorySlot>().enabled)
        {
            Destroy(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.instance)
        {
            GameManager.instance.uiAudioSource.PlayOneShot(hoverAC);
        }
        else
        {
            SceneChangeManager.instance.uiAudioSource.PlayOneShot(hoverAC);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.instance)
        {
            GameManager.instance.uiAudioSource.PlayOneShot(clickAC);
        }
        else
        {
            SceneChangeManager.instance.uiAudioSource.PlayOneShot(clickAC);
        }
    }
}
