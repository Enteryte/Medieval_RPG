using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractableObjectCanvas : MonoBehaviour
{
    public TMP_Text howToInteractTxt;
    public GameObject iOTextParentObj;
    public GameObject iOBillboardParentObj;
    public GameObject iOCanvas;

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
        transform.LookAt(Camera.main.transform.position);
    }
}
