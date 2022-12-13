using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyBaseProfile Stats;
    private Animator Anim;
    private GenericEnemyKi AI;
    private float LifePoints;
    public void Initialize(EnemyBaseProfile _stats, Animator _anim, GenericEnemyKi _ai)
    {
        Stats = _stats;
        LifePoints = Stats.normalHealth;
        Anim = _anim;
        AI = _ai;
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

    private void DeathCheck()
    {
        if(LifePoints > 0)
            return;
        AI.Death();
    }
}
