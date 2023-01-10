using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagerDelegator : MonoBehaviour
{
    [SerializeField] private EnemyDamager EnemyDamager;

    public void DamageOn() => EnemyDamager.DamageOn();
    public void DamageOff() => EnemyDamager.DamageOff();
}
