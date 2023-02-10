using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamager : MonoBehaviour
{
    private float Damage;
    private bool IsDamaging;
    private Animator anim;
    private NavMeshAgent Agent;

    private void OnCollisionEnter(Collision _collision)
    {
        Debug.Log($"Hit Gameobject: {_collision.gameObject.name}");
        if (!GameManager.instance)
            return;
        if(_collision.gameObject.CompareTag("Shield"))
        {
            //Add Recoil Animation and stop the damager here.
            anim.SetTrigger(Animator.StringToHash("Recoil"));
            Agent.isStopped = true;
            return;
        }

        if (!IsDamaging || _collision.gameObject != GameManager.instance.playerGO)
            return;
        Attack(_collision.gameObject);
    }

    public void Init(float _damage)
    {
        Damage = _damage;
        anim = GetComponentInParent<Animator>();
        Agent = anim.gameObject.GetComponentInParent<NavMeshAgent>();
    }

    public void DamageOn()
    {
        IsDamaging = true;
    }

    public void DamageOff()
    {
        IsDamaging = false;
    }

    private void Attack(GameObject _playerGameObject)
    {
        if (_playerGameObject.TryGetComponent(out GotDamage gDmg))
            gDmg.GotHit(true);
        PlayerValueManager.instance.CurrHP -= Damage;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;
        IsDamaging = false;
    }
}