using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StartScreenMainButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image boarder;

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
        if (StartScreenManager.currSelectedSSMBtn != null)
        {
            StartScreenManager.currSelectedSSMBtn.GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        StartScreenManager.currSelectedSSMBtn = this;
        this.gameObject.GetComponent<TMP_Text>().color = Color.gray;
    }
}
