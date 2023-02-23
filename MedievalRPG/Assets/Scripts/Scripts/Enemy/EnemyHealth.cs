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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] HitClips;
    [SerializeField] private AudioClip[] DeathClips;


    public float LifePoints { get; private set; }

    public void Initialize(EnemyBaseProfile _stats, Animator _anim, BaseEnemyKI _ai)
    {
        Stats = _stats;
        MaxLifePoints = Stats.normalHealth * DifficultyHandler.instance.enemyHpMultiplier;
        LifePoints = Stats.normalHealth;
        Anim = _anim;
        AI = _ai;
    }

    private void HitSound()
    {
        audioSource.clip = HitClips[UnityEngine.Random.Range(0, HitClips.Length - 1)];
        audioSource.Play();
    }
    private void DeathSound()
    {
        audioSource.clip = HitClips[UnityEngine.Random.Range(0, DeathClips.Length - 1)];
        audioSource.Play();
    }

    public void LightDamage(float _lightDamageTaken)
    {
        AI.UnusualNoticePlayerReaction();
        LifePoints -= _lightDamageTaken;
        if (!DeathCheck())
            Anim.SetTrigger(Animator.StringToHash("LightAttackTaken"));

        HitSound();
    }

    public void HeavyDamage(float _heavyDamageTaken)
    {
        AI.UnusualNoticePlayerReaction();
        LifePoints -= _heavyDamageTaken;
        if (!DeathCheck())
            Anim.SetTrigger(Animator.StringToHash("HeavyAttackTaken"));

        HitSound();
    }

    /// <summary>
    /// TODO: Create a base enemy both AI bases inherit from so this mess can be removed
    /// </summary>
    private bool DeathCheck()
    {
        if (LifePoints > 0)
            return false;
        AI.Death();
        DeathSound();
        return true;
    }

    public void BossHeal()
    {
        LifePoints = MaxLifePoints;
    }
}