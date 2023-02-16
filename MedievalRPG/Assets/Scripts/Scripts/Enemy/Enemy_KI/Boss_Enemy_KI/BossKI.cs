using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AttackType
{
    AOESweep,
    SideSweep,
    ShockWave,
    MeteorAndShield,
    ConeAttack,
    DoubleSweep,
    FruitNinja,
    BitchSlap,
    SlowAOE,
}

public class BossKI : BaseEnemyKI
{
    [Header("Includes")] 
    [SerializeField] private SideDetector[] SideDetectors;
    [SerializeField] private EnemyAttackController AttackController;
    [Header("Detectors")] 
    
    [SerializeField] private float MinDistanceForRangeMode;

    [SerializeField] private float LifePointsToPhaseSwitch;

    private bool IsOnSides;

    [Header("System Variables")]
    [SerializeField] private float CoolDownTime;

    private List<AttackType> AttackChoiceList;
    private bool IsInPhaseTwo = false;

//Strengths to determine how much influence a check should have on the list
    const int LowStrength = 1;
    const int MiddleStrength = 3;
    const int HighStrength = 5;

    public override void Init(EnemySpawner _mySpawner)
    {
        base.Init();
        for (int i = 0; i < SideDetectors.Length; i++)
            SideDetectors[i].Init(this);
    }

    protected override void Update()
    {
    }

    public override void Death()
    {
        base.Death();
    }

    protected override void OnDrawGizmos()
    {
    }

    public void SetIsOnSides(bool _isOnSide)
    {
        IsOnSides = _isOnSide;
    }

    private IEnumerator AttackLoop()
    {
        AttackChoiceList.Clear();
        yield return new WaitForSeconds(CoolDownTime);
        if (!IsInPhaseTwo)
        {
            if (Health.LifePoints >= LifePointsToPhaseSwitch)
            {
                Health.BossHeal();
                IsInPhaseTwo = true;
            }
            else
                AttackChoiceList = AttackCheckPhaseOne();
        }
        else
            AttackChoiceList = AttackCheckPhaseTwo();


        AttackType type = AttackChoiceList[Random.Range(0, AttackChoiceList.Count)];
        ChooseAttack(type);
    }

    /// <summary>
    /// The Function to generate the AttackList.
    /// It works by checking some of the checks and putting them into a list, which is later randomized from
    /// <para></para> However, if a trigger is successfully checked, it should add it along the Strengths I put above.
    /// That way, triggers can be weighted in the randomization.
    /// </summary>
    /// <returns></returns>
    private List<AttackType> AttackCheckPhaseOne()
    {
        List<AttackType> typeList = new List<AttackType>();
        if (IsOnSides)
            for (int i = 0; i < MiddleStrength; i++)
                typeList.Add(AttackType.SideSweep);


        return typeList;
    }
/// <summary>
/// Does the same as above, but is used for Phase 2 so that it includes the phase 2 attacks.
/// </summary>
/// <returns></returns>
    private List<AttackType> AttackCheckPhaseTwo()
    {
        List<AttackType> typeList = AttackCheckPhaseOne();


        return typeList;
    }

    private void ChooseAttack(AttackType _type)
    {
        switch (_type)
        {
            case AttackType.AOESweep:
                AttackController.AOESweep();
                break;
            case AttackType.SideSweep:
                AttackController.SideCleaning();
                break;
            case AttackType.ShockWave:
                AttackController.FireShockWave();
                break;
            case AttackType.MeteorAndShield:
                AttackController.DropMeteor();
                break;
            case AttackType.ConeAttack:
                AttackController.ConeBoneZone();
                break;
            case AttackType.DoubleSweep:
                AttackController.DoubleSweep();
                break;
            case AttackType.FruitNinja:
                break;
            case AttackType.BitchSlap:
                Animator.SetTrigger(Animator.StringToHash("AttackLaunch"));
                break;
            case AttackType.SlowAOE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
        }
    }
}