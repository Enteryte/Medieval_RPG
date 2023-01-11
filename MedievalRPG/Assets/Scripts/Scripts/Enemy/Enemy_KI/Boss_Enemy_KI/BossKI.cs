using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKI : BaseEnemyKI
{
    [SerializeField] private float MinDistanceForRangeMode;


    private bool IsOnSides;
    public override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Death()
    {
        base.Death();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public void SetIsOnSides(bool _isOnSide)
    {
        IsOnSides = _isOnSide;
    }
    
    private IEnumerator AttackLoop()
    {
        yield return null;
    }
}
