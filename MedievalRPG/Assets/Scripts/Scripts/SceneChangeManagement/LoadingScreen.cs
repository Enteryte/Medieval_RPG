using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public TMP_Text placeNameTxt;
    public Image backgroundImg;
    public TMP_Text descriptionTxt;

    [Header("LoadingScreen-Profile")]
    public static LoadingScreenProfile currLSP;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Start()
    {
   
    }

    public void ChangeToNewScene()
    {
        SceneManager.LoadScene(currLSP.sceneToLoadIndex);
    }
}
