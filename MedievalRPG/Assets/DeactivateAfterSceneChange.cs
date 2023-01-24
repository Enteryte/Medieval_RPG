using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterSceneChange : MonoBehaviour
{
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
    }

    public void OnLevelWasLoaded(int level)
    {

    }
}
