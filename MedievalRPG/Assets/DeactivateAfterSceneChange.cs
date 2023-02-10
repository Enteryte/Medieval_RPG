using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterSceneChange : MonoBehaviour
{
    public bool changeAfterChangedToSIZero = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf && GameManager.instance != null && !GameManager.instance.gameIsPaused)
        {
            this.gameObject.SetActive(false);
        }
        else if (GameManager.instance == null && changeAfterChangedToSIZero)
        {
            this.gameObject.SetActive(false);
            Debug.Log("HJKL;Ö:");
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void OnLevelWasLoaded(int level)
    {

    }
}
