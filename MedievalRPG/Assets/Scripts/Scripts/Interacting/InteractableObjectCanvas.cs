using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class InteractableObjectCanvas : MonoBehaviour
{ 
    public GameObject iOCanvas;

    public GameObject iOBillboardParentObj;
    //public GameObject iOTextParentObj;

    //public TMP_Text howToInteractTxt;
    //public Image keyToPressFillImg;

    public GameObject correspondingGO;

    [HideInInspector] private GameObject doorChildGO;
    [HideInInspector] private GameObject iOCanvasLookAtObj;
    [HideInInspector] private GameObject iOCanvasLookAtSitObj;

    private bool isADoor = false;
    private bool isANPC = false;
    private bool isASeatingObj = false;

    //public static int seatPlaceNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        iOCanvas = this.gameObject;

        if (correspondingGO.GetComponent<Door>() != null)
        {
            isADoor = true;

            doorChildGO = correspondingGO.transform.GetChild(0).gameObject;
        }
        else if (correspondingGO.GetComponent<NPC>() != null || correspondingGO.GetComponent<TavernKeeper>() != null)
        {
            isANPC = true;

            if (correspondingGO.GetComponent<NPC>() != null)
            {
                iOCanvasLookAtObj = correspondingGO.GetComponent<NPC>().iOCanvasLookAtObj;
            }
            else
            {
                iOCanvasLookAtObj = correspondingGO.GetComponent<TavernKeeper>().iOCanvasLookAtObj;
            }
        }
        else if (correspondingGO.GetComponent<Merchant>() != null)
        {
            isANPC = true;

            iOCanvasLookAtObj = correspondingGO.GetComponent<Merchant>().iOCanvasLookAtObj;
        }
        else if (correspondingGO.GetComponent<SeatingObject>() != null)
        {
            isASeatingObj = true;

            iOCanvasLookAtSitObj = correspondingGO.GetComponent<SeatingObject>().iOCanvasLookAtSitPlaceObj;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Camera.main);

        if (correspondingGO == null)
        {
            Destroy(this.gameObject);
        }

        if (!isADoor && !isANPC && !isASeatingObj)
        {
            this.gameObject.transform.position = Camera.main.WorldToScreenPoint(correspondingGO.transform.position);
        }
        else if(!isADoor && isANPC && !isASeatingObj)
        {
            this.gameObject.transform.position = Camera.main.WorldToScreenPoint(iOCanvasLookAtObj.transform.position);
        }
        else if (isASeatingObj&& !isADoor && !isANPC)
        {
            this.gameObject.transform.position = Camera.main.WorldToScreenPoint(iOCanvasLookAtSitObj.transform.position);
        }
        else
        {
            this.gameObject.transform.position = Camera.main.WorldToScreenPoint(doorChildGO.transform.position);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Destroy(this.gameObject);
    }
}
