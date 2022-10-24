using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBetAmountButton : MonoBehaviour
{
    public bool isAddAmountBtn = false;

    // Update is called once per frame
    void Update()
    {
        if (isAddAmountBtn)
        {
            if (GuessTheCardMinigameManager.instance.currBetAmount == PlayerValueManager.instance.money)
            {
                this.gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.gameObject.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            if (GuessTheCardMinigameManager.instance.currBetAmount == 0)
            {
                this.gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }
}
