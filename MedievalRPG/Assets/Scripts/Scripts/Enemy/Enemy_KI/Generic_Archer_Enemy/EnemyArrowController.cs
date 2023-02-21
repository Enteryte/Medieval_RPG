using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyArrowController : MonoBehaviour
{
    private Transform PlayerTransform;
    private Vector3 Target;
    private Rigidbody Rb;
    private ArrowPool AssignedPoolManager;
    private float DespawnTime;
    private float Damage;
    private float Velocity;

    private readonly float InaccuracyXZ = 0.1f;
    private readonly float InaccuracyY = 0.6f;
    private bool CanDamage = true;

    public void Initialize(ArrowPool _assignedPoolManager, float _despawnTime, float _damage, float _velocity,
        Transform _playerTransform)
    {
        AssignedPoolManager = _assignedPoolManager;
        DespawnTime = _despawnTime;
        Damage = _damage;
        Velocity = _velocity;
        PlayerTransform = _playerTransform;
        Rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision _collision)
    {
        // Debug.Log($"{gameObject.name} hit {_collision.gameObject.name} Collision");
        StartCoroutine(Despawn());
        //Do the Sticking
        transform.SetParent(_collision.transform);
        Rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider _collision)
    {
        Debug.Log($"{gameObject.name} hit {_collision.gameObject.name} Trigger");
        //Do Damage after checking if you hit the player or not
        if (_collision.gameObject.CompareTag("Shield"))
            CanDamage = false;
        if (!CanDamage || !_collision.TryGetComponent(out GotDamage gDmg)) return;
        Debug.Log($"{gameObject.name} caused {Damage} to the Player");
        try
        {
            gDmg.GotHit(true);
            Debug.Log("No GotHit error");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in GotHit method: " + ex.Message);
        }

        PlayerValueManager.instance.CurrHP -= Damage * DifficultyHandler.instance.dmgMultiplier;
        CanDamage = false;
    }


    public void Fire()
    {
        transform.parent = null;
        Target = PlayerTransform.position;
        Target.y += 1.0f;
        Target += new Vector3(Random.Range(-InaccuracyXZ, InaccuracyXZ), Random.Range(-InaccuracyY, InaccuracyY),
            Random.Range(-InaccuracyXZ, InaccuracyXZ));
        Vector3 dir = Target - transform.position;
        transform.LookAt(Target);
        Rb.AddForce(dir * Velocity, ForceMode.Impulse);
    }

// private void FixedUpdate()
// {
//     if (!IsFired || HasCollided)
//         return;
//     MoveArrow(Time);
//     Time += UnityEngine.Time.deltaTime;
//     if (Vector3.Distance(transform.position, Target) < DstToStopBeingPerfect)
//         Target = PlayerTransform.position;
// }
//
// private void MoveArrow(float _time)
// {
//     transform.position = Vector3.Lerp(FiringPoint, Target, _time);
//     
// }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(DespawnTime);
        CanDamage = true;
        AssignedPoolManager.DespawnArrow(this);
        transform.parent = AssignedPoolManager.GetArrowParent;
        Rb.isKinematic = false;
        Target = new Vector3();
    }
}