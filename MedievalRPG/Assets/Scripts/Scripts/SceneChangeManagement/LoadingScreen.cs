using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    public TMP_Text placeNameTxt;
    public Image backgroundImg;
    public TMP_Text descriptionTxt;

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
}
