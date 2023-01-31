using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSwipeMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float secondsToWait;

    private float seconds = 0;

    private void MoveIt()
    {
        transform.Translate(new Vector3(0, 0, 1) * speed * Time.deltaTime, Space.World);
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
