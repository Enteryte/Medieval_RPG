using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSwipeMovement : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float secondsToWait;

    private float seconds = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerValueManager.instance.CurrHP -= damage;
        }
    }

    private void MoveIt()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void Destructor()
    {
        seconds += Time.deltaTime;
        if (seconds >= secondsToWait)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        MoveIt();
        Destructor();
    }
}
