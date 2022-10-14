using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractableObjectCanvas : MonoBehaviour
{ 
    public GameObject iOCanvas;

    public GameObject iOBillboardParentObj;
    //public GameObject iOTextParentObj;

    //public TMP_Text howToInteractTxt;
    //public Image keyToPressFillImg;

    public GameObject correspondingGO;

    public void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        iOCanvas = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = Camera.main.WorldToScreenPoint(correspondingGO.transform.position);
    }
}
