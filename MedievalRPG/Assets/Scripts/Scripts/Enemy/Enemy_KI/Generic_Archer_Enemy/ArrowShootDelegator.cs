using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShootDelegator : MonoBehaviour
{
    [SerializeField] private ArcherEnemyKI AI;
    public void ReleaseArrow() => AI.FireArrow();
    private void DisableAnimator()
    {
        AI.DisableAnimator();
        StartCoroutine(DespawnEnemy());
    }

    private IEnumerator DespawnEnemy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(transform.parent.gameObject);
    }
}
