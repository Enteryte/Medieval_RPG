using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCursor : MonoBehaviour
{
    public static ShopCursor instance;

    public Image cursorImg;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = Input.mousePosition;
    }
}
