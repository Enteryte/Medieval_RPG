using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinimapIcon : MonoBehaviour
{
    public Camera minimapCam;

    public TMP_Text mMIconMissionTxt;

    public Merchant corrMerchant;
    public TavernKeeper corrTavernKeeper;
    public Item corrItem;

    //// Start is called before the first frame update
    void Start()
    {
        minimapCam = MinimapManager.instance.minimapCam;

        corrMerchant = this.gameObject.GetComponentInParent<Merchant>();
        corrTavernKeeper = this.gameObject.GetComponentInParent<TavernKeeper>();
        corrItem = this.gameObject.GetComponentInParent<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        var rot = minimapCam.transform.rotation;
        rot.x = 90;

        transform.rotation = (minimapCam.transform.rotation);
    }
}
