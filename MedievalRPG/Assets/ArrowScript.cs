using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private bool destroy = true;

    private void Start()
    {
        StartCoroutine(TriggerEvents());
    }

    IEnumerator TriggerEvents()
    {
        yield return new WaitForSeconds(0.05f);
        this.gameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        StopCoroutine(TriggerEvents());
        Destroy(this.GetComponent<Rigidbody>());
        this.transform.SetParent(other.gameObject.transform);
    }
}
