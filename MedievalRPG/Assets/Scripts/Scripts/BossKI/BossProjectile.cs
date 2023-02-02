using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [HideInInspector] public Transform player;
    
    public float speed = 5;

    [SerializeField] private float damage = 5;
    [SerializeField] private Rigidbody rig;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerValueManager.instance.CurrHP -= damage;
        }

        if(!other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("BossHitbox"))
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (player != null)
        {
            rig.isKinematic = true;
            transform.LookAt(player);
            transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
        }
    }
}
