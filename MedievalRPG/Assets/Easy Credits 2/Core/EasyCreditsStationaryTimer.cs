using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onerat.EasyCredits
{
    public class EasyCreditsStationaryTimer : MonoBehaviour
    {
        public float showTime;
        public float destoryTime;
        public GameObject element;

        public IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(showTime);
            if(element)
            element.SetActive(true);
            yield return new WaitForSeconds(destoryTime - showTime);
            element.SetActive(false);
        }
    }
}
