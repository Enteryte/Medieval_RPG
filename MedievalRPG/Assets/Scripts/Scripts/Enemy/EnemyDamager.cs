using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(HasBeenDamagedAlready || !IsDamaging)
            return;
        if (other.gameObject == GameManager.instance.playerGO)
            Attack();
    }

   

    private float Damage;
    private bool IsDamaging;
    private bool HasBeenDamagedAlready;

    public void Init(float _damage)
    {
        Damage = _damage;
    }

    public void DamageOn()
    {
        IsDamaging = true;
    }
    
    public void DamageOff()
    {
        IsDamaging = true;
    }
    public void Attack()
    {
        //Do Damage to the Player Health here. The Debug is to be replaced with that.
        Debug.Log($"{Damage} launched");
        HasBeenDamagedAlready = true;
    }

    public void ResetAttack()
    {
        HasBeenDamagedAlready = false;
    }
}