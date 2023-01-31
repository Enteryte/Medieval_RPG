using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [HideInInspector] public Transform target;

    [SerializeField] private float secondsToWait = 2;

    private void Start()
    {
        StartCoroutine(DeleteThis());
    }

    private void Update()
    {
        transform.LookAt(target);
    }

    IEnumerator DeleteThis()
    {
        yield return new WaitForSeconds(secondsToWait);
        Destroy(this.gameObject);
    }
}
