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

    public bool isDead = false;

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
        AI.GotHitReaction();
        LifePoints -= _lightDamageTaken;
        if (!DeathCheck())
            Anim.SetTrigger(Animator.StringToHash("LightAttackTaken"));
    }

    public void HeavyDamage(float _heavyDamageTaken)
    {
        AI.GotHitReaction();
        LifePoints -= _heavyDamageTaken;
        if (!DeathCheck())
            Anim.SetTrigger(Animator.StringToHash("HeavyAttackTaken"));
    }

    /// <summary>
    /// TODO: Create a base enemy both AI bases inherit from so this mess can be removed
    /// </summary>
    private bool DeathCheck()
    {
        if (LifePoints > 0)
            return false;
        AI.Death();
        return true;
    }

    public void BossHeal()
    {
        LifePoints = MaxLifePoints;
    }
}