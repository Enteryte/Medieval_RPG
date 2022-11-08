using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public GameObject destroyedObjPrefab;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        var destroyedObjVersion = Instantiate(destroyedObjPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

        Destroy(this.gameObject);
    }
}
