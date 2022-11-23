using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMissionButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(StartMissionDialogue);
    }

    public void StartMissionDialogue()
    {
        CutsceneManager.instance.currCP = ShopManager.instance.currMerchant.currCorrTask.dialogToPlayAfterPressedButton;
        CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
        CutsceneManager.instance.playableDirector.Play();
    }
}
