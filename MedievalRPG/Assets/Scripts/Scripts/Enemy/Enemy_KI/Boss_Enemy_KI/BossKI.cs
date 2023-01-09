using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKI : BaseEnemyKI
{
    [SerializeField] private float MinDistanceForRangeMode;
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

    private IEnumerator AttackLoop()
    {
        yield return null;
    }
}
