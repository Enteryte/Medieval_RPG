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
    [SerializeField] private EnemyUI HealthUI;
    public float LifePoints { get; private set; }

    public void Initialize(EnemyBaseProfile _stats, Animator _anim, BaseEnemyKI _ai)
    {
        Stats = _stats;
        MaxLifePoints = Stats.normalHealth;
        LifePoints = Stats.normalHealth;
        Anim = _anim;
        AI = _ai;
        HealthUI.Init(MaxLifePoints, _stats.enemyName);
        
    }

    public void LightDamage(float _lightDamageTaken)
    {
        AI.UnusualNoticePlayerReaction();
        LifePoints -= _lightDamageTaken;
        HealthUI.HealthUpdate(LifePoints, false,_lightDamageTaken);
        if (!DeathCheck())
            Anim.SetTrigger(Animator.StringToHash("LightAttackTaken"));
    }

    public void HeavyDamage(float _heavyDamageTaken)
    {
        AI.UnusualNoticePlayerReaction();
        LifePoints -= _heavyDamageTaken;
        HealthUI.HealthUpdate(LifePoints, true,_heavyDamageTaken);
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

    public void BossHeal() => LifePoints = MaxLifePoints;
}