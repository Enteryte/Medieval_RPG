using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private float Damage;
    private bool IsDamaging = true;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject == GameManager.instance.playerGO || IsDamaging)
            HitPlayer(_other.gameObject);
    }

    private void HitPlayer(GameObject _playerGO)
    {
        _playerGO.GetComponent<GotDamage>().GotHit(true);
        PlayerValueManager.instance.CurrHP -= Damage;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;
        IsDamaging = false;
    }

    public void Initialize(float _damage)
    {
        Damage = _damage;
    }

    public void EndShockwave()
    {
        Destroy(gameObject);
    }
}