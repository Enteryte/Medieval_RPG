using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyBaseProfile Stats;
    private Animator Anim;
    private BaseEnemyKI AI;
    private float MaxLifePoints;
    public float LifePoints { get; private set; }

    public void Initialize(EnemyBaseProfile _stats, Animator _anim, BaseEnemyKI _ai)
    {
        Stats = _stats;
        MaxLifePoints = Stats.normalHealth;
        LifePoints = Stats.normalHealth;
        Anim = _anim;
        AI = _ai;
    }

    public void LightDamage(float _lightDamageTaken)
    {
        LifePoints -= _lightDamageTaken;
        Anim.SetTrigger(Animator.StringToHash("LightAttackTaken"));
        DeathCheck();
    }

    public void HeavyDamage(float _heavyDamageTaken)
    {
        LifePoints -= _heavyDamageTaken;
        Anim.SetTrigger(Animator.StringToHash("HeavyAttackTaken"));
        DeathCheck();
    }
    /// <summary>
    /// TODO: Create a base enemy both AI bases inherit from so this mess can be removed
    /// </summary>
    private void DeathCheck()
    {
        if(LifePoints > 0)
            return;
        AI.Death();
    }

    public void BossHeal()
    {
        LifePoints = MaxLifePoints;
    }
}
