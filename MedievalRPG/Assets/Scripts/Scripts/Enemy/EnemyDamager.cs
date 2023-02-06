using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamager : MonoBehaviour
{
    private float Damage;
    private bool IsDamaging;

    private void OnCollisionEnter(Collision _collision)
    {
        Debug.Log($"Hit Gameobject: {_collision.gameObject.name}");
        if (!GameManager.instance)
        {
            
            return;
        }
        if (!IsDamaging || _collision.gameObject == GameManager.instance.playerGO)
            return;
        Attack(_collision.gameObject);
    }
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
        IsDamaging = false;
    }

    private void Attack(GameObject _playerGameObject)
    {
        _playerGameObject.TryGetComponent<GotDamage>(out GotDamage gdmg);
        if (!gdmg)
            throw new Exception("Player Got Damage not at Right place!");
        gdmg.GotHit(true);
        PlayerValueManager.instance.CurrHP -= Damage;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;
        IsDamaging = false;
    }
}