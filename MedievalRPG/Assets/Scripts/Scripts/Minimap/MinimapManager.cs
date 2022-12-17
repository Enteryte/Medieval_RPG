using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager instance;

    public Camera minimapCam;

    [Header("Merchants")]
    public Sprite normalMerchantSprite;
    public Sprite normalHealerSprite;

    [Header("TavernKeeper")]
    public Sprite normalTavernKeeperSprite;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHasUnacceptedMissionText(MinimapIcon minimapIcon)
    {
        minimapIcon.mMIconMissionTxt.text = "!";
    }

    public void SetHasTaskToCompleteMissionText(MinimapIcon minimapIcon)
    {
        minimapIcon.mMIconMissionTxt.text = "?";
    }
}
