using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float timeOffset;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float colliderTime;
    [SerializeField] private float timeToWaitBeforeGrowing;
    [SerializeField] private AudioSource source;

    private float secondsGone_1;
    private float secondsGone_2;
    private float secondsGone_3;
    private float t = 0;
    private CapsuleCollider coll;

    private void Start()
    {
        coll = GetComponent<CapsuleCollider>();
        speed /= 100;
        source.volume = AudioManager.Instance.EffectsVolume * AudioManager.Instance.MasterVolume;
    }

    private void GrowRadius()
    {
        if(secondsGone_3 >= timeOffset)
        {
            coll.radius = Mathf.Lerp(0, 10.16f, t);
            t += speed * Time.deltaTime;
        }

        secondsGone_3 += Time.deltaTime;
    }

    private void DeactivateCollider()
    {
        secondsGone_1 += Time.deltaTime;

        if (secondsGone_1 >= colliderTime)
        {
            coll.enabled = false;
        }
    }

    private void Destructor()
    {
        secondsGone_2 += Time.deltaTime;

        if (secondsGone_2 >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerValueManager.instance.CurrHP -= damage;
        }
    }

    private void Update()
    {
        GrowRadius();
        DeactivateCollider();
        Destructor();
    }
}
