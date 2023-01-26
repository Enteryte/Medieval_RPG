using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchMenuButton : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject missionLogUI;

    public GameObject otherGO;

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryUI != null)
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(OpenInventory);

            if (inventoryUI.activeSelf)
            {
                OpenInventory();
            }
        }
        else
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(OpenMissionLog);
        }
    }

    public void OpenInventory()
    {
        InventoryManager.instance.DisplayItemsOfCategory();

        this.gameObject.GetComponent<TMP_Text>().color = Color.yellow;
        otherGO.GetComponent<TMP_Text>().color = Color.white;

        inventoryUI.SetActive(true);
        otherGO.GetComponent<SwitchMenuButton>().missionLogUI.SetActive(false);
    }

    public void OpenMissionLog()
    {
        MissionLogScreenHandler.instance.DisplayMissions();

        this.gameObject.GetComponent<TMP_Text>().color = Color.yellow;
        otherGO.GetComponent<TMP_Text>().color = Color.white;

        missionLogUI.SetActive(true);
        otherGO.GetComponent<SwitchMenuButton>().inventoryUI.SetActive(false);
    }
}
