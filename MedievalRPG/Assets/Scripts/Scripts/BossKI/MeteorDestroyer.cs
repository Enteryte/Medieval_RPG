using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDestroyer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Destructor());
    }

    IEnumerator Destructor()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
