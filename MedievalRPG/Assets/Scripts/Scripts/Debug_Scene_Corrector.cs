using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Scene_Corrector : MonoBehaviour
{
    // A simple script to move the scene back to the Starting scene if launched from the village scene
    // This is in order to make testing more convenient
    void Start()
    {
        if (!OptionManager.instance)
        {
            //Figure out what needs to be done for this to work, currently it just breaks.
            SceneManager.LoadScene(0);
        }
    }

}
