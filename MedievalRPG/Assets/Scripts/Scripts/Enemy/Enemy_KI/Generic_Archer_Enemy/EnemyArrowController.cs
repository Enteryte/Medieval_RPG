using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowController : MonoBehaviour
{
    private Transform PlayerTransform;
    private Vector3 Target;
    private Vector3 FiringPoint;

    private ArrowPool AssignedPoolManager;
    private float DstToStopBeingPerfect;
    private float DespawnTime;
    private bool HasCollided;
    private float Damage;
    private float Speed;

    public void Initialize(ArrowPool _assignedPoolManager, float _dstToStopBeingPerfect, float _despawnTime,
        float _damage, float _speed)
    {
        AssignedPoolManager = _assignedPoolManager;
        DstToStopBeingPerfect = _dstToStopBeingPerfect;
        DespawnTime = _despawnTime;
        Damage = _damage;
        Speed = _speed;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        HasCollided = true;
        Debug.Log(Damage);
        //Do the Sticking after checking if you hit the player or not
        if (!_collision.gameObject.CompareTag("Player"))
            return;
        //Do Something the arrow sticks or something, parent it to the player until despawn.
    }

    private void OnTriggerEnter(Collider _collision)
    {
        Debug.Log(Damage);
        //Do Damage after checking if you hit the player or not
        if (!_collision.gameObject.CompareTag("Player"))
            return;
        // _collision.gameObject.GetComponent(Insert PlayerhealthDamageScript Here)
        //Insert PlayerhealthDamageScript Here.DoDamage(Damage);
    }

    public void Fire()
    {
        FiringPoint = transform.position;
        StartCoroutine(FireProcess());
    }

    private IEnumerator FireProcess()
    {
        float time = 0f;
        while (!HasCollided)
        {
            //If working as intended, this should be a convenient 
            if (Vector3.Distance(transform.position, Target) < DstToStopBeingPerfect)
                Target = PlayerTransform.position;
            MoveArrow(time);
            time += (Speed * Time.deltaTime);
        }

        HasCollided = false;
        StartCoroutine(Despawn());
        yield return null;
    }

    private void MoveArrow(float _time)
    {
        transform.position = Vector3.Slerp(FiringPoint, Target, _time);
        transform.rotation = Quaternion.LookRotation(Vector3.Slerp(FiringPoint, Target, _time + Time.deltaTime));
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(DespawnTime);
        AssignedPoolManager.DespawnArrow(this);
        Target = new Vector3();
        FiringPoint = new Vector3();
    }
}