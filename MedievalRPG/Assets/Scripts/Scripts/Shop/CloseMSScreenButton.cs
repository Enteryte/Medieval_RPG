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

    public static void CloseScreen()
    {
        FightManager.instance.UpdateArrowHUDDisplay();

        //ShopManager.instance.CheckIfNeededForCutscene();

        ShopManager.instance.shopScreen.SetActive(false);

        //CutsceneManager.instance.cutsceneCam.SetActive(false);

        //GameManager.instance.playerGO.transform.parent = CutsceneManager.instance.playerBaseMeshParentTrans;

        GameManager.instance.playerGO.transform.parent = CutsceneManager.instance.playerBaseMeshParentTrans;

        //CutsceneManager.instance.ActivateHUDUI();

        //CutsceneManager.instance.playableDirector.Stop();

        //getBeerScreen.SetActive(false);

        //ThirdPersonController.instance.canMove = true;
        //GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        CutsceneManager.instance.cutsceneCam.SetActive(false);

        CutsceneManager.instance.ActivateHUDUI();

        CutsceneManager.instance.playableDirector.Stop();

        ThirdPersonController.instance.canMove = true;

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        if (ShopManager.instance.currMerchant != null)
        {
            if (ShopManager.instance.currMerchant.whereToSetPlayerTrans.gameObject != null)
            {
                ShopManager.instance.currMerchant.whereToSetPlayerTrans.gameObject.SetActive(false);
            }

            ShopManager.instance.currMerchant.normalMerchantObj.SetActive(true);
        }

        var randomMerchantDialogue = Random.Range(0, ShopManager.instance.currMerchant.mEndBuyingShopPA.Length);

        CutsceneManager.instance.playableDirector.playableAsset = ShopManager.instance.currMerchant.mEndBuyingShopPA[randomMerchantDialogue];
        CutsceneManager.instance.playableDirector.Play();

        ShopManager.instance.currMerchant = null;

        GameManager.instance.ContinueGame();
        GameManager.instance.cantPauseRN = false;
    }
}
