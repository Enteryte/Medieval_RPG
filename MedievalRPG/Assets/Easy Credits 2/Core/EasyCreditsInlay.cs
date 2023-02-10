using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Onerat.EasyCredits
{
    [CreateAssetMenu(fileName = "Easy Credits Inlay", menuName = "Easy Credits/Create Inlay", order = 1)]
    public class EasyCreditsInlay : ScriptableObject
    {

        public Sprite Sprite;
        public GameObject AnimatedObject;
        public AudioClip AudioClip;
    }
}
