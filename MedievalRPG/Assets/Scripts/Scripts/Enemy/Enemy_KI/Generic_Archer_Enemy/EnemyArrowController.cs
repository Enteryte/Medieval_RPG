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
    private bool IsFired;
    private float DstToStopBeingPerfect;
    private float DespawnTime;
    private bool HasCollided;
    private float Damage;
    private float Speed;
    float Time = 0f;

    public void Initialize(ArrowPool _assignedPoolManager, float _dstToStopBeingPerfect, float _despawnTime,
        float _damage, float _speed, Transform _playerTransform)
    {
        AssignedPoolManager = _assignedPoolManager;
        DstToStopBeingPerfect = _dstToStopBeingPerfect;
        DespawnTime = _despawnTime;
        Damage = _damage;
        Speed = _speed;
        PlayerTransform = _playerTransform;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        HasCollided = true;
        IsFired = false;

        StartCoroutine(Despawn());
        //Do the Sticking after checking if you hit the player or not
        if (!_collision.gameObject.CompareTag("Player"))
        {
            transform.SetParent(_collision.transform);
        }
        //Do Something the arrow sticks or something, parent it to the player until despawn.
    }

    private void OnTriggerEnter(Collider _collision)
    {
        //Do Damage after checking if you hit the player or not
        if (!_collision.gameObject.CompareTag("Player"))
            return;
        // _collision.gameObject.GetComponent(Insert PlayerhealthDamageScript Here)
        //Insert PlayerhealthDamageScript Here.DoDamage(Damage);
    }

    public void Fire()
    {
        FiringPoint = transform.position;
        // StartCoroutine(FireProcess());
        Time = 0f;
        Target = PlayerTransform.position;
        IsFired = true;
    }

    private void FixedUpdate()
    {
        if (!IsFired || HasCollided)
            return;
        MoveArrow(Time);
        Time += UnityEngine.Time.deltaTime;
        // if (Vector3.Distance(transform.position, Target) < DstToStopBeingPerfect)
        //     Target = PlayerTransform.position;
    }

    private void MoveArrow(float _time)
    {
        transform.position = Vector3.Slerp(FiringPoint, Target, _time);
        transform.rotation = Quaternion.LookRotation(Vector3.Slerp(FiringPoint, Target, _time + 0.1f));
    }

    private IEnumerator Despawn()
    {
        Debug.Log(gameObject.name + "Starts Despawning");
        yield return new WaitForSeconds(DespawnTime);
        AssignedPoolManager.DespawnArrow(this);
        transform.parent = null;
        Target = new Vector3();
        FiringPoint = new Vector3();
    }
}