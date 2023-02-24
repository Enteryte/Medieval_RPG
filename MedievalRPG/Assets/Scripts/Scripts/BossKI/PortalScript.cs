using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [HideInInspector] public Transform target;

    [SerializeField] private float secondsToWait = 2;
    [SerializeField] private AudioSource source;

    private void Start()
    {
        source.volume = AudioManager.Instance.EffectsVolume;
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
