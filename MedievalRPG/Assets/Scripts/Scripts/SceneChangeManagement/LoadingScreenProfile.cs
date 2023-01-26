using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/LoadingScreen/LoadingScreen Profile" , fileName = "New LoadingScreen-Profile")]
public class LoadingScreenProfile : ScriptableObject
{
    public string placeName; // Place name: Where you go to; z. B. "Dorf", "Dungeon"

    public Sprite backgroundSprite;

    public string descriptionTextString;

    public int sceneToLoadIndex;
}
