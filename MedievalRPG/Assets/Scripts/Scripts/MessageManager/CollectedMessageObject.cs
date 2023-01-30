using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollectedMessageObject : MonoBehaviour
{
    public TMP_Text itemNameTxt;
    public Image itemSpriteImg;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyMessageObject());
    }

    public IEnumerator DestroyMessageObject()
    {
        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);
    }
}
