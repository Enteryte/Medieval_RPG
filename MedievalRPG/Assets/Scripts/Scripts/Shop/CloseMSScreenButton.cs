using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseMSScreenButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(CloseScreen);
    }

    public void CloseScreen()
    {
        ShopManager.instance.CheckIfNeededForCutscene();

        ShopManager.instance.shopScreen.SetActive(false);

        CutsceneManager.instance.cutsceneCam.SetActive(false);

        GameManager.instance.playerGO.transform.parent = CutsceneManager.instance.playerBaseMeshParentTrans;

        if (ShopManager.instance.currMerchant != null)
        {
            if (ShopManager.instance.currMerchant.whereToSetPlayerTrans.gameObject != null)
            {
                ShopManager.instance.currMerchant.whereToSetPlayerTrans.gameObject.SetActive(false);
            }

            ShopManager.instance.currMerchant.normalMerchantObj.SetActive(true);
        }

        ThirdPersonController.instance.canMove = true;

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);
    }
}
