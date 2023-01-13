using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKI : BaseEnemyKI
{
    [SerializeField] private SideDetector[] SideDetectors;
    [SerializeField] private float MinDistanceForRangeMode;

    [SerializeField] private float LifePointsToPhaseSwitch;
    private bool IsInPhaseTwo = false;

    private bool IsOnSides;
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
        
    }
}
