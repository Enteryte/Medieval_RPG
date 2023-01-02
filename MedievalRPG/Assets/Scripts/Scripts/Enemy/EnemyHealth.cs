using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyBaseProfile Stats;
    private Animator Anim;
    private BaseEnemyKI AI;
    private MeleeEnemyKi MeleeAI;
    private ArcherEnemyKI ArcherAI;
    private float LifePoints;
    public void Initialize(EnemyBaseProfile _stats, Animator _anim, BaseEnemyKi _ai)
    {
        Stats = _stats;
        LifePoints = Stats.normalHealth;
        Anim = _anim;
        AI = _ai;
    }
    public void InitializeMelee(EnemyBaseProfile _stats, Animator _anim, MeleeEnemyKi _ai)
    {
        Stats = _stats;
        LifePoints = Stats.normalHealth;
        Anim = _anim;
        MeleeAI = _ai;
    }
    public void InitializeArcher(EnemyBaseProfile _stats, Animator _anim, ArcherEnemyKI _ai)
    {
        Stats = _stats;
        LifePoints = Stats.normalHealth;
        Anim = _anim;
        ArcherAI = _ai;
    }

    public void LightDamage(float _lightDamageTaken)
    {
        LifePoints -= _lightDamageTaken;
        Anim.SetTrigger(Animator.StringToHash("LightAttackTaken"));
    }

    public void HeavyDamage(float _heavyDamageTaken)
    {
        
        LifePoints -= _heavyDamageTaken;
        Anim.SetTrigger(Animator.StringToHash("HeavyAttackTaken"));
    }
/// <summary>
/// TODO: Create a base enemy both AI bases inherit from so this mess can be removed
/// </summary>
    private void DeathCheck()
    {
        if(LifePoints > 0)
            return;
        MeleeAI.Death();
    }
}
