using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableObjectCanvas : MonoBehaviour
{
    public TMP_Text howToInteractTxt;
    public GameObject iOCanvas;

    public void Awake()
    {
        iOCanvas = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
