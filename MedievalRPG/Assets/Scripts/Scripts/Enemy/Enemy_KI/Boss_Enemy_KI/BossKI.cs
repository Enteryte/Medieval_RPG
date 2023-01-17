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
    [SerializeField] private SideDetector[] SideDetectors;
    [SerializeField] private float MinDistanceForRangeMode;

    [SerializeField] private float LifePointsToPhaseSwitch;
    private bool IsInPhaseTwo = false;

    private bool IsOnSides;


    private List<AttackType> AttackChoiceList;


    int SideStrength = 3;

    public override void Init()
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
        yield return null;
        if (!IsInPhaseTwo)
        {
            if (Health.LifePoints >= LifePointsToPhaseSwitch)
            {
                Health.BossHeal();
                IsInPhaseTwo = true;
            }
        }
        else
        {
        }

        AttackType type = AttackChoiceList[Random.Range(0, AttackChoiceList.Count)];
    }

    private List<AttackType> AttackCheckPhaseOne()
    {
        List<AttackType> typeList = new List<AttackType>();
        if (IsOnSides)
            for (int i = 0; i < SideStrength; i++)
                typeList.Add(AttackType.SideSweep);


        return typeList;
    }

    private List<AttackType> AttackCheckPhaseTwo()
    {
        List<AttackType> typeList = AttackCheckPhaseOne();


        return typeList;
    }
}